using System;
using System.Collections.Generic;
using SAP.WebControls;
using System.Data;

public partial class AutoGenerate_1 : System.Web.UI.Page
{
    public static SAPGridView oSGV = new SAPGridView();
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = MakeDataTable();

        oSGV.Grids["MyGrid1"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 400,
            Data = dt
        };

        oSGV.GridBind("MyGrid1");
    }


    public DataTable MakeDataTable()
    {
        DataTable oDT = new DataTable();
        oDT.Columns.Add("a", typeof(int));
        oDT.Columns.Add("b", typeof(string));
        oDT.Columns.Add("c", typeof(string));
        oDT.Columns.Add("d", typeof(string));

        for (int i = 0; i < 100; i++)
        {
            DataRow Row1 = oDT.NewRow();
            Row1["a"] = i;
            Row1["b"] = "bb " + i;
            Row1["c"] = "cc " + i;
            Row1["d"] = "dd " + i;

            oDT.Rows.Add(Row1);
        }
        return oDT;
    }

}