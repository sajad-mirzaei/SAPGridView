using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class HeaderComplexRow
    {
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("columnsToBeMerged")] public List<string> ColumnsToBeMerged { get; set; }
        public HeaderComplexRow()
        {
            ColumnsToBeMerged = new List<string>();
        }
    }
}