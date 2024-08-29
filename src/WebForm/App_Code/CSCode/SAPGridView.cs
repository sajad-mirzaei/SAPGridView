using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace SAP.WebControls
{
    public class SAPGridView
    {
        public Dictionary<string, string> DefaultParameters = new Dictionary<string, string>();
        public Dictionary<string, Grid> Grids = new Dictionary<string, Grid>();
        public object CustomData;
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
            ScriptManager.RegisterStartupScript(ThisPage, ThisPage.GetType(), Guid.NewGuid().ToString(), "<script>SapGridViewJSBind(" + JsonData + ", 1, '1')</script>", false);
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
    public class Grid
    {
        [JsonProperty("containerId")] public string ContainerId { get; set; }
        [JsonProperty("containerHeight")] public int ContainerHeight { get; set; }
        [JsonProperty("data")] public object Data { get; set; }
        [JsonProperty("gridTitle")] public string GridTitle { get; set; }
        [JsonProperty("counterColumn")] public Boolean CounterColumn { get; set; }
        [JsonProperty("columns")] public List<Column> Columns { get; set; }
        [JsonProperty("options")] public Option Options { get; set; }
        [JsonProperty("customizeButtons")] public List<CustomizeButton> CustomizeButtons { get; set; }
        [JsonProperty("gridParameters")] public Dictionary<string, string> GridParameters = new Dictionary<string, string>();
        [JsonProperty("headerComplex")] public List<HeaderComplexRow> HeaderComplex { get; set; }
        [JsonProperty("charts")] public List<Chart> Charts { get; set; }
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
            Charts = new List<Chart>();
        }
        public Grid DeepCopy()
        {
            Grid o = (Grid)this.MemberwiseClone();
            o.Columns = new List<Column>(o.Columns);
            o.Charts = new List<Chart>(o.Charts);
            o.HeaderComplex = new List<HeaderComplexRow>(o.HeaderComplex);
            for (var i = 0; i < o.Columns.Count; i++)
            {
                if (o.Columns[i] != null)
                    o.Columns[i] = o.Columns[i].DeepCopy();
            }
            for (var j = 0; j < o.Charts.Count; j++)
            {
                if (o.Charts[j] != null)
                    o.Charts[j] = (Chart)o.Charts[j].DeepCopy();
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
        /// <summary> حذف همه فیلترها و جستجوها </summary>
        [JsonProperty("removeAllFilters")] public Boolean RemoveAllFilters { get; set; }
        /// <summary> جستجو در کل جدول </summary>
        [JsonProperty("gridSearchTextBox")] public Boolean GridSearchTextBox { get; set; }
        /// <summary> حذف عنوان جدول از سطر اول خروجی اکسل </summary>
        [JsonProperty("titleRowInExcelExport")] public Boolean TitleRowInExcelExport { get; set; }
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
            RemoveAllFilters = true;
            GridSearchTextBox = true;
            TitleRowInExcelExport = true;
        }
    }
    public class CustomizeButton
    {
        [JsonProperty("buttonName")] public ButtonNames ButtonName { get; set; }
        [JsonProperty("javascriptMethodName")] public string JavascriptMethodName { get; set; }
        [JsonProperty("data")] public Dictionary<string, string> Data { get; set; }
        public CustomizeButton()
        {
            Data = new Dictionary<string, string>();
        }
    }
    public enum ButtonNames
    {
        Copy = 1,
        Print = 2,
        Excel = 3,
        ColumnsSearch = 4,
        Recycle = 5,
        DropDownFilter = 6
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
        [JsonProperty("decimalPlaces")]
        public int DecimalPlaces
        {
            get
            {
                return this.MaximumFractionDigits;
            }
            set
            {
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
        [JsonProperty("enable")] public bool Enable { get; set; }
        [JsonProperty("javascriptMethodName")] public string JavascriptMethodName { get; set; }
        [JsonProperty("selectAll")] public bool SelectAll { get; set; }
        public SAPCheckBox()
        {
            Section = SectionValue.Tbody;
            CssClass = "sapCheckBox";
            FuncName = this.GetType().Name;
            Enable = true;
            JavascriptMethodName = "sapJavascriptMethodName";
            SelectAll = true;
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
    public class RowComplex
    {
        public string PrimaryKeyId { get; set; }
        public string GroupBy { get; set; }
        public string ColumnToPivotName { get; set; }
        public string ColumnToPivotId { get; set; }
        //public bool AllowDuplicateColumnToPivot { get; set; }
        public string FirstComplexedColumnTitle { get; set; }
        public string FirstComplexedColumn { get; set; }
        public int DefaultValueType { get; set; }
        public string DefaultRefType { get; set; }
        public string TrOddCssClass { get; set; }
        public string TrEvenCssClass { get; set; }
        public string TableHeight { get; set; }
        public string TrHeight { get; set; }
        public string TableCssClass { get; set; }
        public string GridName { get; set; }
        public List<ComplexColumn> ComplexColumns { get; set; }

        private List<string> ColumnsToComplex { get; set; }
        private List<string> ColumnsToComplexTitle { get; set; }

        /// <summary>
        /// تعیین این پارامتر یعنی نیاز به پیوت دیتا می باشد
        /// در صورت تعیین این مورد دیگر نیازی به تعیین دیتا برای گرید نیست
        /// </summary>
        public DataTable FlatDataForPivot { get; set; }

        public RowComplex()
        {
            FirstComplexedColumn = "DataOfComplexedColumn";
            FirstComplexedColumnTitle = "";
            PrimaryKeyId = "Id";
            DefaultValueType = 0;
            DefaultRefType = "-";
            TrOddCssClass = "table-info";
            TrEvenCssClass = "table-warning";
            TrHeight = "20px";
            TableHeight = "40px";
            TableCssClass = "table";
            //AllowDuplicateColumnToPivot = true;
            FlatDataForPivot = null;
        }
        private void SetComplexColumns()
        {
            ColumnsToComplex = new List<string>();
            ColumnsToComplexTitle = new List<string>();
            foreach (ComplexColumn item in ComplexColumns)
            {
                ColumnsToComplex.Add(item.Data);
                ColumnsToComplexTitle.Add(item.Title);
            }
        }

        public DataTable BuildPivotData(DataTable rawData)
        {
            #region Add first columns & define variables
            SetComplexColumns();
            var checkDuplicateRows = new List<int>();
            var checkDuplicateColumns = new List<int>();
            var checkDuplicatePivotColumns = new List<int>();
            DataTable pivotDataTable = new DataTable();

            pivotDataTable.Columns.Add(GroupBy, typeof(int));
            pivotDataTable.PrimaryKey = new DataColumn[] { pivotDataTable.Columns[GroupBy] };

            string[] temp = new string[] { GroupBy, ColumnToPivotName };
            List<string> anotherColumns = new List<string>();
            foreach (DataColumn column in rawData.Columns)
            {
                if (temp.Contains(column.ColumnName) == false && ColumnsToComplex.Contains(column.ColumnName) == false)
                {
                    pivotDataTable.Columns.Add(column.ColumnName, column.DataType);
                    anotherColumns.Add(column.ColumnName);
                }
            }
            #endregion


            #region Add pivot columns
            foreach (DataRow rawDataItem in rawData.Rows)
            {
                var PivotColumnName = ColumnToPivotName + rawDataItem[PrimaryKeyId].ToString();
                var primaryKeyValue = int.Parse(rawDataItem[PrimaryKeyId].ToString());
                var columnToPivotValue = int.Parse(rawDataItem[ColumnToPivotId].ToString());

                #region Add rows & set value to first columns-rows
                if (checkDuplicateRows.Contains(int.Parse(rawDataItem[GroupBy].ToString())) == false)
                {
                    checkDuplicateRows.Add(int.Parse(rawDataItem[GroupBy].ToString()));
                    DataRow row1 = pivotDataTable.NewRow();
                    row1[GroupBy] = rawDataItem[GroupBy];
                    foreach (string anotherColumnItem in anotherColumns)
                    {
                        row1[anotherColumnItem] = rawDataItem[anotherColumnItem];
                    }
                    pivotDataTable.Rows.Add(row1);
                }
                #endregion

                DataColumn newColumn1 = new DataColumn(PivotColumnName, rawData.Columns[ColumnToPivotName].DataType);
                newColumn1.DefaultValue = DefaultRefType;
                DataRow row = pivotDataTable.Rows.Find(rawDataItem[GroupBy]);
                if (checkDuplicateColumns.Contains(primaryKeyValue) == false && checkDuplicatePivotColumns.Contains(columnToPivotValue) == false)
                {
                    checkDuplicateColumns.Add(primaryKeyValue);
                    checkDuplicatePivotColumns.Add(columnToPivotValue);
                    pivotDataTable.Columns.Add(newColumn1);
                    row[PivotColumnName] = rawDataItem[ColumnToPivotName];
                }

                foreach (string columnsToComplexItem in ColumnsToComplex)
                {
                    var colName = columnsToComplexItem + rawDataItem[ColumnToPivotId].ToString();
                    var colType = rawData.Columns[columnsToComplexItem].DataType;

                    if (pivotDataTable.Columns.Contains(colName) == false)
                    {
                        DataColumn newColumn = new DataColumn(colName, colType);
                        if (colType.IsValueType)
                            newColumn.DefaultValue = Convert.ChangeType(DefaultValueType, colType);
                        else
                            newColumn.DefaultValue = DefaultRefType;
                        pivotDataTable.Columns.Add(newColumn);
                    }

                    row[colName] = rawDataItem[columnsToComplexItem];
                }
            }
            #endregion

            return pivotDataTable;
        }

        public List<Column> AddColumns(List<Column> columns, DataTable rawData)
        {
            SetComplexColumns();
            string titleRow, bodyRow, bodyRowsSample, cssClass;
            titleRow = "<table class='" + TableCssClass + ";' style='height:" + TableHeight + ";'>";
            bodyRowsSample = "<table class='" + TableCssClass + "' style='height:" + TableHeight + ";'>";
            var i = 0;
            foreach (string columnToComplexTitleItem in ColumnsToComplexTitle)
            {
                //titleRow
                cssClass = i % 2 == 0 ? TrEvenCssClass : TrOddCssClass;
                titleRow += "<tr style='height:" + TrHeight + ";' class='" + cssClass + "'><td>" + columnToComplexTitleItem + "</td></tr>";

                //bodyRow
                var colName = ColumnsToComplex[i] + "ColumnToPivotIdMustBeReplaced";
                var colType = rawData.Columns[ColumnsToComplex[i]].DataType;
                cssClass = i % 2 == 0 ? TrEvenCssClass : TrOddCssClass;
                bodyRowsSample += "<tr style='height:" + TrHeight + ";' class='" + cssClass + "'><td> " + colName + " </td></tr>";

                i++;
            }
            titleRow += "</table>";
            bodyRowsSample += "</table>";

            columns.Add(new Column { Data = FirstComplexedColumn, Title = FirstComplexedColumnTitle, DefaultContent = titleRow });
            List<string> duplicatedColumns = new List<string>();
            foreach (DataRow rawDataItem in rawData.Rows)
            {
                var PivotColumnName = ColumnToPivotName + rawDataItem[ColumnToPivotId].ToString();
                if (duplicatedColumns.Contains(PivotColumnName) == false)
                {
                    duplicatedColumns.Add(PivotColumnName);
                    DataColumn newColumn1 = new DataColumn(PivotColumnName, rawData.Columns[ColumnToPivotName].DataType);
                    newColumn1.DefaultValue = DefaultRefType;
                    bodyRow = bodyRowsSample.Replace("ColumnToPivotIdMustBeReplaced", rawDataItem[ColumnToPivotId].ToString());
                    columns.Add(new Column
                    {
                        Data = PivotColumnName,
                        Title = rawDataItem[ColumnToPivotName].ToString(),
                        DefaultContent = "",
                        Functions = {
                            new TextFeature {
                                Section = Function.SectionValue.Tbody,
                                ChangeOriginalData = true,
                                Condition = "1==1",
                                IsTrueText = bodyRow,
                                NumericCheckInText = false
                            }
                        }
                    });
                }
            }
            return columns;
        }
    }
    public class ComplexColumn
    {
        public string Title { get; set; }
        public string Data { get; set; }
    }

    #region Charts

    public class Chart
    {
        [JsonProperty("chartName")] protected string ChartName { get; set; }
        [JsonProperty("chartContainerId")] public string ChartContainerId { get; set; }
        public virtual object DeepCopy()
        {
            return (Chart)MemberwiseClone();
        }
    }

    public class ChartTitle
    {
        [JsonProperty("text")] public string Text { get; set; }
        [JsonProperty("align")] public TitleAlign Align { get; set; }

        public ChartTitle()
        {
            Text = string.Empty;
            Align = TitleAlign.Center;
        }
    }

    public class ChartSubTitle
    {
        [JsonProperty("text")] public string Text { get; set; }
        [JsonProperty("align")] public TitleAlign Align { get; set; }

        public ChartSubTitle()
        {
            Text = string.Empty;
            Align = TitleAlign.Center;
        }
    }


    public class ChartTooltip
    {
        [JsonProperty("valueSuffix")] public string ValueSuffix { get; set; }
        public ChartTooltip()
        {
            ValueSuffix = string.Empty;
        }
    }

    public enum TitleAlign
    {
        Right = 0,
        Center = 1,
        Left = 2
    }

    //-ColumnChart-------------
    public class ColumnChart : Chart
    {
        [JsonProperty("title")] public ChartTitle Title { get; set; }
        [JsonProperty("subTitle")] public ChartSubTitle SubTitle { get; set; }
        [JsonProperty("xAxis")] public ColumnChartXAxis XAxis { get; set; }
        [JsonProperty("yAxis")] public ColumnChartYAxis YAxis { get; set; }
        [JsonProperty("tooltip")] public ChartTooltip Tooltip { get; set; }
        [JsonProperty("plotOptions")] public ColumnChartPlotOptions PlotOptions { get; set; }
        [JsonProperty("series")] public List<string> Series { get; set; }
        public ColumnChart()
        {
            ChartName = GetType().Name;
            Title = new ChartTitle();
            SubTitle = new ChartSubTitle();
            XAxis = new ColumnChartXAxis();
            YAxis = new ColumnChartYAxis();
            Tooltip = new ChartTooltip();
            PlotOptions = new ColumnChartPlotOptions();
            Series = new List<string>();
        }
    }
    public class ColumnChartXAxis
    {
        [JsonProperty("categories")] public string Categories { get; set; }
        [JsonProperty("crosshair")] public bool Crosshair { get; set; }
        [JsonProperty("accessibility")] public Accessibility Accessibility { get; set; }
        [JsonProperty("title")] public ChartTitle Title { get; set; }

        public ColumnChartXAxis()
        {
            Categories = String.Empty;
            Crosshair = true;
            Accessibility = new Accessibility();
            Title = new ChartTitle();
        }
    }
    public class Accessibility
    {
        [JsonProperty("description")] public string Description { get; set; }

        public Accessibility()
        {
            Description = String.Empty;
        }
    }
    public class ColumnChartYAxis
    {
        [JsonProperty("min")] public int Min { get; set; }
        [JsonProperty("title")] public ChartTitle Title { get; set; }

        public ColumnChartYAxis()
        {
            Min = 0;
            Title = new ChartTitle();
        }
    }
    public class ColumnChartPlotOptions
    {
        [JsonProperty("column")] public PlotOptionsColumn Column { get; set; }

        public ColumnChartPlotOptions()
        {
            Column = new PlotOptionsColumn();
        }
    }
    public class PlotOptionsColumn
    {
        [JsonProperty("pointPadding")] public float PointPadding { get; set; }
        [JsonProperty("borderWidth")] public int BorderWidth { get; set; }

        public PlotOptionsColumn()
        {
            PointPadding = 0.2f;
            BorderWidth = 0;
        }
    }

    //-LineChart-------------
    public class LineChart : Chart
    {
        [JsonProperty("title")] public ChartTitle Title { get; set; }
        [JsonProperty("subTitle")] public ChartSubTitle SubTitle { get; set; }
        [JsonProperty("xAxis")] public LineChartXAxis XAxis { get; set; }
        [JsonProperty("yAxis")] public LineChartYAxis YAxis { get; set; }
        [JsonProperty("tooltip")] public ChartTooltip Tooltip { get; set; }
        [JsonProperty("plotOptions")] public LineChartPlotOptions PlotOptions { get; set; }
        [JsonProperty("series")] public List<string> Series { get; set; }
        public LineChart()
        {
            ChartName = GetType().Name;
            Title = new ChartTitle();
            SubTitle = new ChartSubTitle();
            XAxis = new LineChartXAxis();
            YAxis = new LineChartYAxis();
            Tooltip = new ChartTooltip();
            PlotOptions = new LineChartPlotOptions();
            Series = new List<string>();
        }
    }
    public class LineChartXAxis
    {
        [JsonProperty("categories")] public string Categories { get; set; }

        public LineChartXAxis()
        {
            Categories = String.Empty;
        }
    }
    public class LineChartYAxis
    {
        [JsonProperty("title")] public ChartTitle Title { get; set; }

        public LineChartYAxis()
        {
            Title = new ChartTitle();
        }
    }
    public class LineChartPlotOptions
    {
        [JsonProperty("line")] public PlotOptionsLine Line { get; set; }

        public LineChartPlotOptions()
        {
            Line = new PlotOptionsLine();
        }
    }
    public class PlotOptionsLine
    {
        [JsonProperty("dataLabels")] public PlotOptionsLineDataLabels DataLabels { get; set; }
        [JsonProperty("enableMouseTracking")] public bool EnableMouseTracking { get; set; }

        public PlotOptionsLine()
        {
            DataLabels = new PlotOptionsLineDataLabels();
            EnableMouseTracking = true;
        }
    }
    public class PlotOptionsLineDataLabels
    {
        [JsonProperty("enabled")] public bool Enabled { get; set; }

        public PlotOptionsLineDataLabels()
        {
            Enabled = true;
        }
    }

    //-PieChart----------------
    public class PieChart : Chart
    {
        [JsonProperty("key")] public string Key { get; set; }
        [JsonProperty("value")] public string Value { get; set; }
        [JsonProperty("seriesName")] public string SeriesName { get; set; }
        [JsonProperty("title")] public ChartTitle Title { get; set; }
        [JsonProperty("subTitle")] public ChartSubTitle SubTitle { get; set; }
        public PieChart()
        {
            ChartName = GetType().Name;
            SeriesName = "Series 1";
        }
    }

    #endregion
}