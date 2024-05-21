using System;
using System.Collections.Generic;
using SAP.WebControls;
using System.Data;

public partial class SumFooter1 : System.Web.UI.Page
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
            GridTitle = "گزارش تست 1",
            Options = new Option() {
                Order = "[[2, 'desc']]"
            },
            Columns = new List<Column>() {
                new Column { Data = "a", Title = "aa" },
                new Column { Data = "b", Title = "bb" },
                new Column { Data = "c", Title = "cc",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new Separator { Section = Function.SectionValue.Tfoot }
                    }
                },
                new Column { Data = "d", Title = "dd",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new Separator { Section = Function.SectionValue.Tfoot }
                    }
                },
                new Column { Data = "e", Title = "{body=a}{footer=c+d}", DefaultContent = "",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tbody, Formula = "a" },
                        new Calc { Section = Function.SectionValue.Tfoot, Formula = "c + d" },
                        new Separator { Section = Function.SectionValue.Tfoot }
                    }
                },
                new Column { Data = "f", Title = "{body=b}{footer=c/d}", DefaultContent = "",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tbody, Formula = "b" },
                        new Calc { Section = Function.SectionValue.Tfoot, Formula = "c / d" },
                        new Separator { Section = Function.SectionValue.Tfoot }
                    }
                }
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

        for (int i = 0; i < 100; i++)
        {
            DataRow Row1 = oDT.NewRow();
            Row1["a"] = i + 10;
            Row1["b"] = i + 100;
            Row1["c"] = i + 1000;
            Row1["d"] = i + 10000;
            oDT.Rows.Add(Row1);
        }
        return oDT;
    }

}