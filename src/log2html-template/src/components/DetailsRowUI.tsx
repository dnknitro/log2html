import { PictureFilled } from "@ant-design/icons"
// import { Typography } from "react"
import { Typography } from "antd"
import { DetailsRow } from "../types"
import { timeOnlyFormatter } from "../utils"
import { LogLevelTag } from "./LogLevelTag"

export const DetailsRowUI = ({ detailsRow }: { detailsRow: DetailsRow }) => {
	// // eslint-disable-next-line @typescript-eslint/no-explicit-any
	// useEffect(() => { (window as any).highlighSyntax() })

	return <>
		<tr className={`detailsRow ${detailsRow.Level.toLowerCase()}`}>
			<td className='timeCol' title={`[${detailsRow.ThreadName}]`}>{timeOnlyFormatter.format(detailsRow.TimeStampUtc)}</td>
			<td className='statusCol'><LogLevelTag level={detailsRow.Level} /></td>
			<td className='messageCol'>
				{/* {anchorID > 0 && <a name={`issue${anchorID}`}></a>} */}
				{detailsRow.ScreenshotPath != null && (
					<Typography.Link target='_blank' href={detailsRow.ScreenshotPath} title='Open Screenshot' className="screenshot"><PictureFilled /></Typography.Link>
				)}
				<span dangerouslySetInnerHTML={{ __html: detailsRow.Message }} />
			</td>
		</tr>
		{detailsRow.Exception != null &&
			<tr className='detailsExceptionRow'>
				<td className='exceptionCol' colSpan={3}>
					<div>{detailsRow.Exception}</div>
				</td>
			</tr>
		}
	</>
}