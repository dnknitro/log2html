import { Modal } from "antd"
import { useState } from "react"
import { SummaryRow } from "../types"
import { DetailsTable } from "./DetailsTable"
import { TestsList } from "./TestsList"

export const TestsWithPopup = () => {
	const [activeSummaryRow, setActiveSummaryRow] = useState<SummaryRow>()
	const isModalOpen = !!activeSummaryRow
	const closeModal = () => setActiveSummaryRow(undefined)

	return <>
		<TestsList onTestClick={summaryRow => {
			setActiveSummaryRow(summaryRow)
		}} />

		<Modal
			title={<span dangerouslySetInnerHTML={{ __html: activeSummaryRow?.testCaseName ?? '' }} />}
			destroyOnClose
			width='90%'
			footer={null}
			open={isModalOpen} onOk={closeModal} onCancel={closeModal}>
			<DetailsTable summaryRow={activeSummaryRow!} showTitle={false} />
		</Modal>
	</>
}