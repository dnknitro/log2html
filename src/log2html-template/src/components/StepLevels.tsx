import { useLevels } from "../DataStateContext"

export const StepLevels = () => {
	const { uniqueLevels, visibleLevels, toggleLevel } = useLevels()
	//TODO: const levels = this.props.store.getState().stepNames;
	return (
		<span>
			[<span id="stepNames">
				{uniqueLevels.map(level => {
					let crossedOutClass = ""
					if (!visibleLevels.includes(level)) crossedOutClass = "crossedOut"
					return (
						<span key={level} className={`link ${crossedOutClass} status${level.toLowerCase()}`} onClick={() => toggleLevel(level)}>{level}</span>
					)
				})}
			</span>]
		</span>
	)
}