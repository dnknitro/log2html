import { Flex, Typography } from "antd"
import { useContext } from "react"

import { DataContext } from "../DataContext"
import { calcDuration, timeOnlyFormatter } from "../utils"

export const TestsHeader = () => {
	const { reportMetaData, reportStartTime, reportEndTime/*, summaryRows, levelsAndBrowsers*/ } = useContext(DataContext)

	return (<>
		<Typography.Title level={2}>{reportMetaData.ReportName}</Typography.Title>
		<Typography.Title level={5}>
			<label>Environment:</label> {reportMetaData.ReportEnvironment}
		</Typography.Title>
		<Typography.Title level={5}>
			<Flex gap="middle" align='flex-start'>
				<span className='nowrap'><label>Start:</label> {timeOnlyFormatter.format(reportStartTime)}</span>
				<span className='nowrap'><label>End:</label> {timeOnlyFormatter.format(reportEndTime)}</span>
				<span className='nowrap'><label>Duration:</label> {calcDuration(reportStartTime, reportEndTime)}</span>
			</Flex>
		</Typography.Title>
	</>)
}