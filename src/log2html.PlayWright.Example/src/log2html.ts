import { expect, test as base, type TestInfo } from '@playwright/test'

export type LogLevel = 'DEBUG' | 'INFO' | 'RETRY' | 'PASS' | 'WARN' | 'FAIL'

const log2htmlAttachmentName = 'log2html-record'
const levelValues: Record<LogLevel, number> = {
  DEBUG: 0,
  INFO: 1,
  RETRY: 2,
  PASS: 3,
  WARN: 4,
  FAIL: 5
}

type LogOptions = {
  browser?: string | null
  exception?: Error | string | null
  screenshotPath?: string | null
  stackTrace?: string | null
  testCaseName?: string | null
  threadName?: string | null
}

type Log2HtmlRecord = {
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

export class Log2HtmlReport {
  private lastTimestampMs = 0

  constructor(private readonly testInfo: TestInfo) {
  }

  debug(message: string, options?: LogOptions) {
    return this.log('DEBUG', message, options)
  }

  info(message: string, options?: LogOptions) {
    return this.log('INFO', message, options)
  }

  retry(message: string, options?: LogOptions) {
    return this.log('RETRY', message, options)
  }

  pass(message: string, options?: LogOptions) {
    return this.log('PASS', message, options)
  }

  warn(message: string, options?: LogOptions) {
    return this.log('WARN', message, options)
  }

  fail(message: string, options?: LogOptions) {
    return this.log('FAIL', message, options)
  }

  json(value: unknown, level: LogLevel = 'INFO', options?: LogOptions) {
    return this.log(level, asCode(JSON.stringify(value, null, 2), 'json'), options)
  }

  async log(level: LogLevel, message: string, options?: LogOptions) {
    const exception = options?.exception ?? null
    const record: Log2HtmlRecord = {
      ID: 0,
      Level: level,
      LevelValue: levelValues[level],
      Message: message,
      ThreadName: options?.threadName ?? `Worker#${this.testInfo.workerIndex}`,
      StackTrace: options?.stackTrace ?? (exception instanceof Error ? exception.stack ?? null : null),
      TimeStampUtc: this.nextTimestampUtc(),
      ScreenshotPath: options?.screenshotPath ?? null,
      Browser: options?.browser ?? this.testInfo.project.name,
      Exception: exception instanceof Error ? exception.toString() : exception,
      TestCaseName: options?.testCaseName ?? testCaseName(this.testInfo)
    }

    await this.testInfo.attach(log2htmlAttachmentName, {
      body: Buffer.from(JSON.stringify(record), 'utf8'),
      contentType: 'application/json'
    })
  }

  private nextTimestampUtc() {
    const nextTimestampMs = Math.max(Date.now(), this.lastTimestampMs + 1)
    this.lastTimestampMs = nextTimestampMs
    return new Date(nextTimestampMs).toISOString()
  }
}

export const test = base.extend<{ report: Log2HtmlReport }>({
  report: async ({}, use, testInfo) => {
    await use(new Log2HtmlReport(testInfo))
  }
})

export { expect }

function asCode(code: string, language?: string) {
  const languageClass = language ? ` class="language-${language}"` : ''
  return `<pre><code${languageClass}>${escapeHtml(code)}</code></pre>`
}

function escapeHtml(value: string) {
  return value
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
}

function testCaseName(testInfo: TestInfo) {
  return testInfo.titlePath.filter(Boolean).join(' > ')
}
