import { Tag } from "antd"
import { LogLevel } from "../types"
import { levelToColor } from "../utils"

export const LogLevelTag = ({ level }: { level: LogLevel }) => {
	return <Tag style={{ width: '60px', textAlign: 'center', fontWeight: 'bold' }} color={levelToColor.get(level)}>{level}</Tag>
}