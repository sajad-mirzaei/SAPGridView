using Newtonsoft.Json;

namespace WWWPGrids.Charts;

public class ColumnChart : Chart
{
    [JsonProperty("title")] public ChartTitle Title { get; set; } = new();
    [JsonProperty("subTitle")] public ChartSubTitle SubTitle { get; set; } = new();
    [JsonProperty("xAxis")] public ColumnChartXAxis XAxis { get; set; } = new();
    [JsonProperty("yAxis")] public ColumnChartYAxis YAxis { get; set; } = new();
    [JsonProperty("tooltip")] public ChartTooltip Tooltip { get; set; } = new();
    [JsonProperty("plotOptions")] public ColumnChartPlotOptions PlotOptions { get; set; } = new();
    [JsonProperty("series")] public List<string> Series { get; set; } = new();
    public ColumnChart()
    {
        ChartName = GetType().Name;
    }
}
public class ColumnChartXAxis
{
    [JsonProperty("categories")] public string Categories { get; set; } = String.Empty;
    [JsonProperty("crosshair")] public bool Crosshair { get; set; } = true;
    [JsonProperty("accessibility")] public Accessibility Accessibility { get; set; } = new();
    [JsonProperty("title")] public ChartTitle Title { get; set; } = new();
}
public class Accessibility
{
    [JsonProperty("description")] public string Description { get; set; } = String.Empty;
}
public class ColumnChartYAxis
{
    [JsonProperty("min")] public int Min { get; set; } = 0;
    [JsonProperty("title")] public ChartTitle Title { get; set; } = new();
}
public class ColumnChartPlotOptions
{
    [JsonProperty("column")] public PlotOptionsColumn Column { get; set; } = new();
}
public class PlotOptionsColumn
{
    [JsonProperty("pointPadding")] public float PointPadding { get; set; } = 0.2f;
    [JsonProperty("borderWidth")] public int BorderWidth { get; set; } = 0;
}
//public class ColumnChartSeries<T>
//{
//    [JsonProperty("name")] public string Name { get; set; } = String.Empty;
//    [JsonProperty("data")] public List<T> Data { get; set; } = new();
//}