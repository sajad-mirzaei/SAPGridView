using Newtonsoft.Json;
using SAP.WebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public partial class CumulativeSumFunc : System.Web.UI.Page
{
    public static SAPGridView oSGV = new SAPGridView();
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (Session["dtCumulativeSum"] == null)
        {
            dt = MakeDataTable();
            string JsonData = JsonConvert.SerializeObject(dt);
            Session["dtCumulativeSum"] = JsonData;
        }
        else
        {
            string c = Session["dtCumulativeSum"].ToString();
            dt = JsonConvert.DeserializeObject<DataTable>(c);
        }
        DataTable dtCustom = MakeCustomDataTable();

        //--MyGrid1------------------------------------------
        oSGV.Grids["MyGrid1"] = new Grid()
        {
            ContainerId = "MyGridId1",
            ContainerHeight = 400,
            Data = dt,
            Columns = new List<Column>() {
                new Column { Data = "a", Title = "A"},
                new Column { Data = "b", Title = "B" },
                new Column { Data = "c", Title = "C" },
                new Column { Data = "d", Title = "D" },

                new Column { Data = "e", Title = "e = ((a-b) + (c-d))", DefaultContent = "", CssClass = "ltr",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tbody, Formula = " (( a - b ) + ( c - d )) " }
                    }
                },
                new Column { Data = "f", Title = "f = CumulativeSum(e)", DefaultContent = "", CssClass = "ltr",
                    Functions = {
                        new CumulativeSum { Section = Function.SectionValue.Tbody, SourceField = "e" }
                    }
                },
                new Column { Data = "g", Title = "g = CumulativeSum((( a - b ) + ( c - d )))", DefaultContent = "", CssClass = "ltr",
                    Functions = {
                        new CumulativeSum { Section = Function.SectionValue.Tbody, SourceField = "(( a - b ) + ( c - d ))" }
                    }
                },
                new Column { Data = "h", Title = "h = f > 0 ? f : 0", DefaultContent = "", CssClass = "ltr",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tbody, Formula = " f > 0 ? f : 0 " }
                    }
                },
                new Column { Data = "i", Title = "i = f < 0 ? f : 0", DefaultContent = "", CssClass = "ltr",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tbody, Formula = " f < 0 ? f : 0 " }
                    }
                }
            }
        };
        //--MyGrid2------------------------------------------
        oSGV.Grids["MyGrid2"] = new Grid()
        {
            ContainerId = "MyGridId2",
            ContainerHeight = 400,
            Data = dt,
            Columns = new List<Column>() {
                new Column { Data = "a", Title = "A"},
                new Column { Data = "b", Title = "B" },
                new Column { Data = "c", Title = "C" },
                new Column { Data = "d", Title = "D" },

                new Column { Data = "e", Title = "e = CumulativeSum((( a - b ) + ( c - d )))", DefaultContent = "", CssClass = "ltr",
                    Functions = {
                        new CumulativeSum { Section = Function.SectionValue.Tbody, SourceField = "(( a - b ) + ( c - d ))" }
                    }
                },
                new Column { Data = "h", Title = "h = e > 0 ? e : 0", DefaultContent = "", CssClass = "ltr",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tbody, Formula = " e > 0 ? e : 0 " }
                    }
                },
                new Column { Data = "i", Title = "i = e < 0 ? e : 0", DefaultContent = "", CssClass = "ltr",
                    Functions = {
                        new Calc { Section = Function.SectionValue.Tbody, Formula = " e < 0 ? e : 0 " }
                    }
                }
            }
        };

        oSGV.GridBind("MyGrid1", "MyGrid2");
    }


    public DataTable MakeDataTable()
    {
        DataTable oDT = new DataTable();
        oDT.Columns.Add("id", typeof(int));
        oDT.Columns.Add("a", typeof(string));
        oDT.Columns.Add("b", typeof(string));
        oDT.Columns.Add("c", typeof(string));
        oDT.Columns.Add("d", typeof(string));

        Random rnd = new Random();
        for (int i = 0; i < 10; i++)
        {
            DataRow Row1 = oDT.NewRow();
            Row1["id"] = i;
            Row1["a"] = rnd.Next(-50, 52);
            Row1["b"] = rnd.Next(-50, 52);
            Row1["c"] = rnd.Next(-50, 52);
            Row1["d"] = rnd.Next(-50, 52);
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


    protected void ChangeData_Click(object sender, EventArgs e)
    {
        string JsonData = JsonConvert.SerializeObject(MakeDataTable());
        Session["dtCumulativeSum"] = JsonData;
        Response.Redirect(HttpContext.Current.Request.Url.ToString());
    }
}