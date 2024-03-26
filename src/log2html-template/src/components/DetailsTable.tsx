import { SummaryRowState, useLevels } from "../DataStateContext"
import { DetailsRowUI } from "./DetailsRowUI"

export const DetailsTable = ({ summaryRowState }: { summaryRowState: SummaryRowState }) => {
	const { visibleLevels } = useLevels()
	const { summaryRow: { detailsRows } } = summaryRowState
	return (
		<table className='details'>
			<tbody>
				{detailsRows
					.filter(detailsRow => visibleLevels.includes(detailsRow.Level))
					.map(detailsRow => <DetailsRowUI key={detailsRow.ID} detailsRow={detailsRow} />)}
			</tbody>
		</table>
	)
}