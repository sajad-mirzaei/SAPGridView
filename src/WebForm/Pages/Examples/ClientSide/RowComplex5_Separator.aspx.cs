using SAP.WebControls;
using System;
using System.Collections.Generic;
using System.Data;

public partial class Grid_RowComplex5_Separator : System.Web.UI.Page
{
    public static SAPGridView oSGV = new SAPGridView();
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = BuildFlatData();
        oSGV.Grids["MyGrid1"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 450,
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
        oSGV.GridBind("MyGrid1");
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
        dt.Columns.Add("Count", typeof(int));
        dt.Columns.Add("Amount", typeof(double));
        dt.Rows.Add(1, 1, "ProductName_1", 11, 1, "SupplierName_1", 11, 11.1);
        dt.Rows.Add(2, 2, "ProductName_2", 12, 2, "SupplierName_2", 22, 22.2);
        dt.Rows.Add(3, 3, "ProductName_3", 13, 2, "SupplierName_2", 33, 33.3);
        dt.Rows.Add(4, 4, "ProductName_4", 14, 2, "SupplierName_2", 44, 44.4);
        dt.Rows.Add(5, 5, "ProductName_5", 15, 2, "SupplierName_2", 55, 550);
        dt.Rows.Add(6, 6, "ProductName_6", 16, 2, "SupplierName_2", 66, 660);
        dt.Rows.Add(7, 6, "ProductName_6", 16, 1, "SupplierName_1", 77, 770);
        return dt;
    }
}