import fs from 'node:fs'
import path from 'node:path'
import { pathToFileURL } from 'node:url'

import type {
  FullConfig,
  FullResult,
  Reporter,
  TestCase,
  TestResult,
  TestStep
} from '@playwright/test/reporter'

type LogLevel = 'DEBUG' | 'INFO' | 'RETRY' | 'PASS' | 'WARN' | 'FAIL'

type ReporterOptions = {
  environment?: string
  outputFile?: string
  reportName?: string
  stepCategories?: string[]
  templatePath?: string
}

type ReportMetaData = {
  ReportName: string
  ReportEnvironment: string
  ReportStartDateTime: string
  ReportTitle: string
}

type ReportEntry = {
  ID: number
  Level: LogLevel
  LevelValue: number
  Message: string
  ThreadName: string
  StackTrace: string | null
  TimeStampUtc: string
  ScreenshotPath: string | null
  Browser: string
  Exception: string | null
  TestCaseName: string
}

type TimedReportEntry = {
  entry: ReportEntry
  order: number
}

const log2htmlAttachmentName = 'log2html-record'
const detailsRowsPlaceholder = '/*detailsRows*/'
const reportMetaDataPlaceholder = '{/*reportMetaData*/ }'
const reportSummaryPlaceholder = '{Server Side LevelsBrowsers}'
const levelValues: Record<LogLevel, number> = {
  DEBUG: 0,
  INFO: 1,
  RETRY: 2,
  PASS: 3,
  WARN: 4,
  FAIL: 5
}
const valueLevels = new Map<number, LogLevel>(
  Object.entries(levelValues).map(([level, value]) => [value, level as LogLevel])
)

export default class Log2HtmlReporter implements Reporter {
  private readonly options: Required<Pick<ReporterOptions, 'environment' | 'reportName' | 'stepCategories'>> & ReporterOptions
  private readonly entries: TimedReportEntry[] = []
  private configDir = process.cwd()
  private order = 0
  private outputFile = ''
  private reportStartDateTime = new Date()
  private templatePath = ''

  constructor(options: ReporterOptions = {}) {
    this.options = {
      environment: options.environment ?? process.env.NODE_ENV ?? 'DEV',
      outputFile: options.outputFile,
      reportName: options.reportName ?? 'log2html Playwright Execution Report',
      stepCategories: options.stepCategories ?? ['test.step', 'expect', 'pw:api'],
      templatePath: options.templatePath
    }
  }

  onBegin(config: FullConfig) {
    const configFile = (config as FullConfig & { configFile?: string }).configFile
    this.configDir = configFile ? path.dirname(configFile) : process.cwd()
    const reportMetaData = this.createReportMetaData()
    this.outputFile = path.resolve(
      this.configDir,
      this.options.outputFile ?? path.join('..', 'Results', `${this.defaultReportFileName()}.html`)
    )
    this.templatePath = path.resolve(
      this.configDir,
      this.options.templatePath ?? path.join('..', 'log2html', 'ReportTemplate.html')
    )
  }

  onStepEnd(test: TestCase, result: TestResult, step: TestStep) {
    if (!this.options.stepCategories.includes(step.category)) {
      return
    }

    const pathTitle = stepTitlePath(step)
    const duration = typeof step.duration === 'number' ? ` (${step.duration} ms)` : ''
    const error = step.error ?? null
    const level = stepLevel(step)
    this.addEntry({
      ID: 0,
      Level: error ? 'FAIL' : level,
      LevelValue: error ? levelValues.FAIL : levelValues[level],
      Message: `${stepMessagePrefix(step, error)}: ${escapeHtml(pathTitle)}${duration}`,
      ThreadName: `Worker#${result.workerIndex}`,
      StackTrace: error?.stack ?? null,
      TimeStampUtc: step.startTime instanceof Date ? step.startTime.toISOString() : new Date().toISOString(),
      ScreenshotPath: null,
      Browser: projectName(test),
      Exception: error ? testErrorToString(error) : null,
      TestCaseName: testCaseName(test)
    })
  }

