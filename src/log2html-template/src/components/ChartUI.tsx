import { useContext } from "react"
import { Chart } from "react-google-charts"
import { DataContext } from "../DataContext"
import { LogLevel } from "../types"
import { levelToColor } from "../utils"

export const ChartUI = () => {
	const { summaryRowsGroups } = useContext(DataContext)

	const logLevelToCount = Object.entries(summaryRowsGroups).map(([key, group]) => ([key as LogLevel, group.length])) as ([LogLevel, string | number])[]

	const chartData = [
		['Level' as LogLevel, 'Amount'],
		...logLevelToCount
	] as ([LogLevel, string | number])[]
	const chartColors = logLevelToCount.map(([level]) => levelToColor.get(level)) as string[]

	return (
		<Chart
			chartType="PieChart"
			data={chartData}
			options={{
				colors: chartColors,
			}}
			width={"100%"}
		/>
	)
}