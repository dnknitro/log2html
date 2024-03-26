export type ReportMetaData = {
	ReportName: string
	ReportEnvironment: string
	ReportStartDateTime: string
	ReportTitle: string
}

export type LogLevel = 'DEBUG' | 'PASS' | 'INFO' | 'FAILSKIP' | 'WARN' | 'RETRY' | 'FAIL' | 'ERROR' | 'FATAL'

export type DetailsRow = {
	ID: number
	Level: LogLevel
	LevelValue: number
	Message: string
	ThreadName: string
	StackTrace: string | null
	TimeStampUtc: Date // must be converted from string before usage
	ScreenshotPath: string | null
	Browser: string
	Exception: string | null
	TestCaseName: string
}

export type SummaryRow = {
	testCaseName: string
	testCaseNameShort: string
	startTime: Date,
	endTime: Date,
	level: LogLevel,
	tempBrowser?: string,
	durationStr: string,
	durationMsec: number
	relativeDuration: number
	detailsRows: DetailsRow[]
	uniqueStepNames: string[]
}
