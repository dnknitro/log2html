import { useToggleAllRowsIsExpanded } from "../DataStateContext"

export const ExpandCollapseAll = () => {
	const { areAllExpanded, toggleAllRowsIsExpanded } = useToggleAllRowsIsExpanded()
	const labelState = areAllExpanded ? 'Collapse' : 'Expand'
	return (
		<span>
			[ <span id="expandCollapseAll" className='link' onClick={toggleAllRowsIsExpanded}>
				{labelState} All</span> ]
		</span>
	)
}