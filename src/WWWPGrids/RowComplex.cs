using System.Data;
/// <summary>
/// Summary description for SAPGridView
/// </summary>
namespace WWWPGrids
{
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
}