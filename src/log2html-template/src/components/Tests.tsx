import { Modal, Typography } from "antd"
import { useState } from "react"

import { SummaryRow } from "../types"
import { DetailsTable } from "./DetailsTable"
import { LogLevelTag } from "./LogLevelTag"
import { TestsList } from "./TestsList"

export const Tests = () => {
	const [activeSummaryRow, setActiveSummaryRow] = useState<SummaryRow>()
	const isModalOpen = !!activeSummaryRow
	const closeModal = () => setActiveSummaryRow(undefined)

	return <div>
		<TestsList onTestClick={summaryRow => {
			setActiveSummaryRow(summaryRow)
		}} />

		<Modal
			title={<Typography.Title level={3} style={{ marginTop: 0 }}><LogLevelTag level={activeSummaryRow?.level ?? "DEBUG"} /><span dangerouslySetInnerHTML={{ __html: activeSummaryRow?.testCaseName ?? '' }} /></Typography.Title>}
			destroyOnClose
			width='96%'
			footer={null}
			open={isModalOpen} onOk={closeModal} onCancel={closeModal}>
			<DetailsTable summaryRow={activeSummaryRow!} showTitle={false} />
		</Modal>
	</div>
}