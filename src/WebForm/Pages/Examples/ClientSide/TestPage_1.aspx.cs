using SAP.WebControls;
using System;
using System.Collections.Generic;
using System.Data;

public partial class GridTestPage_1 : System.Web.UI.Page
{
    public static SAPGridView oSGV = new SAPGridView();
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = MakeDataTable();

        oSGV.Grids["MyGrid1"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 600,
            Data = dt,
            GridTitle = "گزارش تست 1",
            Options = new Option() { DropDownFilterButton = true, TitleRowInExcelExport = false },
            Columns = new List<Column>() {
                new Column { Data = "v1", Title = "vv1", DefaultContent = "vv2", Visible = false },
                new Column { Data = "a", Title = "aa",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new Separator { Section = Function.SectionValue.Tfoot }
                    }
                },
                new Column { Data = "v2", Title = "vv2", DefaultContent = "vv2", Visible = false },
                new Column { Data = "CumulativeTest1", Title = "CumulativeSum(aa)", DefaultContent = "", CssClass = "ltr",
                    Functions = {
                        new CumulativeSum { Section = Function.SectionValue.Tbody, SourceField = "a" }
                    }
                },
                new Column { Data = "b", Title = "bb" },
                new Column { Data = "c", Title = "cc",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum }
                    }
                },
                new Column { Data = "d", Title = "dd" },
                new Column { Data = "e", Title = "ee",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum }
                    }
                },
                new Column { Data = "f", Title = "ff" }
            }
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
        oDT.Columns.Add("e", typeof(string));
        oDT.Columns.Add("f", typeof(string));

        for (int i = 0; i < 100; i++)
        {
            DataRow Row1 = oDT.NewRow();
            Row1["a"] = i + 1000;
            Row1["b"] = "bb " + i;
            Row1["c"] = i + 1;

            if (i % 2 == 0)
                Row1["d"] = "آریا اکبری";
            else
                Row1["d"] = "آريا اكبري";

            if (i < 3) Row1["e"] = 3;
            else if (i < 6) Row1["e"] = 6;
            else if (i < 9) Row1["e"] = 9;
            else if (i < 12) Row1["e"] = 12;
            else if (i < 15) Row1["e"] = 15;
            else if (i < 18) Row1["e"] = 18;
            else if (i < 21) Row1["e"] = 21;
            else Row1["e"] = 22;


            if (i < 8) Row1["f"] = "ff1";
            else if (i < 16) Row1["f"] = "ff2";
            else if (i < 24) Row1["f"] = "ff3";
            else if (i < 32) Row1["f"] = "ff4";
            else if (i < 40) Row1["f"] = "ff5";
            else if (i < 48) Row1["f"] = "ff6";
            else if (i < 56) Row1["f"] = "ff7";
            else Row1["f"] = "ff8";

            oDT.Rows.Add(Row1);
        }
        return oDT;
    }

}