import { Card, Flex, Space, Typography } from "antd"
import { useContext, useMemo, useState } from "react"
import { DataContext } from "../DataContext"
import { SummaryRow } from "../types"
import { timeOnlyFormatter, toggleLevel } from "../utils"
import { LogLevelTag } from "./LogLevelTag"
import { LogLevels } from "./LogLevels"
import { SearchContext } from "../SearchContext"

export const TestsList = ({ onTestClick }: { onTestClick: (summaryRow: SummaryRow) => void }) => {
	const { summaryRows, allLevels, allLevelsAndCounts } = useContext(DataContext)

	const [visibleLevels, setVisibleLevels] = useState(allLevels)

	const { searchKeyword } = useContext(SearchContext)

	const filteredSummaryRows = useMemo(() => {
		return summaryRows.filter(summaryRow => {
			if (!visibleLevels.includes(summaryRow.level)) return false
			if (searchKeyword.length < 2) return true
			return summaryRow.testCaseName.toLowerCase().includes(searchKeyword.toLowerCase()) || summaryRow.detailsRows.some(detailsRow => detailsRow.Message.toLowerCase().includes(searchKeyword.toLowerCase()))
		})
	}, [searchKeyword, summaryRows, visibleLevels])

	return (<>
		<Space direction="vertical" size="small">
			<div style={{ paddingLeft: '8px' }}>
				<LogLevels allLevels={allLevelsAndCounts} visibleLevels={visibleLevels} toggleLevel={level => setVisibleLevels(toggleLevel(level, visibleLevels))} />
			</div>

			{filteredSummaryRows.map(summaryRow => {
				const relativeDurationHack = Array.from({ length: summaryRow.relativeDuration }, (_value, index) => index)

				return (
					<Card key={summaryRow.testCaseName} hoverable size="small" className="testNameCard" onClick={() => onTestClick(summaryRow)}>
						<Flex align="center" gap="small">
							<LogLevelTag level={summaryRow.level} />
							<Typography.Text copyable style={{ fontWeight: 'bold', flex: '1' }}>{summaryRow.testCaseNameShort}</Typography.Text>
							<div style={{ display: 'flex' }} title={`Duration: ${summaryRow.durationStr} ${timeOnlyFormatter.format(summaryRow.startTime)} ... ${timeOnlyFormatter.format(summaryRow.endTime)}`}>
								<span style={{ marginTop: '8px', marginRight: '4px' }} className={summaryRow.level}>{relativeDurationHack.map(x => (<span key={x} className='durationMark'></span>))}</span>
								{summaryRow.durationStr}
							</div>
						</Flex>
					</Card>
				)
			})}
		</Space>
	</>)
}