using Newtonsoft.Json;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
    public class Option
    {
        [JsonProperty("pageLength")] public int PageLength { get; set; }
        [JsonProperty("info")] public bool Info { get; set; }
        [JsonProperty("lengthMenu")] public string LengthMenu { get; set; }
        [JsonProperty("paging")] public bool Paging { get; set; }
        [JsonProperty("order")] public string Order { get; set; }
        /// <summary> کپی جدول </summary>
        [JsonProperty("copyButton")] public bool CopyButton { get; set; }
        /// <summary> پرینت جدول </summary>
        [JsonProperty("printButton")] public bool PrintButton { get; set; }
        /// <summary> خروجی اکسل </summary>
        [JsonProperty("excelButton")] public bool ExcelButton { get; set; }
        /// <summary> جستجو روی هر سطر جدول </summary>
        [JsonProperty("columnsSearchButton")] public bool ColumnsSearchButton { get; set; }
        /// <summary> فیلتر روی هر سطر جدول </summary>
        [JsonProperty("dropDownFilterButton")] public bool DropDownFilterButton { get; set; }
        /// <summary> حذف وضعیت های ذخیره شده </summary>
        [JsonProperty("recycleButton")] public bool RecycleButton { get; set; }
        /// <summary> جستجو در کل جدول </summary>
        [JsonProperty("gridSearchTextBox")] public bool GridSearchTextBox { get; set; }
        /// <summary> حذف عنوان جدول از سطر اول خروجی اکسل </summary>
        [JsonProperty("titleRowInExelExport")] public bool TitleRowInExelExport { get; set; }
        public Option()
        {
            PageLength = 10;
            Info = true;
            LengthMenu = "[[10, 20, 30, 40, 50, 100, -1], [10, 20, 30, 40, 50, 100, \"همه\"]]";
            Paging = true;
            CopyButton = true;
            PrintButton = true;
            ExcelButton = true;
            ColumnsSearchButton = true;
            RecycleButton = true;
            DropDownFilterButton = false;
            GridSearchTextBox = true;
            TitleRowInExelExport = true;
        }
    }
}