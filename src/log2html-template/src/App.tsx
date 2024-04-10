import { useState } from 'react'

import { Tests } from './components/Tests'
import { TestsHeader } from './components/TestsHeader'
import { DataContext, defaultDataContextData } from './DataContext'
import { SearchContext } from './SearchContext'

import './App.css'

function App() {
	const [searchKeyword, setSearchKeyword] = useState("")

	return (
		<DataContext.Provider value={defaultDataContextData}>
			<SearchContext.Provider value={{ searchKeyword, setSearchKeyword }}>
				<table>
					<tr><td><TestsHeader /></td></tr>
					<tr><td><Tests /></td></tr>
				</table>
			</SearchContext.Provider>
		</DataContext.Provider>
	)
}

export default App
