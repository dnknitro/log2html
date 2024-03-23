using dnkUtils;
using Newtonsoft.Json;

namespace dnk.log2html;

public class ReportTemplate
{
    public ReportTemplate(ReportMetaData reportMetaData) => _reportMetaData = reportMetaData;

    private readonly ReportMetaData _reportMetaData;
    public string SummaryRowBgColor { get; set; } = "#fbfbfb";

    public string GetTemplate()
    {
        var reportTemplate = ResourceUtils.ReadStringFromEmbeddedResource("dnk.log2html.ReportTemplate.html", typeof(ReportFile).Assembly);

        var reportMetaDataJson = JsonConvert.SerializeObject(_reportMetaData);
        return reportTemplate
            .Replace("var reportMetaData = {};", $"var reportMetaData = {reportMetaDataJson};")
            .Replace("#fbfbfb", SummaryRowBgColor);
    }
}