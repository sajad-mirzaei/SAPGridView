using Newtonsoft.Json;
using System.Data;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class CustomData
    {
        [JsonProperty("dataName")] public string DataName { get; set; }
        [JsonProperty("dataKey")] public string DataKey;
        [JsonProperty("dataValue")] public string DataValue;
        [JsonProperty("Data")]
        public object Data
        {
            get
            {
                return ((DataTable)DicData)
                    //if DataKey doesn't unique, make it unique
                    .AsEnumerable().GroupBy(r => r.Field<dynamic>(DataKey)).Select(g => g.First()).CopyToDataTable()
                    //convert DataTable to Dictionary for javascript object array
                    .AsEnumerable().ToDictionary<DataRow, string, string>(row => row.Field<dynamic>(DataKey).ToString(), row => row.Field<dynamic>(DataValue).ToString());
            }
            set
            {
                DicData = value;
            }
        }
        private object DicData;
    }
}