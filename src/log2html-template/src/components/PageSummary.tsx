import { useContext } from "react"
import { DataContext } from "../DataContext"
import { calcDuration, timeOnlyFormatter } from "../utils"
import { LevelsBrowsers } from "./LevelsBrowsers"
import { ExpandCollapseAll } from "./ExpandCollapseAll"
import { StepLevels } from "./StepLevels"

export const PageSummary = () => {
	const { reportMetaData, reportStartTime, reportEndTime } = useContext(DataContext)
	return (
		<>
			<h1 id="reportTitle">{reportMetaData.ReportName}</h1>
			<h4 id="environmentInfo"><label>Environment:</label> <span id="environment">{reportMetaData.ReportEnvironment}</span></h4>
			<h4 id="timeInfo">
				<label>Start:</label> <span id="startTime">{timeOnlyFormatter.format(reportStartTime)}</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<label>End:</label> <span id="endTime">{timeOnlyFormatter.format(reportEndTime)}</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<label>Duration:</label> <span id="duration">{calcDuration(reportStartTime, reportEndTime)}</span>
			</h4>

			<LevelsBrowsers />
			<div>
				<ExpandCollapseAll />&nbsp;&nbsp;
				<StepLevels />
			</div>
		</>
	)
}