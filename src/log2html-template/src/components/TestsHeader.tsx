import { Flex, Input, Typography } from "antd"
import { memo, useContext } from "react"

import { DataContext } from "../DataContext"
import { SearchContext } from "../SearchContext"
import { calcDuration, timeOnlyFormatter } from "../utils"
import { ChartUI } from "./ChartUI"

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const htmlVersion = (window as any).reportVersion

export const TestsHeader = () => {
	const { reportMetaData, reportStartTime, reportEndTime/*, summaryRows, levelsAndBrowsers*/ } = useContext(DataContext)
	const { searchKeyword, setSearchKeyword } = useContext(SearchContext)

	const ChartUIMemo = memo(ChartUI)

	return (
		<Flex align='stretch'>
			<div>
				<Typography.Title style={{ margin: 0 }} level={2} title={`report JS version ${import.meta.env.VITE_REACT_APP_VERSION}; HTML version ${htmlVersion}`}>{reportMetaData.ReportName}</Typography.Title>
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
				<Input placeholder="type to search" style={{ width: 300 }} autoFocus allowClear value={searchKeyword} onChange={e => setSearchKeyword(e.target.value)} />
			</div>
			<ChartUIMemo />
		</Flex>
	)
}