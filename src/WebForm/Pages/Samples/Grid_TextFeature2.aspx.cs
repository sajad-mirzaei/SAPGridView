using System;
using System.Collections.Generic;
using SAP.WebControls;
using System.Data;

public partial class Grid_TextFeature2 : System.Web.UI.Page
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
                new Column { Data = "colA", Title = "TitleA" },
                new Column { Data = "colB", Title = "TitleB",
                    Functions = {
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "colA % 2",
                            IsTrueText = "متن تست"
                        }
                    }
                },
                new Column { Data = "colC", Title = "TitleC" },
                new Column { Data = "colD", Title = "TitleD" },
                new Column { Data = "customColA", DefaultContent = "", Title = "customTitleA",
                    Functions = {
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "1==1",
                            IsTrueText = "<a target='_blank' href='colC'>colC</a>",
                            NumericCheckInText = true //IsTrueText columns and IsFalseText columns does not replace with numeric data (default is true)
                        }
                    }
                },
                new Column { Data = "customColB", DefaultContent = "", Title = "customTitleB",
                    Functions = {
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "1==1",
                            IsTrueText = "<a target='_blank' href='colC'>colC</a>",
                            NumericCheckInText = false  //IsTrueText columns and IsFalseText columns replace with numeric data (default is true)
                        }
                    }
                },
                new Column { Data = "customColC", DefaultContent = "", Title = "customTitleC",
                    Functions = {
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "'colB' == 'bb 2'",
                            IsTrueText = "<a target='_blank' href='colB'>click on colB</a>",
                            IsFalseText = "colB",
                            NumericCheckInText = false,
                            NumericCheckInCondition = false //Condition columns does not replace with numeric data (default is true)
                        }
                    }
                },
                new Column { Data = "customColD", DefaultContent = "", Title = "customTitleD",
                    Functions = {
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "1==1",
                            IsTrueText = "colD",
                            ChangeOriginalData = true, //TextFeature change original data if this property assingen to true (excel export show difference with other columns)
                            StrReplace = { { "dd", "" } }
                        }
                    }
                }
            }
        };

        oSGV.GridBind("MyGrid1");
    }


    public DataTable MakeDataTable()
    {
        DataTable oDT = new DataTable();
        oDT.Columns.Add("colA", typeof(int));
        oDT.Columns.Add("colB", typeof(string));
        oDT.Columns.Add("colC", typeof(string));
        oDT.Columns.Add("colD", typeof(string));

        for (int i = 0; i < 10; i++)
        {
            DataRow Row1 = oDT.NewRow();
            Row1["colA"] = i;
            Row1["colB"] = "bb " + i;
            Row1["colC"] = "cc " + i;
            Row1["colD"] = "dd " + i;
            oDT.Rows.Add(Row1);
        }
        return oDT;
    }

}