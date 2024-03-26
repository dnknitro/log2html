import { useContext } from "react"
import { DataContext } from "../DataContext"

export const LevelsBrowsers = () => {
  const { summaryRows, levelsAndBrowsers } = useContext(DataContext)

  return (
    <h4 id="passFailInfo">
      <div style={{ float: 'left', marginRight: '20px' }}>
        <label>Tests Run:</label> <span>{summaryRows.length}</span>
      </div>
      {Array.from(levelsAndBrowsers.entries()).map(([key, value]) => {
        const level = value.Level.toLowerCase()
        const count = value.Count
        return (
          <div key={key} style={{ float: 'left', marginRight: '20px' }}>
            <label className={`status${level}`}>{key}:</label> <span className={`status${level}`}>{count}</span>
          </div>
        )
      })}
      <div className="clearBoth"></div>
    </h4>
  )
}