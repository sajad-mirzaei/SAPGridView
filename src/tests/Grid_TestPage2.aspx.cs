using System;
using System.Collections.Generic;
using SAP.WebControls;
using SAP.Utility;
using System.Data;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Services;

public partial class GridTestPage_Simple : System.Web.UI.Page
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
                new Column { Data = "x", Title = "کد دیتابیس",
                    Functions = { 
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum }
                    }
                },
                new Column { Data = "y", Title = "نام", CssClass = "text-info bg-danger" },
                new Column { Data = "z", Title = "فامیلی" },
                new Column { Data = "w", Title = "معدل" },
                new Column { Data = "t", Title = "تاریخ", CssClass = "ltr",
                    Functions = { 
                        new MiladiToJalali { Output = MiladiToJalali.DateValue.FullDate, ZeroPad = true }
                    }
                }
            }
        };

        oSGV.GridBind("MyGrid1");
    }


    public DataTable MakeDataTable()
    {
        DataTable oDT = new DataTable();
        oDT.Columns.Add("x", typeof(int));
        oDT.Columns.Add("y", typeof(string));
        oDT.Columns.Add("z", typeof(string));
        oDT.Columns.Add("w", typeof(string));
        oDT.Columns.Add("t", typeof(string));

        for (int i = 0; i < 10; i++)
        {
            DataRow Row1 = oDT.NewRow();
            Row1["x"] = i;
            Row1["y"] = "yy " + i;
            Row1["z"] = "zz " + i;
            Row1["w"] = "ww " + i;
            Row1["t"] = "2021-05-16 1:" + i.ToString() + ":" + i.ToString();
            oDT.Rows.Add(Row1);
        }
        return oDT;
    }
}