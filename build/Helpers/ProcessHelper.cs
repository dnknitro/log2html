using System.Linq;
using Nuke.Core.Tooling;

namespace dnkLog4netHtmlReport.build.Helpers
{
	class ProcessHelper
	{
		public static string StartProcess(string processPath, string arguments, string workingDirectory = null)
		{
			var process = ProcessTasks.StartProcess(processPath, arguments, workingDirectory, null, null, true);
			process?.WaitForExit();
			var result = string.Join(System.Environment.NewLine, process?.Output.Select(x => x.Text));
			if (process?.ExitCode == 1)
				throw new System.Exception(result);
			return result;
		}
	}
}