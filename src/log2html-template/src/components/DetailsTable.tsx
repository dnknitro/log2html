import { ExclamationCircleOutlined, VerticalAlignTopOutlined } from "@ant-design/icons"
import { Button, FloatButton, Space, Typography } from "antd"
import { useEffect, useMemo, useRef, useState } from "react"

import { LogLevel, SummaryRow } from "../types"
import { toggleLevel } from "../utils"
import { DetailsRowUI } from "./DetailsRowUI"
import { LogLevels } from "./LogLevels"

const detailsRowAnimationMs = 300

export const DetailsTable = ({ summaryRow, showTitle }: { summaryRow: SummaryRow, showTitle?: boolean }) => {
	const { detailsRows, detailsRowsLevels } = summaryRow

	const initialVisibleLevels = useMemo(() => {
		return detailsRowsLevels.map(([level]) => level).filter(level => detailsRowsLevels.length < 2 || level !== 'DEBUG')
	}, [detailsRowsLevels])
	const [visibleLevels, setVisibleLevels] = useState(initialVisibleLevels)
	const [renderedLevels, setRenderedLevels] = useState(initialVisibleLevels)
	const hideTimeoutsRef = useRef(new Map<LogLevel, ReturnType<typeof setTimeout>>())

	useEffect(() => {
		return () => {
			hideTimeoutsRef.current.forEach(timeout => clearTimeout(timeout))
			hideTimeoutsRef.current.clear()
		}
	}, [])

	// const { searchKeyword } = useContext(SearchContext)
	// const filteredDetailsRows = useMemo(() => {
	// 	return detailsRows.filter(detailsRow => {
	// 		if (!visibleLevels.includes(detailsRow.Level)) return false
	// 		if (searchKeyword.length < 2) return true
	// 		return detailsRow.Message.toLowerCase().includes(searchKeyword.toLowerCase())
	// 	})
	// }, [detailsRows, searchKeyword, visibleLevels])
	const filteredDetailsRows = detailsRows.filter(detailsRow => visibleLevels.includes(detailsRow.Level))
	const renderedDetailsRows = detailsRows.filter(detailsRow => renderedLevels.includes(detailsRow.Level))

	const handleToggleLevel = (level: LogLevel) => {
		const nextVisibleLevels = toggleLevel(level, visibleLevels)
		const isShowing = nextVisibleLevels.includes(level)

		setVisibleLevels(nextVisibleLevels)
		clearLevelTimeout(level)

		if (isShowing) {
			setRenderedLevels(levels => levels.includes(level) ? levels : [...levels, level])
			return
		}

		const timeout = setTimeout(() => {
			setRenderedLevels(levels => levels.filter(renderedLevel => renderedLevel !== level))
			hideTimeoutsRef.current.delete(level)
		}, detailsRowAnimationMs)
		hideTimeoutsRef.current.set(level, timeout)
	}

	const clearLevelTimeout = (level: LogLevel) => {
		const timeout = hideTimeoutsRef.current.get(level)
		if (!timeout) return

		clearTimeout(timeout)
		hideTimeoutsRef.current.delete(level)
	}

	const topRef = useRef<HTMLDivElement>(null)
	const failRef = useRef<HTMLTableRowElement>(null)
	const failDetailsRow = useMemo(() => {
		return filteredDetailsRows.find(x => x.Level === 'FAIL')
	}, [filteredDetailsRows])

	return (<>
		<FloatButton icon={<VerticalAlignTopOutlined />} type="default" tooltip='Jump to top'
			onClick={() => topRef.current?.scrollIntoView({ behavior: 'smooth', block: 'center' })} />
		<Space orientation="vertical" style={{ width: '100%', overflow: 'auto' }}>
			{showTitle && <Typography.Title level={4} style={{ margin: 0 }}>{summaryRow.testCaseName}</Typography.Title>}
			{/* <Affix> */}
			<div ref={topRef}>
				<LogLevels allLevels={detailsRowsLevels} visibleLevels={visibleLevels} toggleLevel={handleToggleLevel}>
					{!!failDetailsRow && <Button type="link" icon={<ExclamationCircleOutlined />}
						onClick={() => failRef.current?.scrollIntoView({ behavior: 'smooth', block: 'center' })}>
						Jump to Fail
					</Button>}
				</LogLevels>
			</div>
			{/* </Affix> */}
			<table className='details'>
				<tbody>
					{renderedDetailsRows.map(detailsRow => <DetailsRowUI
						ref={detailsRow === failDetailsRow ? failRef : null}
						key={detailsRow.ID}
						detailsRow={detailsRow}
						isVisible={visibleLevels.includes(detailsRow.Level)}
					/>)}
				</tbody>
			</table>
		</Space>
	</>)
}
