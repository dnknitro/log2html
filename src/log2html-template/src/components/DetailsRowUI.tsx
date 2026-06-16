import { PictureFilled } from "@ant-design/icons"
// import { Typography } from "react"
import { Typography } from "antd"
import { forwardRef } from "react"

import { DetailsRow } from "../types"
import { timeOnlyFormatter } from "../utils"
import { LogLevelTag } from "./LogLevelTag"

export const DetailsRowUI = forwardRef(({ detailsRow, isVisible }: { detailsRow: DetailsRow, isVisible: boolean }, ref: React.Ref<HTMLTableRowElement>) => {
	const animationClassName = isVisible ? 'detailsRowVisible' : 'detailsRowHidden'

	return <>
		<tr ref={ref} className={`detailsRow detailsRowAnimated ${animationClassName} ${detailsRow.Level.toLowerCase()}`}>
			<td className='timeCol' title={`[${detailsRow.ThreadName}]`}>{timeOnlyFormatter.format(detailsRow.TimeStampUtc)}</td>
			<td className='statusCol'><LogLevelTag level={detailsRow.Level} /></td>
			<td className='messageCol'>
				{detailsRow.ScreenshotPath != null && (
					<Typography.Link target='_blank' href={detailsRow.ScreenshotPath} title='Open Screenshot' className="screenshot"><PictureFilled /></Typography.Link>
				)}
				<span dangerouslySetInnerHTML={{ __html: detailsRow.Message }} />
			</td>
		</tr>
		{detailsRow.Exception != null &&
			<tr className={`detailsExceptionRow detailsRowAnimated ${animationClassName}`}>
				<td className='exceptionCol' colSpan={3}>
					<div>{detailsRow.Exception}</div>
				</td>
			</tr>
		}
	</>
})
