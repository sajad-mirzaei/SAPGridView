using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class Grid
    {
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
            HeaderComplex = new List<HeaderComplexRow>();
            CustomizeButtons = new List<CustomizeButton>();
            Options = new Option();
            RowComplex = new RowComplex();
        }
        public Grid DeepCopy()
        {
            Grid o = (Grid)MemberwiseClone();
            o.Columns = new List<Column>(o.Columns);
            o.HeaderComplex = new List<HeaderComplexRow>(o.HeaderComplex);
            for (var i = 0; i < o.Columns.Count; i++)
            {
                if (o.Columns[i] != null)
                    o.Columns[i] = o.Columns[i].DeepCopy();
            }
            return o;
        }
    }
}