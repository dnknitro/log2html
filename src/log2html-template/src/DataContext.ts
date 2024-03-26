import { createContext } from 'react'
import { DetailsRow, LogLevel, ReportMetaData, SummaryRow } from './types'
import groupBy from 'lodash/groupBy'
import minBy from 'lodash/minBy'
import maxBy from 'lodash/maxBy'
import { calcDuration } from './utils'

// eslint-disable-next-line @typescript-eslint/no-explicit-any
let reportMetaData: ReportMetaData = (window as any).reportMetaData
if (!reportMetaData.ReportName && process.env.NODE_ENV === 'development') {
	const testReportName = `${process.env.NODE_ENV} Test Execution Report`
	reportMetaData = {
		"ReportName": testReportName,
		"ReportEnvironment": `${process.env.NODE_ENV}`,
		"ReportStartDateTime": new Date().toString(),
		"ReportTitle": testReportName + ' Title',
	}
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
let detailsRows = ((window as any).detailsRows as DetailsRow[])

if (!detailsRows.length) {
	detailsRows = (process.env.NODE_ENV !== 'development'
		// no data
		? [
			{ "ID": 1, "Level": "INFO", "LevelValue": 40000, "Message": "NO log records", "ThreadName": "", "StackTrace": null, "TimeStampUtc": new Date().toISOString(), "ScreenshotPath": null, "Browser": "", "Exception": null, "TestCaseName": "NO log records" },
		]
		// test data
		: [
			{ "ID": 2, "Level": "INFO", "LevelValue": 40000, "Message": "TestAppend1: Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", "ThreadName": "ParallelWorker#1", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:38.2117412Z", "ScreenshotPath": null, "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(1)" },
			{ "ID": 3, "Level": "INFO", "LevelValue": 40000, "Message": "TestAppend1: Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", "ThreadName": "ParallelWorker#4", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:38.2117412Z", "ScreenshotPath": null, "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(3)" },
			{ "ID": 4, "Level": "INFO", "LevelValue": 40000, "Message": "TestAppend1: Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:38.2117412Z", "ScreenshotPath": null, "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(4)" },
			{ "ID": 5, "Level": "PASS", "LevelValue": 30000, "Message": "TestAppend1: Debug", "ThreadName": "ParallelWorker#2", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:38.6786882Z", "ScreenshotPath": "http://opspl.com/wp-content/uploads/2017/01/automation-vs-manual-testing.gif", "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(2)" },
			{ "ID": 6, "Level": "DEBUG", "LevelValue": 30000, "Message": "TestAppend1: Debug", "ThreadName": "ParallelWorker#1", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:39.5899008Z", "ScreenshotPath": "http://opspl.com/wp-content/uploads/2017/01/automation-vs-manual-testing.gif", "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(1)" },
			{ "ID": 7, "Level": "DEBUG", "LevelValue": 30000, "Message": "TestAppend1: Debug", "ThreadName": "ParallelWorker#4", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:39.7698994Z", "ScreenshotPath": "http://opspl.com/wp-content/uploads/2017/01/automation-vs-manual-testing.gif", "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(3)" },
			{ "ID": 8, "Level": "DEBUG", "LevelValue": 30000, "Message": "TestAppend1: Debug", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:40.0866758Z", "ScreenshotPath": "http://opspl.com/wp-content/uploads/2017/01/automation-vs-manual-testing.gif", "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(4)" },
			{ "ID": 9, "Level": "PASS", "LevelValue": 55000, "Message": "No Fails!", "ThreadName": "ParallelWorker#2", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:40.7480408Z", "ScreenshotPath": null, "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(2)" },
			{ "ID": 10, "Level": "PASS", "LevelValue": 55000, "Message": "No Fails!", "ThreadName": "ParallelWorker#1", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:41.1270541Z", "ScreenshotPath": null, "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(1)" },
			{ "ID": 11, "Level": "PASS", "LevelValue": 55000, "Message": "No Fails!", "ThreadName": "ParallelWorker#4", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:41.7622474Z", "ScreenshotPath": null, "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(3)" },
			{ "ID": 12, "Level": "PASS", "LevelValue": 55000, "Message": "No Fails!", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:42.5403783Z", "ScreenshotPath": null, "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(4)" },
			{ "ID": 13, "Level": "INFO", "LevelValue": 40000, "Message": "TestAppend1: Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", "ThreadName": "ParallelWorker#2", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:42.9848336Z", "ScreenshotPath": null, "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(5)" },
			{ "ID": 14, "Level": "INFO", "LevelValue": 40000, "Message": "TestAppend1: Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", "ThreadName": "ParallelWorker#1", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:43.0988322Z", "ScreenshotPath": null, "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(6)" },
			{ "ID": 15, "Level": "INFO", "LevelValue": 40000, "Message": "TestAppend2: Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", "ThreadName": "ParallelWorker#4", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:43.6045741Z", "ScreenshotPath": null, "Browser": "FireFox", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend2" },
			{ "ID": 16, "Level": "WARN", "LevelValue": 60000, "Message": "TestAppend3: Warn test", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:44.2036445Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 17, "Level": "DEBUG", "LevelValue": 30000, "Message": "TestAppend1: Debug", "ThreadName": "ParallelWorker#2", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:45.1730337Z", "ScreenshotPath": "http://opspl.com/wp-content/uploads/2017/01/automation-vs-manual-testing.gif", "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(5)" },
			{ "ID": 18, "Level": "DEBUG", "LevelValue": 30000, "Message": "TestAppend1: Debug", "ThreadName": "ParallelWorker#1", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:46.0792519Z", "ScreenshotPath": "http://opspl.com/wp-content/uploads/2017/01/automation-vs-manual-testing.gif", "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(6)" },
			{ "ID": 19, "Level": "FAIL", "LevelValue": 75000, "Message": "TestAppend2: Fail Skip test", "ThreadName": "ParallelWorker#4", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:46.4143842Z", "ScreenshotPath": null, "Browser": "FireFox", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend2" },
			{ "ID": 20, "Level": "DEBUG", "LevelValue": 30000, "Message": "TestAppend3: Debug", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:47.0936321Z", "ScreenshotPath": "http://opspl.com/wp-content/uploads/2017/01/automation-vs-manual-testing.gif", "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 21, "Level": "FAIL", "LevelValue": 75000, "Message": "test exception", "ThreadName": "ParallelWorker#2", "StackTrace": "   at DynamicAppenderTestFixture.TestAppend1(Int32 index) in c:\\Projects\\R&D\\log2html\\log2html.Test\\DynamicAppenderTestFixture.cs:line 31", "TimeStampUtc": "2018-05-23T05:10:47.2508532Z", "ScreenshotPath": null, "Browser": "IE", "Exception": "An exception of type 'System.InvalidOperationException' occurred and was caught.\r\n................................................................................\r\n05/23/2018 00:10:50\r\nType: System.InvalidOperationException, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\r\nMessage: test exception\r\nSource: log2html.Test\r\nData: System.Collections.ListDictionaryInternal\r\nTargetSite: Void TestAppend1(Int32)\r\nHResult: -2146233079\r\nStack Trace: \r\n   at DynamicAppenderTestFixture.TestAppend1(Int32 index) in c:\\Projects\\R&D\\log2html\\log2html.Test\\DynamicAppenderTestFixture.cs:line 31\r\n\r\n\r\nData[ListDictionaryInternal][6]=\r\n[\r\n\t\"MachineName\"[String]=DNK1\r\n\t\"OS Version\"[String]=Microsoft Windows NT 10.0.17134.0\r\n\t\"AssemblyFullName\"[String]=dnkUtils, Version=1.0.0.42065, Culture=neutral, PublicKeyToken=null\r\n\t\"Thread\"[String]=[20]\r\n\t\"WindowsIdentity\"[String]=DNK1\\dnknitro\r\n]\r\n\r\n", "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(5)" },
			{ "ID": 22, "Level": "FAIL", "LevelValue": 75000, "Message": "test exception", "ThreadName": "ParallelWorker#1", "StackTrace": "   at DynamicAppenderTestFixture.TestAppend1(Int32 index) in c:\\Projects\\R&D\\log2html\\log2html.Test\\DynamicAppenderTestFixture.cs:line 31", "TimeStampUtc": "2018-05-23T05:10:47.840745Z", "ScreenshotPath": null, "Browser": "IE", "Exception": "An exception of type 'System.InvalidOperationException' occurred and was caught.\r\n................................................................................\r\n05/23/2018 00:10:50\r\nType: System.InvalidOperationException, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\r\nMessage: test exception\r\nSource: log2html.Test\r\nData: System.Collections.ListDictionaryInternal\r\nTargetSite: Void TestAppend1(Int32)\r\nHResult: -2146233079\r\nStack Trace: \r\n   at DynamicAppenderTestFixture.TestAppend1(Int32 index) in c:\\Projects\\R&D\\log2html\\log2html.Test\\DynamicAppenderTestFixture.cs:line 31\r\n\r\n\r\nData[ListDictionaryInternal][6]=\r\n[\r\n\t\"MachineName\"[String]=DNK1\r\n\t\"OS Version\"[String]=Microsoft Windows NT 10.0.17134.0\r\n\t\"AssemblyFullName\"[String]=dnkUtils, Version=1.0.0.42065, Culture=neutral, PublicKeyToken=null\r\n\t\"Thread\"[String]=[19]\r\n\t\"WindowsIdentity\"[String]=DNK1\\dnknitro\r\n]\r\n\r\n", "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(6)" },
			{ "ID": 23, "Level": "INFO", "LevelValue": 40000, "Message": "TestAppend4: Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", "ThreadName": "ParallelWorker#4", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:48.7122348Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend4" },
			{ "ID": 24, "Level": "INFO", "LevelValue": 40000, "Message": "2147483647 = OFF", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:49.5152995Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 25, "Level": "PASS", "LevelValue": 55000, "Message": "TestAppend4: Fail Skip test", "ThreadName": "ParallelWorker#4", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:51.4029348Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend4" },
			{ "ID": 26, "Level": "INFO", "LevelValue": 40000, "Message": "-2147483648 = ALL", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:52.3991608Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 27, "Level": "INFO", "LevelValue": 40000, "Message": "10000 = VERBOSE", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:53.3823509Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 28, "Level": "INFO", "LevelValue": 40000, "Message": "20000 = FINER", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:53.7856297Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 29, "Level": "INFO", "LevelValue": 40000, "Message": "20000 = TRACE", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:54.4653032Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 30, "Level": "INFO", "LevelValue": 40000, "Message": "30000 = FINE", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:55.3460892Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 31, "Level": "DEBUG", "LevelValue": 40000, "Message": "30000 = DEBUG", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:56.176832Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 32, "Level": "INFO", "LevelValue": 40000, "Message": "40000 = INFO", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:56.8895708Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 33, "Level": "INFO", "LevelValue": 40000, "Message": "50000 = NOTICE", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:57.2889365Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 34, "Level": "INFO", "LevelValue": 40000, "Message": "10000 = FINEST", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:57.4030151Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 35, "Level": "INFO", "LevelValue": 40000, "Message": "70000 = ERROR", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:58.1916037Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 36, "Level": "INFO", "LevelValue": 40000, "Message": "80000 = SEVERE", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:58.2986772Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 37, "Level": "INFO", "LevelValue": 40000, "Message": "90000 = CRITICAL", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:58.5516766Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 38, "Level": "INFO", "LevelValue": 40000, "Message": "100000 = ALERT", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:58.7881044Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 39, "Level": "INFO", "LevelValue": 40000, "Message": "110000 = FATAL", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:59.156211Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 40, "Level": "RETRY", "LevelValue": 40000, "Message": "120000 = EMERGENCY", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:59.5575047Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 41, "Level": "FAILSKIP", "LevelValue": 40000, "Message": "120000 = log4net:DEBUG", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:11:00.0628846Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 42, "Level": "INFO", "LevelValue": 40000, "Message": "60000 = WARN", "ThreadName": "ParallelWorker#3", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:11:00.4649914Z", "ScreenshotPath": null, "Browser": "Chrome", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend3" },
			{ "ID": 43, "Level": "PASS", "LevelValue": 55000, "Message": "<pre><code class=\"language-json\">[{\r\n    \"debug\": \"on\",\r\n    \"window\": {\r\n        \"title\": \"Sample Konfabulator Widget\",\r\n        \"name\": \"main_window\",\r\n        \"width\": 500,\r\n        \"height\": 500,\r\n        \"active\": true\r\n    }\r\n},{\r\n    \"debug\": \"on\",\r\n    \"window\": {\r\n        \"title\": \"Sample Konfabulator Widget\",\r\n        \"name\": \"main_window\",\r\n        \"width\": 500,\r\n        \"height\": 500,\r\n        \"active\": true\r\n    }\r\n},{\r\n    \"debug\": \"on\",\r\n    \"window\": {\r\n        \"title\": \"Sample Konfabulator Widget\",\r\n        \"name\": \"main_window\",\r\n        \"width\": 500,\r\n        \"height\": 500,\r\n        \"active\": true\r\n    }\r\n},{\r\n    \"debug\": \"on\",\r\n    \"window\": {\r\n        \"title\": \"Sample Konfabulator Widget\",\r\n        \"name\": \"main_window\",\r\n        \"width\": 500,\r\n        \"height\": 500,\r\n        \"active\": true\r\n    }\r\n}]</code></pre>", "ThreadName": "ParallelWorker#1", "StackTrace": null, "TimeStampUtc": "2018-05-23T05:10:42.1270541Z", "ScreenshotPath": null, "Browser": "IE", "Exception": null, "TestCaseName": "DynamicAppenderTestFixture.TestAppend1(1)" },
		]) as unknown as DetailsRow[]
}

detailsRows.forEach(x => {
	x.TimeStampUtc = new Date(x.TimeStampUtc)
})

const uniqueLevels = Object.keys(groupBy(detailsRows, 'Level')) as LogLevel[]

const groupsObject = groupBy(detailsRows, 'TestCaseName')

const summaryRows = Object.values(groupsObject)
	.map((group): SummaryRow => {
		const minRow = minBy(group, 'TimeStampUtc') as DetailsRow
		const maxRow = maxBy(group, 'TimeStampUtc') as DetailsRow

		const startTime = minRow.TimeStampUtc
		const endTime = maxRow.TimeStampUtc

		let lastRetryRowIndex = 0
		for (let i = group.length - 1; i >= 0; i--) {
			if (group[i].Level !== "RETRY") continue
			lastRetryRowIndex = i
			break
		}


		let level = group
			.slice(lastRetryRowIndex) // ignoring records prior RETRY
			.reduce((accumulator, currentValue) => currentValue.LevelValue > accumulator.LevelValue ? currentValue : accumulator, group[0])
			.Level
		if (level === 'PASS' && lastRetryRowIndex > 0) {
			level = 'RETRY'
		}

		const testCaseName = group[0].TestCaseName
		let testCaseNameShort = testCaseName
		const testCaseNameRegex = /^(?:(?:\w+\.)*)((?:[^.]\w+\.\w+).*?)$/g
		const matches = testCaseNameRegex.exec(testCaseNameShort)
		if (matches != null) testCaseNameShort = matches[1]

		const durationStr = calcDuration(startTime, endTime)

		return ({
			testCaseName,
			testCaseNameShort,
			startTime,
			endTime,
			level,
			tempBrowser: group[0].Browser,
			durationStr,
			durationMsec: endTime.getTime() - startTime.getTime(),
			relativeDuration: 0,
			detailsRows: group,
			uniqueStepNames: Object.keys(groupBy(group, 'Level'))
		})
	})
	.sort((a, b) => (a.testCaseName > b.testCaseName) ? 1 : -1)

const summaryRowsGroups = groupBy(summaryRows, 'level')

const maxDurationSummaryRow = maxBy(summaryRows, 'durationMsec') as SummaryRow

summaryRows.forEach(summaryRow => {
	summaryRow.relativeDuration = Math.round(summaryRow.durationMsec / maxDurationSummaryRow.durationMsec * 9) + 1
})

const reportStartTime = (minBy(detailsRows, 'TimeStampUtc') as DetailsRow).TimeStampUtc
const reportEndTime = (maxBy(detailsRows, 'TimeStampUtc') as DetailsRow).TimeStampUtc

const levelsAndBrowsers = new Map<string, { Count: number, Level: LogLevel, Browser: string }>()
summaryRows.map(item => {
	let key = item.level
	if (item.tempBrowser) key = key + ' ' + item.tempBrowser

	const current = levelsAndBrowsers.get(key)
	if (!current) {
		levelsAndBrowsers.set(key, { Count: 1, Level: item.level, Browser: item.tempBrowser ?? '' })
	} else {
		current.Count++
	}
})

export const defaultDataContextData = {
	reportMetaData,
	reportStartTime,
	reportEndTime,
	levelsAndBrowsers,
	uniqueLevels,
	summaryRowsGroups,
	summaryRows,
	detailsRows,
}

export const DataContext = createContext(defaultDataContextData)
