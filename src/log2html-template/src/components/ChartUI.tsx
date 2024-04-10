import { useContext } from "react"
import { Chart } from "react-google-charts"

import { DataContext } from "../DataContext"
import { LogLevel } from "../types"
import { levelToColor } from "../utils"

export const ChartUI = () => {
	const { allLevelsAndCounts } = useContext(DataContext)

	const chartData = [
		['Level' as LogLevel, 'Amount'],
		...allLevelsAndCounts.map(x => [`${x[0]} ${x[1]}`, x[1]])
	] as ([LogLevel, string | number])[]
	const chartColors = allLevelsAndCounts.map(([level]) => levelToColor.get(level)) as string[]

	return (
		<div style={{ height: '165px' }}>
			<Chart
				chartType="PieChart"
				data={chartData}
				width='100%'
				options={{
					colors: chartColors,
					width: 500,
					height: 250
				}}
			/>
		</div>
	)
}