using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace dnk.log2html;

public class DebugLogger
{
    private static readonly object _lock = new();

    public static void LogToFile(string message)
    {
        StackTrace st = new(1);
        var stackFrame = st.GetFrames()?.FirstOrDefault(x => x.GetMethod().Name is not nameof(LogToFile) and not "Current");
        lock (_lock)
        {
            File.AppendAllText(@"c:\projects\log2html\Results\log.log", $"[{Thread.CurrentThread.Name}] {message} from {stackFrame?.GetMethod().Name}:{stackFrame?.GetFileLineNumber()}{Environment.NewLine}");
        }
    }
}