using System;
using System.Collections.Generic;
using SAP.WebControls;
using SAP.Utility;
using System.Data;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Services;

public partial class Grid_HeaderComplex_1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DefineGrid();
    }
    public void DefineGrid()
    {
        List<Dictionary<string, int>> oArrayTest = new List<Dictionary<string, int>>();
        for (int i = 0; i < 200; i++)
        {
            int c = i / 6;
            oArrayTest.Add(
                new Dictionary<string, int>() {
                    { "id", i },
                    { "a", i },
                    { "b", i + 1},
                    { "c", i },
                    { "d", i },
                    { "e", i },
                    { "f", i }
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
                new Column { Data = "a", Title = "A"},
                new Column { Data = "b", Title = "B" },
                new Column { Data = "c", Title = "C" },
                new Column { Data = "d", Title = "D" },
                new Column { Data = "e", Title = "E" },
                new Column { Data = "f", Title = "F" }
            }
        };
        oSGV.Grids["Test1"].HeaderComplex = new List<HeaderComplexRow>() { 
            new HeaderComplexRow { Title = "AB", ColumnsToBeMerged = { "a", "b" } },
            new HeaderComplexRow { Title = "EF", ColumnsToBeMerged = { "e", "f" } }
        };
        oSGV.GridBind("Test1");
    }
}