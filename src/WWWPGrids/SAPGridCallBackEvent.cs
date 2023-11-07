/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class SAPGridCallBackEvent
    {
        public OnClick FuncArray { get; set; }
        public Dictionary<string, string> RowData { get; set; }
        public Dictionary<string, string> GridParameters { get; set; }
        public Dictionary<string, string> TableDetails { get; set; }
    }
}