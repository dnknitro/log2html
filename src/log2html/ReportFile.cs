using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace dnk.log2html;

public class ReportFile : IDisposable
{
    private static readonly Encoding encoding = Encoding.UTF8;
    private const string reportDataPlaceholder = "{ \"EndOfReportData\": true }";
    private const string reportSummaryPlaceholder = "{Server Side LevelsBrowsers}";

    public static string DefaultReportFileNameOnly = $"Report_{DateTime.Now:yyyy-MM-dd_HH.mm.ss.fff}";

    public static string DefaultReportFolder = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory.Trim('\\')).Parent.Parent.Parent.FullName, "Results");

    public ReportFile(ReportTemplate reportTemplate, string reportFileNameOnly = null, string reportFolder = null)
    {
        ReportFileNameOnly = reportFileNameOnly ?? DefaultReportFileNameOnly;
        ReportFolder = reportFolder ?? DefaultReportFolder;
        Directory.CreateDirectory(ReportFolder);

        ReportFilePath = Path.GetFullPath(Path.Combine(ReportFolder, ReportFileNameOnly + ".html"));

        var template = reportTemplate.GetTemplate();
        (string beforeData, string afterData) templateParts = template.Split(new string[] { reportDataPlaceholder }, StringSplitOptions.None) switch
        {
            var chunks when chunks.Length == 2 => (chunks[0], chunks[1]),
            _ => throw new InvalidOperationException("Could not split template into two parts.")
        };
        afterData = templateParts.afterData;

        lock (_fileWriteLock)
        {
            _file = File.Open(ReportFilePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);

            var buffer = encoding.GetBytes(templateParts.beforeData);
            _file.Write(buffer, 0, buffer.Length);
            _file.Flush();
        }
    }

    private FileStream _file;

    public string ReportFileNameOnly { get; }
    public string ReportFolder { get; }
    public string ReportFilePath { get; }

    private static readonly object _fileWriteLock = new();
    private readonly string afterData;

    public void Append(ReportEntry reportEntry)
    {
        lock (_fileWriteLock)
        {
            var json = JsonConvert.SerializeObject(reportEntry /*, Formatting.Indented*/) + $",{Environment.NewLine}			";
            var buffer = encoding.GetBytes(json);
            _file.Write(buffer, 0, buffer.Length);
            _file.Flush();
        }
    }

    public void AddSummaryAndFinish(string summary)
    {
        lock (_fileWriteLock)
        {
            if (_file == null)
            {
                return;
            }

            var afterDataChunks = afterData.Split(new string[] { reportSummaryPlaceholder }, StringSplitOptions.None);
            if (afterDataChunks.Length != 2)
            {
                throw new InvalidOperationException("Could not split afterData chunk into two parts.");
            }

            var buffer = encoding.GetBytes(afterDataChunks[0] + summary + afterDataChunks[1]);
            _file.Write(buffer, 0, buffer.Length);
            _file.Flush(true);
            _file.Dispose();
            _file = null;
        }
    }

    private bool disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: free managed resources
            }

            try
            {
                AddSummaryAndFinish("Called from Dispose");
                _file?.Flush(true);
                _file?.Dispose();
                _file = null;
            }
            catch
            {
                // nothing more can be done
            }

            disposedValue = true;
        }
    }

    ~ReportFile()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}