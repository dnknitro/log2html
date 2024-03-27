import { Button, Tabs } from "antd"
import { ReactNode, useContext, useState } from "react"
import { DataContext } from "../DataContext"
import { DetailsTable } from "./DetailsTable"
import { LogLevelTag } from "./LogLevelTag"
import { TestsList } from "./TestsList"

const defaultTabKey = 'All Tests'

type Tab = {
	key: string,
	label: ReactNode,
	children: ReactNode,
	closable: boolean,
	icon: ReactNode,
}

export const TestsTabs = () => {
	const { allTestsCount } = useContext(DataContext)

	const [tabs, setTabs] = useState(new Array<Tab>())

	const [activeTabKey, setActiveTabKey] = useState(defaultTabKey)

	return <>
		<Tabs
			type="editable-card"
			hideAdd
			activeKey={activeTabKey}
			tabBarExtraContent={<Button type="link" onClick={() => { setTabs([]); setActiveTabKey(defaultTabKey) }}>Close All</Button>}
			items={[{
				label: `All Tests (${allTestsCount})`,
				key: defaultTabKey,
				closable: false,
				children: <TestsList onTestClick={summaryRow => {
					const key = summaryRow.testCaseName
					const tab = tabs.find(x => x.key === key)
					if (!tab) {
						setTabs([
							{
								label: summaryRow.testCaseNameShort,
								key,
								children: <DetailsTable summaryRow={summaryRow} />,
								closable: true,
								icon: <LogLevelTag level={summaryRow.level} />,
							},
							...tabs
						])
					}
					setActiveTabKey(key)
				}} />,
			}, ...tabs]}
			onChange={key => setActiveTabKey(key)}
			onEdit={(targetKey, action) => {
				if (action === 'remove') {
					setTabs(tabs.filter(x => x.key !== targetKey))
					setActiveTabKey(defaultTabKey)
				}
			}}
		/>
	</>
}