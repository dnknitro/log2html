import { Flex, Input, Space } from 'antd'
import { useState } from 'react'
import './App.css'
import { DataContext, defaultDataContextData } from './DataContext'
import { ChartUI } from './components/ChartUI'
import { SearchContext } from './SearchContext'
import { TestsHeader } from './components/TestsHeader'
import { TestsTabs } from './components/TestsTabs'

function App() {
	const [searchKeyword, setSearchKeyword] = useState("")

	return <>
		<DataContext.Provider value={defaultDataContextData}>
			<Flex gap="middle" align='center'>
				<div><TestsHeader /></div>
				<div><ChartUI /></div>
			</Flex>

			<Space direction="vertical" size="small">
				<Input placeholder="type to search" style={{ width: 300 }} autoFocus allowClear value={searchKeyword} onChange={e => setSearchKeyword(e.target.value)} />

				<SearchContext.Provider value={{ searchKeyword, setSearchKeyword }}>
					<TestsTabs />
				</SearchContext.Provider>
			</Space>
		</DataContext.Provider>
	</>
}

export default App
