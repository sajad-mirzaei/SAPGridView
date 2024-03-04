using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids.Charts;

public class Chart
{
    [JsonProperty("chartName")] protected string ChartName { get; set; }
    [JsonProperty("chartContainerId")] public string ChartContainerId { get; set; }
    public virtual object DeepCopy()
    {
        return (Chart)MemberwiseClone();
    }
}

public class ChartTitle
{
    [JsonProperty("text")] public string Text { get; set; } = string.Empty;
    [JsonProperty("align")] public TitleAlign Align { get; set; } = TitleAlign.Center;
}

public class ChartSubTitle
{
    [JsonProperty("text")] public string Text { get; set; } = string.Empty;
    [JsonProperty("align")] public TitleAlign Align { get; set; } = TitleAlign.Center;
}


public class ChartTooltip
{
    [JsonProperty("valueSuffix")] public string ValueSuffix { get; set; } = string.Empty;
}

public enum TitleAlign
{
    Right = 0,
    Center = 1,
    Left = 2
}