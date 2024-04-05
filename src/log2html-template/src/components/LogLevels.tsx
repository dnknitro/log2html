import { Flex, Switch } from "antd"

import { LogLevel } from "../types"
import { levelToColor } from "../utils"

export const LogLevels = ({ allLevels, visibleLevels, toggleLevel, children }: React.PropsWithChildren<{ allLevels: ([LogLevel, number])[], visibleLevels: LogLevel[], toggleLevel: (level: LogLevel) => void }>) => {
	return (
		<Flex gap="small" align='baseline' justify='left'>
			{Array.from(allLevels).map(([level, amount]) => {
				const isVisible = visibleLevels.includes(level)
				const label = `${amount} ${level}`
				return (
					<Switch key={level} checkedChildren={label} unCheckedChildren={label} value={isVisible} onClick={() => toggleLevel(level)}
						style={{ backgroundColor: isVisible ? levelToColor.get(level) : 'lightgray' }} />
				)
			})}
			{children}
		</Flex>
	)
}