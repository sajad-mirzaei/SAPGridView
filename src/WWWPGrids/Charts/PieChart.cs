using Newtonsoft.Json;

/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids.Charts;

public class PieChart : Chart
{
    [JsonProperty("key")] public string Key { get; set; }
    [JsonProperty("value")] public string Value { get; set; }
    [JsonProperty("title")] public ChartTitle Title { get; set; }
    [JsonProperty("subTitle")] public ChartSubTitle SubTitle { get; set; }
    public PieChart()
    {
        ChartName = GetType().Name;
    }
}