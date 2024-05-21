using SAP.WebControls;
using System;
using System.Collections.Generic;
using System.Data;

public partial class RowComplexC : System.Web.UI.Page
{
    public static SAPGridView oSGV = new SAPGridView();
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = BuildFlatData();
        oSGV.Grids["MyGrid1"] = new Grid()
        {
            ContainerId = "MyGridId1",
            ContainerHeight = 400,
            GridTitle = "گزارش تست 1",
            Columns = new List<Column>() {
                new Column { Data = "ProductName", Title = "ProductName" }
            }
        };
        var rowComplex = new RowComplex()
        {
            PrimaryKeyId = "Id",
            ColumnToPivotId = "SupplierId",
            ColumnToPivotName = "SupplierName",
            GroupBy = "ProductId",
            FirstComplexedColumnTitle = "ComplexTitle1",
            ComplexColumns = new List<ComplexColumn> {
                new ComplexColumn { Data = "Amount", Title = "AmountTitle" },
                new ComplexColumn { Data = "Count", Title = "CountTitle" }
            }
        };
        var pivotData = rowComplex.BuildPivotData(dt);
        oSGV.Grids["MyGrid1"].Columns = rowComplex.AddColumns(oSGV.Grids["MyGrid1"].Columns, dt);
        oSGV.Grids["MyGrid1"].Data = pivotData;


        oSGV.Grids["MyGrid2"] = new Grid()
        {
            ContainerId = "MyGridId2",
            ContainerHeight = 450,
            GridTitle = "گزارش تست 2",
            Data = pivotData
        };
        oSGV.GridBind("MyGrid1", "MyGrid2");
    }

    public DataTable BuildFlatData()
    {
        var dt = new DataTable();
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("ProductId", typeof(int));
        dt.Columns.Add("ProductName", typeof(string));
        dt.Columns.Add("ProductCode", typeof(int));
        dt.Columns.Add("SupplierId", typeof(int));
        dt.Columns.Add("SupplierName", typeof(string));
        dt.Columns.Add("Count", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        dt.Rows.Add(1, 1, "ProductName_1", 11, 1, "SupplierName_1", "one", 11.1);
        dt.Rows.Add(2, 2, "ProductName_2", 12, 2, "SupplierName_2", "two", 22.2);
        dt.Rows.Add(3, 3, "ProductName_3", 13, 2, "SupplierName_2", "three", 33.3);
        dt.Rows.Add(4, 4, "ProductName_4", 14, 2, "SupplierName_2", "three", 44.4);
        dt.Rows.Add(5, 5, "ProductName_5", 15, 2, "SupplierName_2", "sdfdsf1", 550);
        dt.Rows.Add(6, 6, "ProductName_6", 16, 2, "SupplierName_2", "three", 660);
        dt.Rows.Add(7, 6, "ProductName_6", 16, 1, "SupplierName_1", "three", 770);
        return dt;
    }
}