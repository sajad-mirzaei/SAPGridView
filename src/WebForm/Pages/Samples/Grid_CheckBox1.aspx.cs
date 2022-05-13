using System;
using System.Collections.Generic;
using SAP.WebControls;
using System.Data;

public partial class Grid_CheckBox1 : System.Web.UI.Page
{
    public static SAPGridView oSGV = new SAPGridView();
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = MakeDataTable();

        oSGV.Grids["MyGrid1"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 400,
            Data = dt,
            Columns = new List<Column>() {
                new Column { Data = "y", Title = "Name 0", DefaultContent = "",
                    Functions = { 
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "1==1",
                            IsTrueText = "<input type='checkbox' name='checkbox1' data-value='x1'>",
                            IsFalseText = "",
                        }
                    }
                },
                new Column { Data = "x1", Title = "Name 1" },
                new Column { Data = "x2", Title = "Name 2" },
                new Column { Data = "x3", Title = "Name 3" },
                new Column { Data = "x4", Title = "Name 4" }
            }
        };

        oSGV.GridBind("MyGrid1");
    }


    public DataTable MakeDataTable()
    {
        DataTable oDT = new DataTable();
        oDT.Columns.Add("x1", typeof(int));
        oDT.Columns.Add("x2", typeof(string));
        oDT.Columns.Add("x3", typeof(string));
        oDT.Columns.Add("x4", typeof(string));

        for (int i = 0; i < 10; i++)
        {
            DataRow Row1 = oDT.NewRow();
            Row1["x1"] = i;
            Row1["x2"] = i + 1;
            Row1["x3"] = i + 2;
            Row1["x4"] = i + 3;
            oDT.Rows.Add(Row1);
        }
        return oDT;
    }

}