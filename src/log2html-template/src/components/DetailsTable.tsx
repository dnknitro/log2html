import { useContext, useMemo, useState } from "react"
import { toggleLevel } from "../utils"
import { DetailsRowUI } from "./DetailsRowUI"
import { LogLevels } from "./LogLevels"
import { Affix } from "antd"
import { SummaryRow } from "../types"
import { SearchContext } from "../SearchContext"

export const DetailsTable = ({ summaryRow }: { summaryRow: SummaryRow }) => {
	const { detailsRows, detailsRowsLevels } = summaryRow

	const [visibleLevels, setVisibleLevels] = useState(detailsRowsLevels.map(([level]) => level).filter(level => level !== 'DEBUG'))

	const { searchKeyword } = useContext(SearchContext)

	const filteredDetailsRows = useMemo(() => {
		return detailsRows.filter(detailsRow => {
			if (!visibleLevels.includes(detailsRow.Level)) return false
			if (searchKeyword.length < 2) return true
			return detailsRow.Message.toLowerCase().includes(searchKeyword.toLowerCase())
		})
	}, [detailsRows, searchKeyword, visibleLevels])


	return (<>
		<div style={{ paddingBottom: '8px' }}>
			<Affix>
				<LogLevels allLevels={detailsRowsLevels} visibleLevels={visibleLevels} toggleLevel={level => setVisibleLevels(toggleLevel(level, visibleLevels))} />
			</Affix>
		</div>
		<table className='details'>
			<tbody>
				{filteredDetailsRows.map(detailsRow => <DetailsRowUI key={detailsRow.ID} detailsRow={detailsRow} />)}
			</tbody>
		</table>
	</>)
}