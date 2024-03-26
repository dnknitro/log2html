import { useContext } from "react"
import { DataStateContext } from "../DataStateContext"
import { SummaryRowUI } from "./SummaryRowUI"

export const SummaryTable = () => {
	const { dataState } = useContext(DataStateContext)
	// const nameToIsExpanded = new Map<string, boolean>(summaryRowsState.state.map(x => [x.summaryRow.testCaseName, x.isExpanded]))

	return (
		<div className="div-table summary">
			{dataState.summaryRows.map(summaryRowState =>
				<SummaryRowUI key={summaryRowState.summaryRow.testCaseName} summaryRowState={summaryRowState} />)}
		</div>
	)
}