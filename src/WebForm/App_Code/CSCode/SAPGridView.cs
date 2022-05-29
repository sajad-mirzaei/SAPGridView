using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace SAP.WebControls
{
    public class SAPGridView
    {
        public Dictionary<string, string> DefaultParameters = new Dictionary<string, string>();
        public Dictionary<string, Grid> Grids = new Dictionary<string, Grid>();
        public object CustomData ;
        public SAPGridView()
        {
            Grids = new Dictionary<string, Grid>();
            DefaultParameters = new Dictionary<string, string>();
        }
        public void GridBind(params string[] GridNameList)
        {
            Dictionary<string, Grid> GridsToBind = new Dictionary<string, Grid>();
            foreach (var GridName in GridNameList)
            {
                Grid g = Grids[GridName].DeepCopy();
                if (g.GridParameters.Count == 0)
                    g.GridParameters = new Dictionary<string, string>(DefaultParameters);
                GridsToBind.Add(GridName, g);
            }
            var ThisPage = HttpContext.Current.CurrentHandler as Page;
            var AllData = new Dictionary<string, object>()
            {
                { "Grids", GridsToBind },
                { "CustomData", CustomData }
            };
            var JsonData = JsonConvert.SerializeObject(AllData);
            ScriptManager.RegisterStartupScript(ThisPage, ThisPage.GetType(), "", "<script>SapGridViewJSBind(" + JsonData + ", 1, '1')</script>", false);
        }
        public string AjaxBind(params string[] GridNameList)
        {
            Dictionary<string, Grid> GridsToBind = new Dictionary<string, Grid>();
            foreach (var GridName in GridNameList)
            {
                Grid g = Grids[GridName].DeepCopy();
                if (g.GridParameters.Count == 0)
                    g.GridParameters = new Dictionary<string, string>(DefaultParameters);
                GridsToBind.Add(GridName, g);
            }
            var AllData = new Dictionary<string, object>()
            {
                { "Grids", GridsToBind },
                { "CustomData", CustomData }
            };
            var x = JsonConvert.SerializeObject(AllData);
            return x;
        }
    }
    public class CustomData
    {
        [JsonProperty("dataName")] public string DataName { get; set; }
        [JsonProperty("dataKey")] public string DataKey;
        [JsonProperty("dataValue")] public string DataValue;
        [JsonProperty("Data")] public object Data { 
            get { 
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
    public class Grid
    {
        [JsonProperty("containerId")] public string ContainerId { get; set; }
        [JsonProperty("containerHeight")] public int ContainerHeight { get; set; }
        [JsonProperty("data")] public object Data { get; set; }
        [JsonProperty("gridTitle")] public string GridTitle { get; set; }
        [JsonProperty("counterColumn")] public Boolean CounterColumn { get; set; }
        [JsonProperty("columns")] public List<Column> Columns { get; set; }
        [JsonProperty("options")] public Option Options { get; set; }
        [JsonProperty("gridParameters")] public Dictionary<string, string> GridParameters = new Dictionary<string, string>();
        [JsonProperty("headerComplex")] public List<HeaderComplexRow> HeaderComplex { get; set; }
        [JsonProperty("serverSidePagination")] public ServerSidePagination ServerSidePagination { get; set; }
        public Grid()
        {
            CounterColumn = true;
            Columns = new List<Column>();
            HeaderComplex = new List<HeaderComplexRow>();
            Options = new Option();
        }
        public Grid DeepCopy()
        {
            Grid o = (Grid)this.MemberwiseClone();
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
    public class HeaderComplexRow
    {
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("columnsToBeMerged")] public List<string> ColumnsToBeMerged { get; set; }
        public HeaderComplexRow()
        {
            ColumnsToBeMerged = new List<string>();
        }
    }
    public class Option
    {
        [JsonProperty("pageLength")] public int PageLength { get; set; }
        [JsonProperty("info")] public Boolean Info { get; set; }
        [JsonProperty("lengthMenu")] public string LengthMenu { get; set; }
        [JsonProperty("paging")] public Boolean Paging { get; set; }
        [JsonProperty("order")] public string Order { get; set; }
        /// <summary> کپی جدول </summary>
        [JsonProperty("copyButton")] public Boolean CopyButton { get; set; }
        /// <summary> پرینت جدول </summary>
        [JsonProperty("printButton")] public Boolean PrintButton { get; set; }
        /// <summary> خروجی اکسل </summary>
        [JsonProperty("excelButton")] public Boolean ExcelButton { get; set; }
        /// <summary> جستجو روی هر سطر جدول </summary>
        [JsonProperty("columnsSearchButton")] public Boolean ColumnsSearchButton { get; set; }
        /// <summary> فیلتر روی هر سطر جدول </summary>
        [JsonProperty("dropDownFilterButton")] public Boolean DropDownFilterButton { get; set; }
        /// <summary> حذف وضعیت های ذخیره شده </summary>
        [JsonProperty("recycleButton")] public Boolean RecycleButton { get; set; }
        /// <summary> جستجو در کل جدول </summary>
        [JsonProperty("gridSearchTextBox")] public Boolean GridSearchTextBox { get; set; }
        /// <summary> حذف عنوان جدول از سطر اول خروجی اکسل </summary>
        [JsonProperty("titleRowInExelExport")] public Boolean TitleRowInExelExport { get; set; }
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
    public class Column
    {
        [JsonProperty("data")] public string Data { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        private string CssClassField;
        [JsonProperty("className")] public string CssClass { get { return CssClassField + " " + Data + "_Class"; } set { CssClassField = value + " " + Data + "_Class"; } }
        [JsonProperty("defaultContent")] public string DefaultContent { get; set; }
        [JsonProperty("orderable")] public Boolean Orderable { get; set; }
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

            Column o = (Column)this.MemberwiseClone();
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
            return (Function)this.MemberwiseClone();
        }
    }
    public class Calc : Function
    {
        [JsonProperty("formula")] public string Formula { get; set; }
        [JsonProperty("operator")] public OperatorValue Operator { get; set; }
        [JsonProperty("numericCheck")] public Boolean NumericCheck { get; set; }
        public enum OperatorValue
        {
            VerticalSum
        }
        public Calc()
        {
            Section = SectionValue.Tbody;
            FuncName = this.GetType().Name;
            NumericCheck = true;
        }
        public override object DeepCopy()
        {
            Calc c = (Calc)this.MemberwiseClone();
            return c;
        }
    }
    public class Separator : Function
    {
        /// <summary> تعداد اعداد بعد از ممیز، پیشفرض 3 می باشد </summary>
        [JsonProperty("decimalPlaces")] public int DecimalPlaces {
            get { 
                return this.MaximumFractionDigits; 
            }
            set {
                this.MaximumFractionDigits = value;
            }
        }
        /// <summary> ماکزیمم تعداد اعداد بعد از ممیز، پیشفرض 3 می باشد </summary>
        [JsonProperty("minimumFractionDigits")] public int MinimumFractionDigits { get; set; }
        /// <summary> مینیمم تعداد اعداد بعد از ممیز، پیشفرض 0 می باشد </summary>
        [JsonProperty("maximumFractionDigits")] public int MaximumFractionDigits { get; set; }
        /// <summary> en-US فرمت مخصوص زبان، پیشفرض </summary>
        [JsonProperty("locales")] public string Locales { get; set; }

        public Separator()
        {
            FuncName = this.GetType().Name;
            Section = SectionValue.Tbody;
            DecimalPlaces = 3;
            MinimumFractionDigits = 0;
            Locales = "en-US";
        }
        public override object DeepCopy()
        {
            Separator c = (Separator)this.MemberwiseClone();
            return c;
        }
    }
    public class MiladiToJalali : Function
    {
        [JsonProperty("outPut")] public DateValue Output { get; set; }
        [JsonProperty("zeroPad")] public bool ZeroPad { get; set; }
        public enum DateValue
        {

            /// <summary> فقط زمان را نشان می دهد </summary>
            TimeOnly,
            /// <summary> فقط تاریخ را نشان می دهد </summary>
            DateOnly,
            /// <summary> تاریخ و زمان را نشان می دهد </summary>
            FullDate,
            /// <summary> فقط زمان را با ثانیه نشان می دهد </summary>
            TimeOnlyWithSecond,
            /// <summary> تاریخ و زمان را با ثانیه نشان می دهد  </summary>
            FullDateWithSecond
        }
        public MiladiToJalali()
        {
            Section = SectionValue.Tbody;
            FuncName = this.GetType().Name;
            Output = DateValue.DateOnly;
            ZeroPad = true;
        }
        public override object DeepCopy()
        {
            MiladiToJalali c = (MiladiToJalali)this.MemberwiseClone();
            return c;
        }
    }
    public class SAPCheckBox : Function
    {
        [JsonProperty("cssClass")] public string CssClass { get; set; }
        [JsonProperty("enable")] public Boolean Enable { get; set; }
        public SAPCheckBox()
        {
            Section = SectionValue.Tbody;
            FuncName = this.GetType().Name;
            Enable = true;
        }
        public override object DeepCopy()
        {
            SAPCheckBox c = (SAPCheckBox)this.MemberwiseClone();
            return c;
        }
    }
    public class OnClick : Function
    {
        [JsonProperty("httpRequestType")] public HttpRequestTypeValue HttpRequestType { get; set; }
        [JsonProperty("hrefLink")] public string HrefLink { get; set; }
        [JsonProperty("cssClass")] public string CssClass { get; set; }
        [JsonProperty("footerText")] public string FooterText { get; set; }
        [JsonProperty("enable")] public Boolean Enable { get; set; }
        [JsonProperty("webMethodName")] public string WebMethodName { get; set; }
        [JsonProperty("javaScriptMethodName")] public string JavaScriptMethodName { get; set; }
        /// <summary>yourTitle - {clickedItem} : انتخاب نام برای تب، یک کلمه یا رشته می پذیرد می توان بصورت ترکیبی کلمه - گزینه ای که کلیک شده تعریف کرد مثال </summary>
        /// <summary>   </summary>
        [JsonProperty("nextTabTitle")] public string NextTabTitle { get; set; }
        public List<string> DataKeys { get; set; }
        public string NextGrid { get; set; }
        public string Level { get; set; }
        public enum HttpRequestTypeValue
        {
            /// <summary> بدون لود شدن دوباره کل صفحه </summary>
            Ajax,
            /// <summary> برای زمانیکه فقط یک لینک باید داشته باشد مثلا به صفحه ای دیگر </summary>
            PageLink,
            /// <summary> برای زمانیکه بعد از کلیک یک متد در جاوااسکریپت باید فراخوانی شود </summary>
            CallJavaScriptMethod
        }
        public OnClick()
        {
            HrefLink = "javascript:void(0)";
            FuncName = this.GetType().Name;
            Enable = true;
            WebMethodName = "SapGridEvent";
            HttpRequestType = HttpRequestTypeValue.Ajax;
            DataKeys = new List<string>();
            Section = SectionValue.Tbody;
            NextTabTitle = "{clickedItem}";
        }
        public override object DeepCopy()
        {
            OnClick o = (OnClick)this.MemberwiseClone();
            o.DataKeys = new List<string>(o.DataKeys);
            return o;
        }
    }
    public class CumulativeSum : Function
    {
        [JsonProperty("sourceField")] public string SourceField { get; set; }
        public CumulativeSum()
        {
            FuncName = this.GetType().Name;
        }
        public override object DeepCopy()
        {
            CumulativeSum c = (CumulativeSum)this.MemberwiseClone();
            return c;
        }
    }
    public class TextFeature : Function
    {
        [JsonProperty("isTrueText")] public string IsTrueText { get; set; }
        [JsonProperty("isFalseText")] public string IsFalseText { get; set; }
        [JsonProperty("condition")] public string Condition { get; set; }
        [JsonProperty("isTrueCssClass")] public string IsTrueCssClass { get; set; }
        [JsonProperty("isFalseCssClass")] public string IsFalseCssClass { get; set; }
        [JsonProperty("strReplace")] public Dictionary<string, string> StrReplace { get; set; }
        [JsonProperty("numericCheckInText")] public Boolean NumericCheckInText { get; set; }
        [JsonProperty("numericCheckInCondition")] public Boolean NumericCheckInCondition { get; set; }
        [JsonProperty("changeOriginalData")] public Boolean ChangeOriginalData { get; set; }
        public TextFeature()
        {
            StrReplace = new Dictionary<string, string>();
            FuncName = this.GetType().Name;
            NumericCheckInText = true;
            NumericCheckInCondition = true;
            ChangeOriginalData = false;
        }
        public override object DeepCopy()
        {
            TextFeature c = (TextFeature)this.MemberwiseClone();
            return c;
        }
    }
    public class SapGridCallBackEvent
    {
        public OnClick FuncArray { get; set; }
        public Dictionary<string, string> RowData { get; set; }
        public Dictionary<string, string> GridParameters { get; set; }
        public Dictionary<string, string> TableDetails { get; set; }
    }
    public class ServerSidePagination
    {
    }

}