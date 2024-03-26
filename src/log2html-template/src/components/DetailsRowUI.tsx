// import { useEffect } from "react"
import { DetailsRow } from "../types"
import { timeOnlyFormatter } from "../utils"

export const DetailsRowUI = ({ detailsRow }: { detailsRow: DetailsRow }) => {
	// // eslint-disable-next-line @typescript-eslint/no-explicit-any
	// useEffect(() => { (window as any).highlighSyntax() })

	return <>
		<tr className={`detailsRow ${detailsRow.Level.toLowerCase()}`}>
			<td className='timeCol' title={`[${detailsRow.ThreadName}]`}>{timeOnlyFormatter.format(detailsRow.TimeStampUtc)}</td>
			<td className='statusCol'><span className={`statusMark ${detailsRow.Level.toLowerCase()}StatusMark`}>{detailsRow.Level}</span></td>
			<td className='messageCol'>
				{/* {anchorID > 0 && <a name={`issue${anchorID}`}></a>} */}
				{detailsRow.ScreenshotPath != null && (
					<span><a className='screenshot' target='_blank' href={detailsRow.ScreenshotPath}><span className='screenshotIcon' title='Open Screenshot'></span></a></span>
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