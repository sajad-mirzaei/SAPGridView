using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class RowGrouping
    {
        [JsonProperty("enable")] public bool Enable { get; set; }
        [JsonProperty("cssClass")] public string CssClass { get; set; }
        public RowGrouping()
        {
            Enable = true;
            CssClass = "bg-secondary text-light";
        }
    }
}