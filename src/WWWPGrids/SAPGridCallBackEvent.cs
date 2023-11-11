/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class SAPGridCallBackEvent
    {
        public OnClick FuncArray { get; set; } = new();
        public Dictionary<string, string> RowData { get; set; } = new();
        public Dictionary<string, string> GridParameters { get; set; } = new();
        public Dictionary<string, string> TableDetails { get; set; } = new();
    }
}