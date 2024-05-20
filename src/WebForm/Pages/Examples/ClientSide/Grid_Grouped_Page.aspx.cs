using SAP.WebControls;
using System;
using System.Collections.Generic;

public partial class Grid_Grouped_Page : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DefineGrid();
    }
    public void DefineGrid()
    {
        List<Dictionary<string, string>> oArrayTest = new List<Dictionary<string, string>>();
        for (int i = 0; i < 200; i++)
        {
            int c = i / 6;
            oArrayTest.Add(
                new Dictionary<string, string>() {
                    { "id", i.ToString() },
                    { "first_name", "FirstName " + i },
                    { "last_name", "LastName " + i },
                    { "position", "Position " + i },
                    { "office", "Office_" + c },
                    { "start_date", "1st Jan 70" },
                    { "salary", "$40000" + i }
                }
            );
        }
        SAPGridView oSGV = new SAPGridView();
        oSGV.Grids["Test1"] = new Grid()
        {
            ContainerHeight = 300,
            ContainerId = "MyGridId",
            Data = oArrayTest,
            Columns = new List<Column>() {
                new Column { Data = "first_name", Title = "نام" },
                new Column { Data = "last_name", Title = "نام فامیل" },
                new Column { Data = "position", Title = "جایگاه" },
                new Column { Data = "office", Title = "دفتر", RowGrouping = new RowGrouping { Enable = true, CssClass = "aa" } },
                new Column { Data = "start_date", Title = "تاریخ" },
                new Column { Data = "salary", Title = "حقوق" }
            }
        };
        oSGV.GridBind("Test1");
    }
}