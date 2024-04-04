import { Tag } from "antd"

import { LogLevel } from "../types"
import { levelToAntColor } from "../utils"

export const LogLevelTag = ({ level }: { level: LogLevel }) => {
	return <span><Tag style={{ width: '60px', textAlign: 'center', verticalAlign: 'middle', fontWeight: 'bold' }}
		color={levelToAntColor(level)}>{level}</Tag></span>
}