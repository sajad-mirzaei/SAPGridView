using System;
using System.Collections.Generic;
using SAP.WebControls;
using System.Data;

public partial class Grid_RowComplex4 : System.Web.UI.Page
{
    public static SAPGridView oSGV = new SAPGridView();
    protected void Page_Load(object sender, EventArgs e)
    {
        BuildGrid1();
        BuildGrid2();
        oSGV.GridBind("MyGrid1", "MyGrid2");
    }
    public void BuildGrid1()
    {
        DataTable dt = BuildFlatData1();
        oSGV.Grids["MyGrid1"] = new Grid()
        {
            ContainerId = "MyGridId1",
            ContainerHeight = 400,
            GridTitle = "گزارش تست 1",
            Columns = new List<Column>() {
                new Column { Data = "ProductName", Title = "ProductName" }
            },
            RowComplex = new RowComplex()
            {
                FlatDataForPivot = dt,

                PrimaryKeyId = "Id",
                ColumnToPivotId = "SupplierId",
                ColumnToPivotName = "SupplierName",
                //AllowDuplicateColumnToPivot = false,
                GroupBy = "ProductId",
                FirstComplexedColumnTitle = "ComplexTitle1",
                ComplexColumns = new List<ComplexColumn> {
                    new ComplexColumn { Data = "Amount", Title = "AmountTitle" },
                    new ComplexColumn { Data = "Count", Title = "CountTitle" }
                }
            }
        };
    }
    public void BuildGrid2()
    {
        DataTable dt = BuildFlatData2();
        oSGV.Grids["MyGrid2"] = new Grid()
        {
            ContainerId = "MyGridId2",
            ContainerHeight = 400,
            GridTitle = "گزارش تست 1",
            Columns = new List<Column>() {
                new Column { Data = "ProductName", Title = "ProductName" }
            },
            RowComplex = new RowComplex()
            {
                FlatDataForPivot = dt,

                PrimaryKeyId = "Id",
                ColumnToPivotId = "SupplierId",
                ColumnToPivotName = "SupplierName",
                GroupBy = "ProductId",
                FirstComplexedColumnTitle = "ComplexTitle1",
                ComplexColumns = new List<ComplexColumn> {
                    new ComplexColumn { Data = "Amount", Title = "AmountTitle" },
                    new ComplexColumn { Data = "Count", Title = "CountTitle" }
                }
            }
        };
    }

    public DataTable BuildFlatData1()
    {
        var dt = new DataTable();
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("ProductId", typeof(int));
        dt.Columns.Add("ProductName", typeof(string));
        dt.Columns.Add("ProductCode", typeof(int));
        dt.Columns.Add("SupplierId", typeof(int));
        dt.Columns.Add("SupplierName", typeof(string));
        dt.Columns.Add("Count", typeof(int));
        dt.Columns.Add("Amount", typeof(double));
        dt.Rows.Add(1, 1, "ProductName_1", 11, 2, "SupplierName_2", 11, 11.1);
        dt.Rows.Add(2, 2, "ProductName_2", 12, 1, "SupplierName_1", 22, 22.2);
        dt.Rows.Add(3, 3, "ProductName_3", 13, 1, "SupplierName_1", 33, 33.3);
        dt.Rows.Add(4, 4, "ProductName_4", 14, 1, "SupplierName_1", 44, 44.4);
        dt.Rows.Add(5, 5, "ProductName_5", 15, 1, "SupplierName_1", 55, 550);
        dt.Rows.Add(6, 6, "ProductName_6", 16, 1, "SupplierName_1", 66, 660);
        dt.Rows.Add(7, 6, "ProductName_6", 16, 2, "SupplierName_2", 77, 770);
        return dt;
    }
    public DataTable BuildFlatData2()
    {
        var dt = new DataTable();
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("ProductId", typeof(int));
        dt.Columns.Add("ProductName", typeof(string));
        dt.Columns.Add("ProductCode", typeof(int));
        dt.Columns.Add("SupplierId", typeof(int));
        dt.Columns.Add("SupplierName", typeof(string));
        dt.Columns.Add("Count", typeof(int));
        dt.Columns.Add("Amount", typeof(double));
        dt.Rows.Add(1, 1, "ProductName_1", 11, 1, "SupplierName_1", 11, 11.1);
        dt.Rows.Add(2, 2, "ProductName_2", 12, 1, "SupplierName_1", 22, 22.2);
        dt.Rows.Add(3, 3, "ProductName_3", 13, 1, "SupplierName_1", 33, 33.3);
        dt.Rows.Add(4, 4, "ProductName_4", 14, 1, "SupplierName_1", 44, 44.4);
        dt.Rows.Add(5, 5, "ProductName_5", 15, 1, "SupplierName_1", 55, 550);
        dt.Rows.Add(6, 6, "ProductName_6", 16, 2, "SupplierName_2", 66, 660);
        dt.Rows.Add(7, 6, "ProductName_6", 16, 2, "SupplierName_2", 77, 770);
        return dt;
    }
}