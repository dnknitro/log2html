import { createContext } from "react"

export const defaultSearchContext = {
	searchKeyword: "",
	// eslint-disable-next-line @typescript-eslint/no-unused-vars
	setSearchKeyword: (_searchKeyword: string) => { },
}

export const SearchContext = createContext(defaultSearchContext)