  onTestEnd(test: TestCase, result: TestResult) {
    const beforeAttachmentsCount = this.entries.length

    for (const attachment of result.attachments) {
      if (attachment.name !== log2htmlAttachmentName) {
        continue
      }

      const records = parseAttachmentRecords(attachment)
      for (const record of records) {
        this.addEntry(normalizeEntry(record, test, result))
      }
    }

    if (result.status === 'passed' && !this.hasTerminalEntryFor(test, beforeAttachmentsCount)) {
      this.addEntry(createStatusEntry('PASS', 'Test passed', test, result))
    }

    if (result.status !== 'passed' && result.status !== 'skipped') {
      this.addEntry(createFailureEntry(test, result))
    }

    if (result.status === 'skipped' && !this.hasAnyEntryFor(test)) {
      this.addEntry(createStatusEntry('WARN', 'Test skipped', test, result))
    }
  }

  async onEnd(result: FullResult) {
    fs.mkdirSync(path.dirname(this.outputFile), { recursive: true })

    const template = fs.readFileSync(this.templatePath, 'utf8')
    const entries = this.entries
      .sort((a, b) => {
        const timeDiff = Date.parse(a.entry.TimeStampUtc) - Date.parse(b.entry.TimeStampUtc)
        return timeDiff === 0 ? a.order - b.order : timeDiff
      })
      .map(({ entry }, index) => ({ ...entry, ID: index + 1 }))

    const reportMetaData = this.createReportMetaData()
    const detailsRowsJson = entries.map(entry => JSON.stringify(entry)).join(`,\n\t\t\t`)
    const levelsSummaryJson = JSON.stringify(createLevelsSummary(entries))
    const html = template
      .replace(reportMetaDataPlaceholder, JSON.stringify(reportMetaData))
      .replace(detailsRowsPlaceholder, detailsRowsJson)
      .replace(reportSummaryPlaceholder, levelsSummaryJson)

    fs.writeFileSync(this.outputFile, html, 'utf8')
    console.log(`\nlog2html report: ${this.outputFile}`)

    if (result.status !== 'passed') {
      console.log(`log2html captured a ${result.status} Playwright run.`)
    }
  }

  private addEntry(entry: ReportEntry) {
    this.entries.push({ entry, order: this.order++ })
  }

  private hasTerminalEntryFor(test: TestCase, sinceEntryIndex = 0) {
    const name = testCaseName(test)
    return this.entries
      .slice(sinceEntryIndex)
      .some(({ entry }) => entry.TestCaseName === name && ['PASS', 'FAIL', 'WARN', 'RETRY'].includes(entry.Level))
  }

  private hasAnyEntryFor(test: TestCase) {
    const name = testCaseName(test)
    return this.entries.some(({ entry }) => entry.TestCaseName === name)
  }

  private createReportMetaData(): ReportMetaData {
    const reportStartDateTime = this.reportStartDateTime.toISOString()
    return {
      ReportName: this.options.reportName,
      ReportEnvironment: this.options.environment,
      ReportStartDateTime: reportStartDateTime,
      ReportTitle: `${timeOnly(this.reportStartDateTime)} ${this.options.reportName} ${this.options.environment}`
    }
  }

  private defaultReportFileName() {
    return sanitizeFileName(`${dateTimeForFileName(this.reportStartDateTime)} ${this.options.reportName} ${this.options.environment}`)
  }
}

function normalizeEntry(record: Partial<ReportEntry>, test: TestCase, result: TestResult): ReportEntry {
  const level = normalizeLevel(record.Level)
  return {
    ID: 0,
    Level: level,
    LevelValue: typeof record.LevelValue === 'number' ? record.LevelValue : levelValues[level],
    Message: record.Message ?? '',
    ThreadName: record.ThreadName ?? `Worker#${result.workerIndex}`,
    StackTrace: record.StackTrace ?? null,
    TimeStampUtc: record.TimeStampUtc ?? new Date().toISOString(),
    ScreenshotPath: record.ScreenshotPath ?? null,
    Browser: record.Browser ?? projectName(test),
    Exception: record.Exception ?? null,
    TestCaseName: record.TestCaseName ?? testCaseName(test)
  }
}

function createStatusEntry(level: LogLevel, message: string, test: TestCase, result: TestResult): ReportEntry {
  return {
    ID: 0,
    Level: level,
    LevelValue: levelValues[level],
    Message: message,
    ThreadName: `Worker#${result.workerIndex}`,
    StackTrace: null,
    TimeStampUtc: new Date().toISOString(),
    ScreenshotPath: screenshotPath(result),
    Browser: projectName(test),
    Exception: null,
    TestCaseName: testCaseName(test)
  }
}

