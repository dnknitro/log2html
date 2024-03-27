import { Affix, Space, Typography } from "antd"
import { useState } from "react"
import { SummaryRow } from "../types"
import { toggleLevel } from "../utils"
import { DetailsRowUI } from "./DetailsRowUI"
import { LogLevels } from "./LogLevels"

export const DetailsTable = ({ summaryRow }: { summaryRow: SummaryRow }) => {
	const { detailsRows, detailsRowsLevels } = summaryRow

	const [visibleLevels, setVisibleLevels] = useState(detailsRowsLevels.map(([level]) => level).filter(level => level !== 'DEBUG'))

	// const { searchKeyword } = useContext(SearchContext)
	// const filteredDetailsRows = useMemo(() => {
	// 	return detailsRows.filter(detailsRow => {
	// 		if (!visibleLevels.includes(detailsRow.Level)) return false
	// 		if (searchKeyword.length < 2) return true
	// 		return detailsRow.Message.toLowerCase().includes(searchKeyword.toLowerCase())
	// 	})
	// }, [detailsRows, searchKeyword, visibleLevels])
	const filteredDetailsRows = detailsRows.filter(detailsRow => visibleLevels.includes(detailsRow.Level))


	return (<>
		<Space direction="vertical">
			<Typography.Title level={4} style={{ margin: 0 }}>{summaryRow.testCaseName}</Typography.Title>
			<Affix>
				<LogLevels allLevels={detailsRowsLevels} visibleLevels={visibleLevels} toggleLevel={level => setVisibleLevels(toggleLevel(level, visibleLevels))} />
			</Affix>
			<table className='details'>
				<tbody>
					{filteredDetailsRows.map(detailsRow => <DetailsRowUI key={detailsRow.ID} detailsRow={detailsRow} />)}
				</tbody>
			</table>
		</Space>
	</>)
}