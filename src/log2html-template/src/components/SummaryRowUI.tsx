import { SummaryRowState, useToggleRowIsExpanded } from "../DataStateContext"
import { timeOnlyFormatter } from "../utils"
import { DetailsTable } from "./DetailsTable"

export const SummaryRowUI = ({ summaryRowState }: { summaryRowState: SummaryRowState }) => {
	const { summaryRow, isExpanded } = summaryRowState
	const { level } = summaryRow

	const toggleRowIsExpanded = useToggleRowIsExpanded(summaryRowState)

	const relativeDurationHack = []
	for (let i = 0; i < summaryRow.relativeDuration; i++) relativeDurationHack.push(i)

	return (
		<>
			<div className={`div-table-row summaryRow ${isExpanded ? 'summaryRowExpanded' : 'summaryRowCollapsed'} ${level.toLowerCase()}`}>
				<div className='div-table-col summaryStatus'><span className={`statusMark ${level.toLowerCase()}StatusMark`}>{level}</span></div>
				<div className='div-table-col summaryTestName link' onClick={toggleRowIsExpanded}>
					{/*<a href="#issue21"><b>&dArr;</b></a>&nbsp;*/}
					<span dangerouslySetInnerHTML={{ __html: summaryRow.testCaseNameShort }} />
				</div>
				<div className='div-table-col summaryStartEnd' title={`Duration: ${summaryRow.durationStr} ${timeOnlyFormatter.format(summaryRow.startTime)} - ${timeOnlyFormatter.format(summaryRow.endTime)}`} onClick={toggleRowIsExpanded}>
					{summaryRow.durationStr}
					<div className='summaryDuration'>
						{relativeDurationHack.map(x => (<span key={x} className='durationMark'></span>))}
					</div>
				</div>
			</div>
			<div className={`div-table-row summaryDetailsRow ${isExpanded ? 'div-table-row-visible' : 'div-table-row-hidden'}`}>
				<div className="div-table-col">
					<DetailsTable summaryRowState={summaryRowState} />
				</div>
			</div>
		</>
	)
}