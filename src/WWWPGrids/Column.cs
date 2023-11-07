using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class Column
    {
        [JsonProperty("data")] public string Data { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        private string CssClassField;
        [JsonProperty("className")] public string CssClass { get { return CssClassField + " " + Data + "_Class"; } set { CssClassField = value + " " + Data + "_Class"; } }
        [JsonProperty("defaultContent")] public string DefaultContent { get; set; }
        [JsonProperty("orderable")] public bool Orderable { get; set; }
        [JsonProperty("width")] public string Width { get; set; }
        [JsonProperty("visible")] public bool Visible { get; set; }
        [JsonProperty("dropDownFilter")] public bool DropDownFilter { get; set; }
        [JsonProperty("rowGrouping")] public RowGrouping RowGrouping { get; set; }
        [JsonProperty("functions")] public List<Function> Functions { get; set; }
        public Column()
        {
            Visible = true;
            Orderable = true;
            DropDownFilter = true;
            Functions = new List<Function>();
            RowGrouping = null;
        }
        public Column DeepCopy()
        {

            Column o = (Column)MemberwiseClone();
            if (o.Functions != null)
            {
                o.Functions = new List<Function>(o.Functions);
                for (var i = 0; i < o.Functions.Count; i++)
                {
                    o.Functions[i] = (Function)o.Functions[i].DeepCopy();
                }
            }

            return o;
        }
    }
}