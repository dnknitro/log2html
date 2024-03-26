import { createContext, useContext } from "react"
import { LogLevel, SummaryRow } from "./types"
import { DataContext } from "./DataContext"

export type DataState = {
	summaryRows: SummaryRowState[]
	visibleLevels: LogLevel[]
}

export type SummaryRowState = {
	isExpanded: boolean
	// levels: string[]
	summaryRow: SummaryRow
}

export const DataStateContext = createContext<{ dataState: DataState, setDataState: (newState: DataState) => void }>({
	dataState: {
		summaryRows: [],
		visibleLevels: [],
	},
	setDataState: () => { },
})

export const useToggleRowIsExpanded = (summaryRowState: SummaryRowState) => {
	const { dataState, setDataState } = useContext(DataStateContext)

	return () => {
		const newSummaryRows = [...dataState.summaryRows]

		const index = newSummaryRows.findIndex(x => x.summaryRow.testCaseName === summaryRowState.summaryRow.testCaseName)
		const currentRowState = newSummaryRows[index]
		newSummaryRows[index] = { ...currentRowState, isExpanded: !currentRowState.isExpanded }

		setDataState({ ...dataState, summaryRows: newSummaryRows })
	}
}

export const useToggleAllRowsIsExpanded = () => {
	const { dataState, setDataState } = useContext(DataStateContext)
	const areAllExpanded = dataState.summaryRows.every(x => x.isExpanded)

	return {
		areAllExpanded,
		toggleAllRowsIsExpanded: () => {
			const newSummaryRows = dataState.summaryRows.map(x => ({ ...x, isExpanded: !areAllExpanded }))
			setDataState({ ...dataState, summaryRows: newSummaryRows })
		}
	}
}

export const useLevels = () => {
	const { uniqueLevels } = useContext(DataContext)
	const { dataState, setDataState } = useContext(DataStateContext)
	const { visibleLevels } = dataState
	return {
		uniqueLevels,
		visibleLevels,
		toggleLevel: (level: LogLevel) => {
			const newDataState = { ...dataState, visibleLevels: [...dataState.visibleLevels] }

			const levelIndex = newDataState.visibleLevels.indexOf(level)

			if (levelIndex >= 0) {
				newDataState.visibleLevels.splice(levelIndex, 1)
			} else {
				newDataState.visibleLevels.push(level)
			}
			setDataState(newDataState)
		},
	}
}