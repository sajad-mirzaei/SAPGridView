using Newtonsoft.Json;
using SAP.WebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Services;
using System.Web.UI;

public partial class Grid_Levels1 : Page
{
    public static SAPGridView oSGV = new SAPGridView();
    public static Dictionary<string, string> oSGVDefaultParams = new Dictionary<string, string>()
    {
        { "Level", "" },
        { "AzTarikh", "" },
        { "TaTarikh", "" },
        { "Id", "" }
    };

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            dtbAzTarikh.Text = DateTime.Now.AddMonths(-5).ToPersianDate().ToString().Replace("/", "");
            dtbTaTarikh.Text = DateTime.Now.ToPersianDate().ToString().Replace("/", "");
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        CreateGrids();
        oSGVDefaultParams["Level"] = "1";
        oSGVDefaultParams["AzTarikh"] = (dtbAzTarikh.Text).ToGregorianDate().ToString();
        oSGVDefaultParams["TaTarikh"] = (dtbTaTarikh.Text).ToGregorianDate().ToString();
        DataTable dt = Get_DataTable1(oSGVDefaultParams);
        if (dt.Rows.Count > 0)
        {
            oSGV.Grids["Grid1"].Data = dt;
            oSGV.GridBind("Grid1");
            Div_Grids.Visible = true;
        }
        else
        {
            lblErrorBox.ShowAlert("برای موارد انتخاب شما اطلاعاتی موجود نیست", AlertBox.info);
            Div_Grids.Visible = false;
        }
    }

    public static void CreateGrids()
    {
        //-----------------------Grid1------------------------
        oSGV.Grids["Grid1"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 300,
            Options = new Option { 
                DropDownFilterButton = true, 
                ExcelButton = true, 
                CopyButton = true, 
                PrintButton = true, 
                RecycleButton = true, 
                ColumnsSearchButton = true,
                Info = true,
                GridSearchTextBox = false
            },
            Columns = new List<Column>() {
                new Column { Title ="شماره ثبت", Data="Id" },
                new Column { Title ="تاریخ", Data="Tarikh",
                    Functions =
                    {
                        new MiladiToJalali {Section = Calc.SectionValue.Tbody, Output = MiladiToJalali.DateValue.FullDate },
                        new OnClick { Section = Function.SectionValue.Tbody, Level = "2", NextGrid = "Grid2", DataKeys = { "Id" } }
                    }
                },
                new Column { Title ="ستون 1", Data="Col1",
                    Functions =
                    {
                        new Calc {Section = Calc.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new Separator { Section = Separator.SectionValue.Tbody, DecimalPlaces = 0 },
                        new Separator { Section = Separator.SectionValue.Tfoot, DecimalPlaces = 0 }
                    }
                },
                new Column { Title ="ستون 2", Data="Col2" },
                new Column { Title ="ستون 3", Data="Col3" }
            }
        };
        //-----------------------Grid2------------------------
        oSGV.Grids["Grid2"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 300,
            Columns = new List<Column>() {
                new Column { Title ="شماره ثبت", Data="Id" },
                new Column { Title ="تاریخ", Data="Tarikh",
                    Functions =
                    {
                        new MiladiToJalali {Section = Calc.SectionValue.Tbody, Output = MiladiToJalali.DateValue.FullDate }
                    }
                },
                new Column { Title ="مشخصه 1", Data="property1",
                    Functions =
                    {
                        new Calc {Section = Calc.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new Separator { Section = Separator.SectionValue.Tbody, DecimalPlaces = 0 },
                        new Separator { Section = Separator.SectionValue.Tfoot, DecimalPlaces = 0 }
                    }
                },
                new Column { Title ="مشخصه 2", Data="property2" },
                new Column { Title ="مشخصه 3", Data="property3" }
            }
        };
    }
    
    protected static DataTable Get_DataTable1(Dictionary<string, string> param)
    {
        //در صورتیکه اطلاعات را از دیتابیس فراخوانی میکنید، نیازی به این متد نیست
        DataTable dt = new DataTable();
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("Tarikh", typeof(DateTime));
        dt.Columns.Add("Col1", typeof(string));
        dt.Columns.Add("Col2", typeof(string));
        dt.Columns.Add("Col3", typeof(string));

        for (int i = 1; i <= 20; i++)
        {
            DataRow row = dt.NewRow();
            row["Id"] = i;
            row["Col1"] = i + 10000;
            row["Col2"] = "Col2 - " + i;
            row["Col3"] = "Col3 - " + i;
            row["Tarikh"] = DateTime.Now.AddMonths(-1 * i);
            dt.Rows.Add(row);
        }
        var result = dt
                    .AsEnumerable()
                    .Where(myRow => myRow.Field<DateTime>("Tarikh") >= DateTime.Parse(param["AzTarikh"]) && myRow.Field<DateTime>("Tarikh")  <= DateTime.Parse(param["TaTarikh"]))
                    .CopyToDataTable();

        return result;
    }
    
    protected static DataTable Get_DataTable2(Dictionary<string, string> param)
    {
        //در صورتیکه اطلاعات را از دیتابیس فراخوانی میکنید، نیازی به این متد نیست
        DataTable dt = new DataTable();
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("Tarikh", typeof(DateTime));
        dt.Columns.Add("property1", typeof(string));
        dt.Columns.Add("property2", typeof(string));
        dt.Columns.Add("property3", typeof(string));

        for (int i = 1; i <= 20; i++)
        {
            DataRow row = dt.NewRow();
            row["Id"] = i;
            row["property1"] = i + 10000 * int.Parse(param["Id"]);
            row["property2"] = "col" + param["Id"] + " - property2 - " + i;
            row["property3"] = "col" + param["Id"] + " - property3 - " + i;
            row["Tarikh"] = DateTime.Now.AddMonths(-1 * i);
            dt.Rows.Add(row);
        }
        var result = dt
                    .AsEnumerable()
                    .Where(myRow => myRow.Field<DateTime>("Tarikh") >= DateTime.Parse(param["AzTarikh"]) && myRow.Field<DateTime>("Tarikh")  <= DateTime.Parse(param["TaTarikh"]))
                    .CopyToDataTable();

        return result;
    }

    [WebMethod]
    public static string SapGridEvent(string CallBackData)
    {
        SapGridCallBackEvent oData = JsonConvert.DeserializeObject<SapGridCallBackEvent>(CallBackData);
        //همه اطلاعات سطری که روی یکی از فیلدهای آن کلیک شده
        var RowData = oData.RowData;
        List<string> DataKeys = oData.FuncArray.DataKeys;
        string NextGrid = oData.FuncArray.NextGrid;
        string Clicked_CellName = oData.TableDetails["CellName"];
        int Level = int.Parse(oData.FuncArray.Level);

        if (oSGV.Grids.Count == 0)
        {
            CreateGrids();
        }

        foreach (KeyValuePair<string, string> item in oData.GridParameters)
        {
            oSGVDefaultParams[item.Key] = oData.GridParameters[item.Key];
        }

        oSGVDefaultParams["Level"] = oData.FuncArray.Level;

        foreach (string DataKey in DataKeys)
        {
            if (RowData.Count != 0)
                oSGVDefaultParams[DataKey] = RowData[DataKey];
        }

        oSGV.Grids[NextGrid].GridParameters = oSGVDefaultParams;

        oSGV.Grids[NextGrid].Data = Get_DataTable2(oSGVDefaultParams);

        return oSGV.AjaxBind(NextGrid);
    }
}