using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class Function
    {
        [JsonProperty("funcName")] protected string FuncName { get; set; }
        [JsonProperty("section")] public SectionValue Section { get; set; }
        public enum SectionValue
        {
            /// <summary> فقط روی تیتر جدول اعمال شود </summary>
            Thead,
            /// <summary> فقط روی سطرها و بدنه ی جدول اعمال شود </summary>
            Tbody,
            /// <summary> فقط روی پاورقی جدول اعمال شود </summary>
            Tfoot
        }
        public virtual object DeepCopy()
        {
            return (Function)MemberwiseClone();
        }
    }
}