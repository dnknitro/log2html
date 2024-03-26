import { useState } from 'react'
import './App.css'
import { DataContext, defaultDataContextData } from './DataContext'
import { SummaryRowState, DataStateContext, DataState } from './DataStateContext'
import { ChartUI } from './components/ChartUI'
import { PageSummary } from './components/PageSummary'
import { SummaryTable } from './components/SummaryTable'

const initialDataState: DataState = {
	summaryRows: defaultDataContextData.summaryRows.map((summaryRow): SummaryRowState => ({
		isExpanded: false,
		summaryRow,
	})),
	visibleLevels: defaultDataContextData.uniqueLevels.filter(x => x !== 'DEBUG'),
}

function App() {
	const [dataState, setDataState] = useState(initialDataState)

	return <>
		<DataContext.Provider value={defaultDataContextData}>
			<DataStateContext.Provider value={{ dataState, setDataState }}>
				<div className="div-table" style={{ width: '1000px' }}>
					<div className="div-table-row">
						<div className="div-table-col">
							<PageSummary />
						</div>
						<div id="headerRight" className="div-table-col">
							<div className="chart-container">
								<ChartUI />
							</div>
						</div>
					</div>
					<div className="div-table-row">
						<div className="div-table-col">
							<SummaryTable />
						</div>
					</div>
				</div>
			</DataStateContext.Provider>
		</DataContext.Provider>
	</>
}

export default App
