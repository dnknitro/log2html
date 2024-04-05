import { ExclamationCircleOutlined, VerticalAlignTopOutlined } from "@ant-design/icons"
import { Button, FloatButton, Space, Typography } from "antd"
import { useMemo, useRef, useState } from "react"

import { SummaryRow } from "../types"
import { toggleLevel } from "../utils"
import { DetailsRowUI } from "./DetailsRowUI"
import { LogLevels } from "./LogLevels"

export const DetailsTable = ({ summaryRow, showTitle }: { summaryRow: SummaryRow, showTitle?: boolean }) => {
	const { detailsRows, detailsRowsLevels } = summaryRow

	const [visibleLevels, setVisibleLevels] = useState(detailsRowsLevels.map(([level]) => level).filter(level => detailsRowsLevels.length < 2 || level !== 'DEBUG'))

	// const { searchKeyword } = useContext(SearchContext)
	// const filteredDetailsRows = useMemo(() => {
	// 	return detailsRows.filter(detailsRow => {
	// 		if (!visibleLevels.includes(detailsRow.Level)) return false
	// 		if (searchKeyword.length < 2) return true
	// 		return detailsRow.Message.toLowerCase().includes(searchKeyword.toLowerCase())
	// 	})
	// }, [detailsRows, searchKeyword, visibleLevels])
	const filteredDetailsRows = detailsRows.filter(detailsRow => visibleLevels.includes(detailsRow.Level))

	const topRef = useRef<HTMLDivElement>(null)
	const failRef = useRef<HTMLTableRowElement>(null)
	const failDetailsRow = useMemo(() => {
		return filteredDetailsRows.find(x => x.Level === 'FAIL')
	}, [filteredDetailsRows])

	return (<>
		<FloatButton icon={<VerticalAlignTopOutlined />} type="default" tooltip='Jump to top' style={{ top: 8 }}
			onClick={() => topRef.current?.scrollIntoView({ behavior: 'smooth', block: 'center' })} />
		<Space direction="vertical" style={{ width: '100%', overflow: 'auto' }}>
			{showTitle && <Typography.Title level={4} style={{ margin: 0 }}>{summaryRow.testCaseName}</Typography.Title>}
			{/* <Affix> */}
			<div ref={topRef}>
				<LogLevels allLevels={detailsRowsLevels} visibleLevels={visibleLevels} toggleLevel={level => setVisibleLevels(toggleLevel(level, visibleLevels))}>
					{!!failDetailsRow && <Button type="link" icon={<ExclamationCircleOutlined />}
						onClick={() => failRef.current?.scrollIntoView({ behavior: 'smooth', block: 'center' })}>
						Jump to Fail
					</Button>}
				</LogLevels>
			</div>
			{/* </Affix> */}
			<table className='details'>
				<tbody>
					{filteredDetailsRows.map(detailsRow => <DetailsRowUI ref={detailsRow === failDetailsRow ? failRef : null} key={detailsRow.ID} detailsRow={detailsRow} />)}
				</tbody>
			</table>
		</Space>
	</>)
}