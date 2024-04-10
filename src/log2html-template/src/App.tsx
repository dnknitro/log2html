import { Flex, Input, Space } from 'antd'
import { useState } from 'react'

import { ChartUI } from './components/ChartUI'
import { TestsHeader } from './components/TestsHeader'
import { TestsWithPopup } from './components/TestsWithPopup'
import { DataContext, defaultDataContextData } from './DataContext'
import { SearchContext } from './SearchContext'

import './App.css'

function App() {
	const [searchKeyword, setSearchKeyword] = useState("")

	return <>
		<DataContext.Provider value={defaultDataContextData}>
			<div>
				<Flex gap="middle" align='center'>
					<div><TestsHeader /></div>
					<div><ChartUI /></div>
				</Flex>

				<Space direction="vertical" size="small" style={{ width: '100%' }}>
					<Input placeholder="type to search" style={{ width: 300 }} autoFocus allowClear value={searchKeyword} onChange={e => setSearchKeyword(e.target.value)} />

					<SearchContext.Provider value={{ searchKeyword, setSearchKeyword }}>
						<TestsWithPopup />
						{/* <TestsTabs /> */}
					</SearchContext.Provider>
				</Space>
			</div>
		</DataContext.Provider>
	</>
}

export default App
