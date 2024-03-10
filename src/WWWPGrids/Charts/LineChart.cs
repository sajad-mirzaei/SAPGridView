using Newtonsoft.Json;

namespace WWWPGrids.Charts;

public class LineChart : Chart
{
    [JsonProperty("title")] public ChartTitle Title { get; set; } = new();
    [JsonProperty("subTitle")] public ChartSubTitle SubTitle { get; set; } = new();
    [JsonProperty("xAxis")] public LineChartXAxis XAxis { get; set; } = new();
    [JsonProperty("yAxis")] public LineChartYAxis YAxis { get; set; } = new();
    [JsonProperty("tooltip")] public ChartTooltip Tooltip { get; set; } = new();
    [JsonProperty("plotOptions")] public LineChartPlotOptions PlotOptions { get; set; } = new();
    [JsonProperty("series")] public List<string> Series { get; set; } = new();
    public LineChart()
    {
        ChartName = GetType().Name;
    }
}
public class LineChartXAxis
{
    [JsonProperty("categories")] public string Categories { get; set; } = String.Empty;
}
public class LineChartYAxis
{
    [JsonProperty("title")] public ChartTitle Title { get; set; } = new();
}
public class LineChartPlotOptions
{
    [JsonProperty("line")] public PlotOptionsLine Line { get; set; } = new();
}
public class PlotOptionsLine
{
    [JsonProperty("dataLabels")] public PlotOptionsLineDataLabels DataLabels { get; set; } = new();
    [JsonProperty("enableMouseTracking")] public bool EnableMouseTracking { get; set; } = true;
}
public class PlotOptionsLineDataLabels
{
    [JsonProperty("enabled")] public bool Enabled { get; set; } = true;
}
//public class LineChartSeries<T>
//{
//    [JsonProperty("name")] public string Name { get; set; } = String.Empty;
//    [JsonProperty("data")] public List<T> Data { get; set; } = new();
//}