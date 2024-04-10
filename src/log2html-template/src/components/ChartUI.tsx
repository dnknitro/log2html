import { useContext } from "react"
import { Chart } from "react-google-charts"

import { DataContext } from "../DataContext"
import { LogLevel } from "../types"
import { levelToColor } from "../utils"

export const ChartUI = () => {
	const { allLevelsAndCounts } = useContext(DataContext)
	console.log('Render ChartUI')
	const chartData = [
		['Level' as LogLevel, 'Amount'],
		...allLevelsAndCounts.map(x => [`${x[0]} ${x[1]}`, x[1]])
	] as ([LogLevel, string | number])[]
	const chartColors = allLevelsAndCounts.map(([level]) => levelToColor.get(level)) as string[]

	return (
		<div style={{ width: 300, height: 200, overflow: 'hidden' }}>
			<div style={{ position: 'relative', top: -15, left: -50}}>
				<Chart
					chartType="PieChart"
					data={chartData}
					options={{
						colors: chartColors,
						width: 400,
						height: 250
					}}
				/>
			</div>
		</div>
	)
}