using System;
using System.Collections.Generic;
using SAP.WebControls;
using System.Data;

public partial class Grid_CustomData: System.Web.UI.Page
{
    public static SAPGridView oSGV = new SAPGridView();
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = MakeDataTable();
        DataTable dtCustom = MakeCustomDataTable();

        oSGV.CustomData = new {
            dtCustom = new CustomData { Data = dtCustom, DataKey = "id", DataValue = "name" }
        };

        oSGV.Grids["MyGrid1"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 400,
            Data = dt,
            Columns = new List<Column>() {
                new Column { Data = "id", Title = "کد دیتابیس" },
                new Column { Data = "id", Title = "نام دیتابیس", 
                    Functions = { 
                        new Calc { Section = Function.SectionValue.Tbody, Formula = "CustomData.dtCustom.Data['id']" }
                    }
                },
                new Column { Data = "y", Title = "نام", CssClass = "text-info bg-danger" },
                new Column { Data = "z", Title = "فامیلی" },
                new Column { Data = "w", Title = "معدل" }
            }
        };

        oSGV.GridBind("MyGrid1");
    }


    public DataTable MakeDataTable()
    {
        DataTable oDT = new DataTable();
        oDT.Columns.Add("id", typeof(int));
        oDT.Columns.Add("y", typeof(string));
        oDT.Columns.Add("z", typeof(string));
        oDT.Columns.Add("w", typeof(string));

        for (int i = 0; i < 10; i++)
        {
            DataRow Row1 = oDT.NewRow();
            Row1["id"] = i;
            Row1["y"] = "yy " + i;
            Row1["z"] = "zz " + i;
            Row1["w"] = "ww " + i;
            oDT.Rows.Add(Row1);
        }
        return oDT;
    }
    public DataTable MakeCustomDataTable()
    {
        DataTable oDT = new DataTable();
        oDT.Columns.Add("id", typeof(int));
        oDT.Columns.Add("name", typeof(string));

        for (int i = 0; i < 10; i++)
        {
            DataRow Row1 = oDT.NewRow();
            Row1["id"] = i;
            Row1["name"] = "Name_ " + i;
            oDT.Rows.Add(Row1);
        }
        return oDT;
    }

}