function createFailureEntry(test: TestCase, result: TestResult): ReportEntry {
  const error = result.error ?? result.errors[0] ?? null
  return {
    ...createStatusEntry('FAIL', error?.message ?? `Test ${result.status}`, test, result),
    StackTrace: error?.stack ?? null,
    ScreenshotPath: screenshotPath(result),
    Exception: error ? testErrorToString(error) : null
  }
}

function createLevelsSummary(entries: ReportEntry[]) {
  const testMaxLevelValues = new Map<string, number>()

  for (const entry of entries) {
    const previous = testMaxLevelValues.get(entry.TestCaseName) ?? Number.NEGATIVE_INFINITY
    testMaxLevelValues.set(entry.TestCaseName, Math.max(previous, entry.LevelValue))
  }

  const countsByLevelValue = new Map<number, { LevelValue: number, Level: LogLevel, Count: number }>()
  for (const levelValue of testMaxLevelValues.values()) {
    const summary = countsByLevelValue.get(levelValue) ?? {
      LevelValue: levelValue,
      Level: valueLevels.get(levelValue) ?? 'INFO',
      Count: 0
    }
    summary.Count += 1
    countsByLevelValue.set(levelValue, summary)
  }

  return [...countsByLevelValue.values()].sort((a, b) => a.LevelValue - b.LevelValue)
}

function parseAttachmentRecords(attachment: TestResult['attachments'][number]): Partial<ReportEntry>[] {
  const text = attachment.body != null
    ? attachment.body.toString('utf8')
    : attachment.path != null
      ? fs.readFileSync(attachment.path, 'utf8')
      : null

  if (text == null) {
    return []
  }

  const parsed = JSON.parse(text) as Partial<ReportEntry> | Partial<ReportEntry>[]
  return Array.isArray(parsed) ? parsed : [parsed]
}

function normalizeLevel(level: unknown): LogLevel {
  if (typeof level === 'string') {
    const normalized = level.toUpperCase()
    if (normalized in levelValues) {
      return normalized as LogLevel
    }
  }

  return 'INFO'
}

function stepLevel(step: TestStep): LogLevel {
  return step.category === 'pw:api' ? 'DEBUG' : 'INFO'
}

function stepMessagePrefix(step: TestStep, error: TestStep['error'] | null) {
  if (error) {
    return 'Step failed'
  }

  return step.category === 'pw:api' ? 'Playwright API' : 'Step'
}

function stepTitlePath(step: TestStep) {
  const titles: string[] = []
  let current: TestStep | undefined = step

  while (current) {
    if (current.title) {
      titles.unshift(current.title)
    }
    current = (current as TestStep & { parent?: TestStep }).parent
  }

  return titles.join(' > ')
}

function screenshotPath(result: TestResult) {
  const screenshot = result.attachments.find(attachment => attachment.name === 'screenshot' && attachment.path)
  return screenshot?.path ? pathToFileURL(path.resolve(screenshot.path)).href : null
}

function testCaseName(test: TestCase) {
  const parts = test.titlePath().filter(Boolean)
  const project = projectName(test)
  return (parts[0] === project ? parts.slice(1) : parts).join(' > ')
}

function projectName(test: TestCase) {
  return test.parent.project()?.name ?? ''
}

function testErrorToString(error: NonNullable<TestResult['error']>) {
  return [error.message, error.stack].filter(Boolean).join('\n\n')
}

function escapeHtml(value: string) {
  return value
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
}

function timeOnly(date: Date) {
  return `${pad(date.getHours())}:${pad(date.getMinutes())}`
}

function dateTimeForFileName(date: Date) {
  return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())} ${pad(date.getHours())}.${pad(date.getMinutes())}.${pad(date.getSeconds())}`
}

function pad(value: number) {
  return value.toString().padStart(2, '0')
}

function sanitizeFileName(value: string) {
  return value
    .replace(/[<>:"/\\|?*\u0000-\u001F]/g, '.')
    .replace(/\.+/g, '.')
    .trim()
}
