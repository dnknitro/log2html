import { LogLevel } from "./types"

export const timeOnlyFormatter = new Intl.DateTimeFormat('en-US', { hour: 'numeric', minute: 'numeric', second: 'numeric', hour12: false })

// TODO: use moment?
export const calcDuration = function (startTime: Date, endTime: Date) {
  let durationStr = ""

  // Set the unit values in milliseconds.
  const msecPerMinute = 1000 * 60
  const msecPerHour = msecPerMinute * 60
  const msecPerDay = msecPerHour * 24
  // Get the difference in milliseconds.
  let interval = endTime.getTime() - startTime.getTime()

  // Calculate how many days the interval contains. Subtract that
  // many days from the interval to determine the remainder.
  const days = Math.floor(interval / msecPerDay)
  if (days > 0) durationStr = days + "d "
  interval = interval - (days * msecPerDay)

  // Calculate the hours, minutes, and seconds.
  const hours = Math.floor(interval / msecPerHour)
  if (hours > 0) durationStr = durationStr + hours + "h "
  interval = interval - (hours * msecPerHour)

  const minutes = Math.floor(interval / msecPerMinute)
  if (minutes > 0) durationStr = durationStr + minutes + "m "
  interval = interval - (minutes * msecPerMinute)

  const seconds = Math.floor(interval / 1000)
  durationStr = durationStr + seconds + "s "

  return durationStr
}

export const levelToColor = new Map<LogLevel, string>([
	['DEBUG', 'gray'],
	['PASS', '#0AC775'],
	['INFO', '#45CAE6'],
	['FAILSKIP', 'gold'],
	['WARN', '#FFAB40'],
	['RETRY', '#A6ED5F'],
	['FAIL', '#F24965'],
	['ERROR', '#F24965'],
	['FATAL', '#F24965'],
])

const styleSheet = document.styleSheets[0]
levelToColor.forEach((color, logLevel) => {
	const level = logLevel.toLowerCase()

	styleSheet.insertRule(`.${level} .durationMark { background-color: ${color} !important; border-color: ${color} !important; }`, 1)
	styleSheet.insertRule(`.${level}StatusMark { background-color: ${color} !important; text-align: center; }`, 1)
	styleSheet.insertRule(`.status${level} { color: ${color} !important; }`, 1)

	styleSheet.insertRule(`.${logLevel} .durationMark { background-color: ${color} !important; border-color: ${color} !important; }`, 1)
	styleSheet.insertRule(`.${logLevel}StatusMark { background-color: ${color} !important; text-align: center; }`, 1)
	styleSheet.insertRule(`.status${logLevel} { color: ${color} !important; }`, 1)
})
