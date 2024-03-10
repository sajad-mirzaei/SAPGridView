using Newtonsoft.Json;
using WWWPGrids.Charts;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class Grid
    {
        [JsonProperty("processing")] public bool Processing { get; set; } = false;
        [JsonProperty("serverSide")] public bool ServerSide { get; set; } = false;

        [JsonProperty("serverSideOptions")] public ServerSideOption ServerSideOptions { get; set; } = new ServerSideOption();
        [JsonProperty("containerId")] public string ContainerId { get; set; }
        [JsonProperty("containerHeight")] public int ContainerHeight { get; set; }
        [JsonProperty("data")] public object Data { get; set; }
        [JsonProperty("gridTitle")] public string GridTitle { get; set; }
        [JsonProperty("counterColumn")] public bool CounterColumn { get; set; }
        [JsonProperty("columns")] public List<Column> Columns { get; set; }
        [JsonProperty("options")] public Option Options { get; set; }
        [JsonProperty("customizeButtons")] public List<CustomizeButton> CustomizeButtons { get; set; }
        [JsonProperty("gridParameters")] public Dictionary<string, string> GridParameters = new Dictionary<string, string>();
        [JsonProperty("headerComplex")] public List<HeaderComplexRow> HeaderComplex { get; set; }
        [JsonProperty("charts")] public List<Chart> Charts { get; set; }
        private RowComplex _rowComplex { get; set; }
        [JsonProperty("rowComplex")]
        public RowComplex RowComplex
        {
            get
            {
                return _rowComplex;
            }
            set
            {
                if (value != null && value.FlatDataForPivot != null)
                {
                    Columns = value.AddColumns(Columns, value.FlatDataForPivot);
                    Data = value.BuildPivotData(value.FlatDataForPivot);
                }
                _rowComplex = value;
            }
        }

        [JsonProperty("serverSidePagination")] public ServerSidePagination ServerSidePagination { get; set; }
        public Grid()
        {
            CounterColumn = true;
            Columns = new List<Column>();
            Charts = new List<Chart>();
            HeaderComplex = new List<HeaderComplexRow>();
            CustomizeButtons = new List<CustomizeButton>();
            Options = new Option();
            RowComplex = new RowComplex();
        }
        public Grid DeepCopy()
        {
            Grid o = (Grid)MemberwiseClone();
            o.Columns = new List<Column>(o.Columns);
            o.Charts = new List<Chart>(o.Charts);
            o.HeaderComplex = new List<HeaderComplexRow>(o.HeaderComplex);
            for (var i = 0; i < o.Columns.Count; i++)
            {
                if (o.Columns[i] != null)
                    o.Columns[i] = o.Columns[i].DeepCopy();
            }
            for (var j = 0; j < o.Charts.Count; j++)
            {
                if (o.Charts[j] != null)
                    o.Charts[j] = (Chart)o.Charts[j].DeepCopy();
            }
            return o;
        }
    }
}