using System;
using System.Collections.Generic;
using SAP.WebControls;
using SAP.Utility;
using System.Data;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Services;

public partial class GridTestPage : System.Web.UI.Page
{

    #region "Variables"
    public static SAPGridView oSGV = new SAPGridView();
    public static string ccSystem = "308700";
    string CodeNoeSys_Array = "";
    #endregion
    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        //ccSystem = Request.QueryString["cc"];
        lblErrorBox.Visible = false;
        if (!IsPostBack)
        {
            Set_Date();
            BindddlMarkaz();
        }
    }
    protected void btnKalaSearch_Click(object sender, EventArgs e)
    {

        if (BindCodeNoeSys_Array() == "")
        {
            lblErrorBox.ShowAlert("لطفا نوع کالا را انتخاب نمایید.", AlertBox.danger);
            return;
        }
        ddlKala.Items.Clear();
        ddlTaminKonandeh.Items.Clear();
        BindddlKala();
        BindddlTaminKonandeh();
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        if (!ChekData())
        {
            return;
        }
        SetFilterDefault();
        oSGV.GridBind("rptSefaresh");
    }
    #endregion
    #region Functions
    public void BindddlMarkaz()
    {
        ddlMarkaz.Items.Clear();
        DataTable dt = new DataTable();
        dt = new SystemPermission().Get_MarkazByPermission(new UserInformations().ccUser, int.Parse(ccSystem), 3);
        ListItem item = new ListItem("-- انتخاب کنید --", "0");
        ddlMarkaz.Items.Add(item);
        ddlMarkaz.DataSource = null;
        ddlMarkaz.DataSource = dt;
        ddlMarkaz.DataTextField = "NameMarkaz";
        ddlMarkaz.DataValueField = "ccMarkaz";
        ddlMarkaz.DataBind();
    }
    protected Boolean ChekData()
    {
        string strError = string.Empty;
        lblErrorBox.Text = "";

        if (dtbAzTarikh.Text.IsNullOrEmpty())
        {
            strError += "از تاریخ را وارد کنید ." + "<BR />";
        }
        else if (dtbTaTarikh.Text.IsNullOrEmpty())
        {
            strError += "تا تاریخ را وارد کنید ." + "<BR />";
        }
        else if (!SAP.Utility.PersianDate.IsValidPersianDate(dtbAzTarikh.Text))
        {
            strError += "از تاریخ  را صحیح وارد کنید ." + "<BR />";
        }
        else if (!SAP.Utility.PersianDate.IsValidPersianDate(dtbAzTarikh.Text))
        {
            strError += "تا تاریخ  را صحیح وارد کنید ." + "<BR />";
        }
        else if (int.Parse(dtbAzTarikh.Text) > int.Parse(dtbTaTarikh.Text))
        {
            strError += "ازتاریخ' باید از 'تاتاریخ' کوچکتر باشد' ." + "<BR />";
        }
        if (ddlMarkaz.SelectedValue == "" || ddlMarkaz.SelectedValue == "0")
        {
            strError += "لطفا مرکز را انتخاب نمایید." + "<BR />";
        }
        if (ddlNoeSefaresh.Text == "")
        {
            strError += "لطفا نوع سفارش را انتخاب نمایید." + "<BR />";
        }
        if (ddlCodeNoeSys.SelectedValue == "" || ddlCodeNoeSys.SelectedValue == "0")
        {
            strError += "لطفا نوع کالا را انتخاب نمایید." + "<BR />";
        }
        return lblErrorBox.ShowAlert(strError, AlertBox.danger);
    }
    public void SetFilterDefault()
    {
        DefineGrids();
        string Markaz_Array = BindMarkaz_Array();
        string NoeSefaresh_Array = BindNoeSefaresh_Array();
        string CodeNoeSys_Array = BindCodeNoeSys_Array();
        string ccKalaCode_Array = BindKala_Array();
        string ccTaminKonandeh_Array = BindTaminKonandeh_Array();
        string AzTarikh;
        string TaTarikh;
        AzTarikh = PersianDate.Parse(dtbAzTarikh.Text).ConvertToMiladi().Date.ToShortDateString().Split(new char[] { ' ', '\t' })[0];
        TaTarikh = PersianDate.Parse(dtbTaTarikh.Text).ConvertToMiladi().Date.ToShortDateString().Split(new char[] { ' ', '\t' })[0];
        oSGV.DefaultParameters["Level"] = "1";
        oSGV.DefaultParameters["AzTarikh"] = AzTarikh;
        oSGV.DefaultParameters["TaTarikh"] = TaTarikh;
        oSGV.DefaultParameters["NoeSefaresh"] = NoeSefaresh_Array;
        oSGV.DefaultParameters["ccMarkazAnbar"] = Markaz_Array;
        oSGV.DefaultParameters["CodeNoeSys"] = CodeNoeSys_Array;
        oSGV.DefaultParameters["ccKalaCode"] = ccKalaCode_Array;
        oSGV.DefaultParameters["ccTaminKonandeh"] = ccTaminKonandeh_Array;
        oSGV.DefaultParameters["ccSefaresh"] = "";
        oSGV.Grids["rptSefaresh"].Data = new Sefaresh().rpt_Sefaresh(oSGV.DefaultParameters);
    }
    void Set_Date()
    {
        var Tarikh = new PersianDate(DateTime.Now.Date);
        dtbAzTarikh.Text = Tarikh.ToString().Replace("/", "");
        dtbTaTarikh.Text = Tarikh.ToString().Replace("/", "");
    }
    protected string BindMarkaz_Array()
    {
        int[] ccMarkaz_Count = ddlMarkaz.GetSelectedIndices();
        string Markaz_Array = "";
        foreach (int i in ccMarkaz_Count)
        {
            if (Markaz_Array != "")
            {
                Markaz_Array += "," + ddlMarkaz.Items[i].Value.ToString();
            }
            else
            {
                Markaz_Array = ddlMarkaz.Items[i].Value.ToString();
            }
        }
        return Markaz_Array;
    }
    protected string BindNoeSefaresh_Array()
    {
        int[] NoeSefaresh_Count = ddlNoeSefaresh.GetSelectedIndices();
        string NoeSefaresh_Array = "";
        foreach (int i in NoeSefaresh_Count)
        {
            if (NoeSefaresh_Array != "")
            {
                NoeSefaresh_Array += "," + ddlNoeSefaresh.Items[i].Value.ToString();
            }
            else
            {
                NoeSefaresh_Array = ddlNoeSefaresh.Items[i].Value.ToString();
            }
        }
        return NoeSefaresh_Array;
    }
    public string BindCodeNoeSys_Array()
    {
        int[] CodeccSys_Count = ddlCodeNoeSys.GetSelectedIndices();
        foreach (int i in CodeccSys_Count)
        {
            if (CodeNoeSys_Array != "")
            {
                CodeNoeSys_Array += "," + ddlCodeNoeSys.Items[i].Value.ToString();
            }
            else
            {
                CodeNoeSys_Array = ddlCodeNoeSys.Items[i].Value.ToString();
            }
        }
        return CodeNoeSys_Array;
    }
    protected string BindKala_Array()
    {
        int[] Kala_Count = ddlKala.GetSelectedIndices();
        string ccKalaCode_Array = "";
        foreach (int i in Kala_Count)
        {
            if (ccKalaCode_Array != "")
            {
                ccKalaCode_Array += "," + ddlKala.Items[i].Value.ToString();
            }
            else
            {
                ccKalaCode_Array = ddlKala.Items[i].Value.ToString();
            }
        }
        return ccKalaCode_Array;
    }
    protected string BindTaminKonandeh_Array()
    {
        int[] TaminKonandeh_Count = ddlTaminKonandeh.GetSelectedIndices();
        string ccTaminKonandeh_Array = "";
        foreach (int i in TaminKonandeh_Count)
        {
            if (ccTaminKonandeh_Array != "")
            {
                ccTaminKonandeh_Array += "," + ddlTaminKonandeh.Items[i].Value.ToString();
            }
            else
            {
                ccTaminKonandeh_Array = ddlTaminKonandeh.Items[i].Value.ToString();
            }
        }
        return ccTaminKonandeh_Array;
    }
    protected void BindddlKala()
    {
        ListItem ItemKala;
        DataTable ccKala = new DataTable();
        ccKala = new Kala().Get_KalaByNoeSys(CodeNoeSys_Array);
        foreach (DataRow Item in ccKala.Rows)
        {
            var Value = Item["ccKalaCode"].ToString();
            var Text = Item["NameKala"].ToString();
            var SubText = Item["CodeKala"].ToString();
            ItemKala = new ListItem(Text, Value);
            ItemKala.Attributes.Add("data-subtext", " - کد کالا: " + SubText);
            ddlKala.Items.Add(ItemKala);
        }
    }
    protected void BindddlTaminKonandeh()
    {
        DataTable ccTaminKonandeh = new DataTable();
        ccTaminKonandeh = new TaminKonandeh().Get_TaminKonandehByKalaNoeSys(CodeNoeSys_Array);
        ListItem ItemTaminKonanade = new ListItem("تامین کننده عمومی", "0");
        ItemTaminKonanade.Attributes.Add("data-subtext", "0");
        ddlTaminKonandeh.Items.Add(ItemTaminKonanade);
        foreach (DataRow Item in ccTaminKonandeh.Rows)
        {
            var Value = Item["ccTaminKonandeh"].ToString();
            var Text = Item["NameTaminKonandeh"].ToString();
            var SubText = Item["CodeTaminKonandeh"].ToString();
            ItemTaminKonanade = new ListItem(Text, Value);
            ItemTaminKonanade.Attributes.Add("data-subtext", " - کد تامین کننده: " + SubText);
            ddlTaminKonandeh.Items.Add(ItemTaminKonanade);
        }
    }
    public static void DefineGrids()
    {
        //--======================================>>> rptSefaresh <<< ============================================
        oSGV.Grids["rptSefaresh"] = new Grid()
        {
            ContainerId = "MyGridId",
            Options = { Paging = false, Order = "[[6,'desc']]" },
            ContainerHeight = 400,
            Columns = new List<Column>() {
                new Column {Title ="مرکز انبار", Data="NameMarkaz"},
                new Column {Title ="نوع سفارش", Data="NoeSefaresh"},
                new Column {Title ="کد تامین کننده", Data="CodeTaminKonandeh"},
                new Column {Title ="نام تامین کننده", Data="NameTaminKonandeh"},
                new Column {Title ="شماره سفارش", Data="ShomarehSefaresh",
                    Functions = new List<Function>() {
                        new OnClick { Section = OnClick.SectionValue.Tbody , NextGrid = "rptSefareshKala", Level = "3" , DataKeys ={ "ccSefaresh"}, CssClass = "btn" },
                        new OnClick { Section = OnClick.SectionValue.Tfoot , NextGrid="rptMajmooeSefareshat" , Level="2", FooterText="مجموع سفارشات کالا"},
                        new TextFeature { Condition = "1==1", Section = Function.SectionValue.Tbody , IsTrueCssClass=" text-primary SAPToolTip" , IsTrueText="<span title='جزییات سفارش جاری'>ShomarehSefaresh</span>" }
                    }
                },
                new Column{Title ="تاریخ سفارش", Data="TarikhVazeiat",
                    Functions = new List<Function>() {
                        new MiladiToJalali{Output=MiladiToJalali.DateValue.DateOnly },
                        //new TextFeature { Condition = "1==1", IsTrueText = "-", IsFalseText = "-", IsFalseCssClass= "", IsTrueCssClass = "", Section = Function.SectionValue.Tfoot }
                    } 
                },
                new Column{Title ="تعداد در سفارش", Data ="MizanSefaresh",
                    Functions = new List<Function>() {
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new TextFeature { Condition = "1==1", IsTrueText = "MizanSefaresh", IsFalseText = "-", IsFalseCssClass= "", IsTrueCssClass = "", Section = Function.SectionValue.Tfoot },
                        new Separator { DecimalPlaces = 0 }} },
                new Column { Title ="تعداد در پیش فاکتور", Data="MizanPishFactor",
                    Functions = new List<Function>() {
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new TextFeature { Condition = "1==1", IsTrueText = "MizanPishFactor", IsFalseText = "-" , IsFalseCssClass= "", IsTrueCssClass = "", Section = Function.SectionValue.Tfoot},
                        new Separator { DecimalPlaces = 0},
                        new OnClick { Section = OnClick.SectionValue.Tbody , NextGrid = "rptMizanPishFactorSefaresh", Level = "6" , DataKeys ={ "ccSefaresh"} , CssClass = "btn"  },
                        new TextFeature { Condition = "1==1", Section = Function.SectionValue.Tbody , IsTrueCssClass=" text-primary SAPToolTip" , IsTrueText="<span title='پیش فاکتورهای سفارش جاری'>MizanPishFactor</span>"}
                    }
                },
                new Column{Title ="تعداد در وارده", Data ="Varede",
                    Functions = new List<Function>() {
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new Separator { DecimalPlaces = 0 },
                        new OnClick { Section = OnClick.SectionValue.Tbody , NextGrid = "rptMizanVaredeSefaresh", Level = "7" ,DataKeys ={ "ccSefaresh"}, CssClass = "btn"  },
                        new TextFeature { 
                            Section = Function.SectionValue.Tbody ,
                            Condition = "1==1",  
                            IsTrueCssClass=" text-primary SAPToolTip" , 
                            IsTrueText="<span title='وارده های سفارش جاری'>Varede</span>"
                        },
                        new Separator { Section = Function.SectionValue.Tfoot, DecimalPlaces = 0 },
                        new TextFeature {
                            Section = Function.SectionValue.Tfoot,
                            Condition = "1 == 1",
                            IsTrueCssClass ="text-info"
                        },
                        new TextFeature {
                            Section = Function.SectionValue.Tfoot,
                            Condition = "1 == 1",
                            IsTrueCssClass ="text-dark"
                        }
                    }
                },
                new Column{Title ="مغایرت بین پیش فاکتور و سفارش", Data ="MoghayeratSP", DefaultContent = "", CssClass = "ltr",
                    Functions = new List<Function>() {
                        new Calc { Section = Calc.SectionValue.Tbody, Formula = "MizanPishFactor - MizanSefaresh"},
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new Separator { Section = Function.SectionValue.Tbody, DecimalPlaces = 0 },
                        new Separator { Section = Function.SectionValue.Tfoot, DecimalPlaces = 0 },
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "MoghayeratSP < 0",
                            IsTrueCssClass ="text-danger",
                            IsFalseCssClass ="",
                            IsTrueText ="(MoghayeratSP)",
                            IsFalseText= "MoghayeratSP",
                            StrReplace = { {"-","" } }
                        },
                        new TextFeature {
                            Section = Function.SectionValue.Tfoot,
                            Condition = "MoghayeratSP < 0",
                            IsTrueCssClass ="text-danger",
                            IsFalseCssClass ="",
                            IsTrueText ="(MoghayeratSP)",
                            IsFalseText= "MoghayeratSP",
                            StrReplace = { {"-","" } }
                        }
                    }
                },
                new Column{Title ="مغایرت بین وارده و سفارش", Data ="MoghayeratSV", DefaultContent = "",
                    Functions = new List<Function>() {
                        new Calc { Section = Calc.SectionValue.Tbody, Formula = "Varede - MizanSefaresh"},
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new TextFeature { Condition = "1==1", IsTrueText = "MoghayeratSV", IsFalseText = "-", IsFalseCssClass= "", IsTrueCssClass = "", Section = Function.SectionValue.Tfoot },
                        new Separator { DecimalPlaces = 0 },
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "MoghayeratSV < 0",
                            IsTrueCssClass ="text-danger",
                            IsFalseCssClass ="",
                            IsTrueText ="(MoghayeratSV)",
                            IsFalseText= "MoghayeratSV",
                            StrReplace = { {"-","" } }
                        },
                        new TextFeature {
                            Section = Function.SectionValue.Tfoot,
                            Condition = "1 == 1",
                            IsTrueCssClass ="text-warning"
                        }
                    }
                },
                new Column{Title ="مغایرت بین وارده و پیش فاکتور", Data ="MoghayeratPV", DefaultContent = "",
                    Functions = new List<Function>() {
                        new Calc { Section = Calc.SectionValue.Tbody, Formula = "Varede - MizanPishFactor"},
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new TextFeature { Condition = "1==1", IsTrueText = "MoghayeratPV", IsFalseText = "-", IsFalseCssClass= "", IsTrueCssClass = "", Section = Function.SectionValue.Tfoot },
                        new Separator { DecimalPlaces = 0 },
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "MoghayeratPV < 0",
                            IsTrueCssClass ="text-danger",
                            IsFalseCssClass ="",
                            IsTrueText ="(MoghayeratPV)",
                            IsFalseText= "MoghayeratPV",
                            StrReplace = { {"-","" } }
                        },
                        new TextFeature {
                            Section = Function.SectionValue.Tfoot,
                            Condition = "1 == 1",
                            IsTrueCssClass ="text-primary"
                        }
                    }
                }
            }
        };
        //--======================================>>> rptMajmooeSefareshat <<< ===================================
        oSGV.Grids["rptMajmooeSefareshat"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 400,
            Options = { Order = "[[1,'asc']]" },
            Columns = new List<Column>() {
                new Column {Title ="کد کالا",Data="CodeKala"},
                new Column {Title ="نام کالا",Data="NameKala",
                  Functions = new List<Function>()
                    {
                    new OnClick { Section = OnClick.SectionValue.Tbody , NextGrid = "rptSefareshKala", Level = "3" ,DataKeys ={ "ccKalaCode" }, CssClass = "btn"},
                    new TextFeature { Condition = "1==1", Section = Function.SectionValue.Tbody , IsTrueCssClass="text-primary SAPToolTip" , IsTrueText="<span title='سفارشات کالا'>NameKala</span>" }
                    }
                },
                new Column {Title ="واحد شمارش",Data="NameVahedShomaresh"},
                new Column {Title ="تعداد در سفارش",Data="MizanSefaresh" ,
                Functions = {
                        new Separator { DecimalPlaces = 0 }} },
                new Column {Title ="تعداد در پیش فاکتور",Data="MizanPishFactor" ,
                Functions = new List<Function>()
                {
                    new Separator { DecimalPlaces = 0 },
                    new OnClick { Section = OnClick.SectionValue.Tbody , NextGrid = "rptMizanPishFactor", Level = "4" ,DataKeys ={"ccKalaCode"}, CssClass = "btn" },
                    new TextFeature { Condition = "1==1", Section = Function.SectionValue.Tbody , IsTrueCssClass="text-primary SAPToolTip" , IsTrueText="<span title='پیش فاکتورهای کالای جاری'>MizanPishFactor</span>"}
                } },
                new Column {Title ="تعداد در وارده",Data="Varede" ,
                 Functions = new List<Function>()
                    {
                      new Separator { DecimalPlaces = 0 },
                      new OnClick { Section = OnClick.SectionValue.Tbody , NextGrid = "rptMizanVarede", Level = "5" ,DataKeys ={"ccKalaCode"}, CssClass = "btn" },
                      new TextFeature { Condition = "1==1", Section = Function.SectionValue.Tbody , IsTrueCssClass="text-primary SAPToolTip" , IsTrueText="<span title='وارده های کالای جاری'>Varede</span>" }
                    } },
                new Column{Title ="مغایرت بین پیش فاکتور و سفارش", Data ="MoghayeratSP", DefaultContent = "",
                    Functions = new List<Function>() {
                        new Calc { Section = Calc.SectionValue.Tbody, Formula = "MizanPishFactor - MizanSefaresh"},
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new TextFeature { Condition = "1==1", IsTrueText = "MoghayeratSP", IsFalseText = "-", IsFalseCssClass= "", IsTrueCssClass = "", Section = Function.SectionValue.Tfoot },
                        new Separator { DecimalPlaces = 0 },
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "MoghayeratSP < 0",
                            IsTrueCssClass ="text-danger",
                            IsFalseCssClass ="",
                            IsTrueText ="(MoghayeratSP)",
                            IsFalseText= "MoghayeratSP",
                            StrReplace = { {"-","" } }
                        }
                    }
                },
                new Column{Title ="مغایرت بین وارده و سفارش", Data ="MoghayeratSV", DefaultContent = "",
                    Functions = new List<Function>() {
                        new Calc { Section = Calc.SectionValue.Tbody, Formula = "Varede - MizanSefaresh"},
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new TextFeature { Condition = "1==1", IsTrueText = "MoghayeratSV", IsFalseText = "-", IsFalseCssClass= "", IsTrueCssClass = "", Section = Function.SectionValue.Tfoot },
                        new Separator { DecimalPlaces = 0 },
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "MoghayeratSV < 0",
                            IsTrueCssClass ="text-danger",
                            IsFalseCssClass ="",
                            IsTrueText ="(MoghayeratSV)",
                            IsFalseText= "MoghayeratSV",
                            StrReplace = { {"-","" } }
                        }
                    }
                },
                new Column{Title ="مغایرت بین وارده و پیش فاکتور", Data ="MoghayeratPV", DefaultContent = "",
                    Functions = new List<Function>() {
                        new Calc { Section = Calc.SectionValue.Tbody, Formula = "Varede - MizanPishFactor"},
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new TextFeature { Condition = "1==1", IsTrueText = "MoghayeratPV", IsFalseText = "-", IsFalseCssClass= "", IsTrueCssClass = "", Section = Function.SectionValue.Tfoot },
                        new Separator { DecimalPlaces = 0 },
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "MoghayeratPV < 0",
                            IsTrueCssClass ="text-danger",
                            IsFalseCssClass ="",
                            IsTrueText ="(MoghayeratPV)",
                            IsFalseText= "MoghayeratPV",
                            StrReplace = { {"-","" } }
                        }
                    }
                }
             }
        };
        //--======================================>>> rptSefareshKala <<< ========================================
        oSGV.Grids["rptSefareshKala"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 400,
            Options = { Order = "[[2,'desc']]" },
            Columns = new List<Column>() {
                new Column {Title ="شماره سفارش", Data="ShomarehSefaresh"},
                new Column {Title ="تاریخ سفارش", Data="TarikhVazeiat" ,
                    Functions = {new  MiladiToJalali{Output=MiladiToJalali.DateValue.DateOnly },
                    new TextFeature { Condition = "1==1", IsTrueText = "-", IsFalseText = "-", IsFalseCssClass= "", IsTrueCssClass = "", Section = Function.SectionValue.Tfoot }}},
                new Column {Title ="کد کالا",Data="CodeKala"},
                new Column {Title ="نام کالا",Data="NameKala"},
                new Column {Title ="واحد شمارش",Data="NameVahedShomaresh"},
                new Column {Title ="تعداد در سفارش",Data="MizanSefaresh" ,
                Functions = {
                        new Separator { DecimalPlaces = 0 }} },
                new Column {Title ="تعداد در پیش فاکتور",Data="MizanPishFactor" ,
                Functions = new List<Function>()
                    {
                      new Separator { DecimalPlaces = 0 },
                    new OnClick { Section = OnClick.SectionValue.Tbody , NextGrid = "rptMizanPishFactorSefaresh", Level = "6" ,DataKeys ={ "ccSefaresh","ccKalaCode"}, CssClass = "btn" },
                    new TextFeature { Condition = "1==1", Section = Function.SectionValue.Tbody , IsTrueCssClass="text-primary SAPToolTip" , IsTrueText="<span title='پیش فاکتورهای کالا درسفارش جاری'>MizanPishFactor</span>"}
                    } },
                new Column {Title ="تعداد در وارده",Data="Varede" ,
                 Functions = new List<Function>()
                    {
                      new Separator { DecimalPlaces = 0 },
                      new OnClick { Section = OnClick.SectionValue.Tbody , NextGrid = "rptMizanVaredeSefaresh", Level = "7" ,DataKeys ={ "ccSefaresh","ccKalaCode"}, CssClass = "btn" },
                      new TextFeature { Condition = "1==1", Section = Function.SectionValue.Tbody , IsTrueCssClass="text-primary SAPToolTip" , IsTrueText="<span title='وارده های کالا درسفارش جاری'>Varede</span>" }
                    } },
                new Column{Title ="مغایرت بین پیش فاکتور و سفارش", Data ="MoghayeratSP", DefaultContent = "",
                    Functions = new List<Function>() {
                        new Calc { Section = Calc.SectionValue.Tbody, Formula = "MizanPishFactor - MizanSefaresh"},
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new TextFeature { Condition = "1==1", IsTrueText = "MoghayeratSP", IsFalseText = "-", IsFalseCssClass= "", IsTrueCssClass = "", Section = Function.SectionValue.Tfoot },
                        new Separator { DecimalPlaces = 0 },
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "MoghayeratSP < 0",
                            IsTrueCssClass ="text-danger",
                            IsFalseCssClass ="",
                            IsTrueText ="(MoghayeratSP)",
                            IsFalseText= "MoghayeratSP",
                            StrReplace = { {"-","" } }
                        }
                    }
                },
                new Column{Title ="مغایرت بین وارده و سفارش", Data ="MoghayeratSV", DefaultContent = "",
                    Functions = new List<Function>() {
                        new Calc { Section = Calc.SectionValue.Tbody, Formula = "Varede - MizanSefaresh"},
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new TextFeature { Condition = "1==1", IsTrueText = "MoghayeratSV", IsFalseText = "-", IsFalseCssClass= "", IsTrueCssClass = "", Section = Function.SectionValue.Tfoot },
                        new Separator { DecimalPlaces = 0 },
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "MoghayeratSV < 0",
                            IsTrueCssClass ="text-danger",
                            IsFalseCssClass ="",
                            IsTrueText ="(MoghayeratSV)",
                            IsFalseText= "MoghayeratSV",
                            StrReplace = { {"-","" } }
                        }
                    }
                },
                new Column{Title ="مغایرت بین وارده و پیش فاکتور", Data ="MoghayeratPV", DefaultContent = "",
                    Functions = new List<Function>() {
                        new Calc { Section = Calc.SectionValue.Tbody, Formula = "Varede - MizanPishFactor"},
                        new Calc { Section = Function.SectionValue.Tfoot, Operator = Calc.OperatorValue.VerticalSum },
                        new TextFeature { Condition = "1==1", IsTrueText = "MoghayeratPV", IsFalseText = "-", IsFalseCssClass= "", IsTrueCssClass = "", Section = Function.SectionValue.Tfoot },
                        new Separator { DecimalPlaces = 0 },
                        new TextFeature {
                            Section = Function.SectionValue.Tbody,
                            Condition = "MoghayeratPV < 0",
                            IsTrueCssClass ="text-danger",
                            IsFalseCssClass ="",
                            IsTrueText ="(MoghayeratPV)",
                            IsFalseText= "MoghayeratPV",
                            StrReplace = { {"-","" } }
                        }
                    }
                }
             }
        };
        //--======================================>>> rptMizanPishFactor <<< ===============================
        oSGV.Grids["rptMizanPishFactor"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 400,
            Options = { Order = "[[2,'desc']]" },
            Columns = new List<Column>() {
                new Column {Title ="شماره سفارش", Data="ShomarehSefaresh" },
                new Column {Title ="شماره پیش فاکتور", Data="ShomarehPishFaktor"},
                new Column{Title ="تاریخ پیش فاکتور", Data="TarikhVazeiat",
                    Functions = {new  MiladiToJalali{Output=MiladiToJalali.DateValue.DateOnly },
                    new TextFeature { Condition = "1==1", IsTrueText = "-", IsFalseText = "-", IsFalseCssClass= "", IsTrueCssClass = "", Section = Function.SectionValue.Tfoot }}},
                new Column {Title ="کد تامین کننده", Data="CodeTaminKonandeh"},
                new Column {Title ="نام تامین کننده", Data="NameTaminKonandeh"},
                new Column {Title ="کد کالا",Data="CodeKala"},
                new Column {Title ="نام کالا",Data="NameKala"},
                new Column {Title ="واحد شمارش",Data="NameVahedShomaresh"},
                new Column {Title ="تعداد در پیش فاکتور",Data="MizanPishFactor" ,
                  Functions = {
                        new Separator { DecimalPlaces = 0 }} }
             }
        };
        //--======================================>>> rptMizanVarede <<< ===============================
        oSGV.Grids["rptMizanVarede"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 400,
            Options = { Order = "[[2,'desc']]" },
            Columns = new List<Column>() {
                new Column {Title ="شماره سفارش", Data="ShomarehSefaresh" },
                new Column {Title ="شماره فرم", Data="ShomarehForm"},
                new Column{Title ="تاریخ فرم", Data="TarikhForm",
                    Functions = {new  MiladiToJalali{Output=MiladiToJalali.DateValue.DateOnly }}},
                new Column {Title ="کد تامین کننده", Data="CodeTaminKonandeh"},
                new Column {Title ="نام تامین کننده", Data="NameTaminKonandeh"},
                new Column {Title ="کد کالا",Data="CodeKala"},
                new Column {Title ="نام کالا",Data="NameKala"},
                new Column {Title ="واحد شمارش",Data="NameVahedShomaresh"},
                new Column{Title ="تعداد در وارده", Data ="Varede",
                 Functions = {
                        new Separator { DecimalPlaces = 0 }} }
             }
        };
        //--======================================>>> rptMizanPishFactorSefaresh <<< ===============================
        oSGV.Grids["rptMizanPishFactorSefaresh"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 400,
            Options = { Order = "[[2,'desc']]" },
            Columns = new List<Column>() {
                new Column {Title ="شماره سفارش", Data="ShomarehSefaresh" },
                new Column {Title ="شماره پیش فاکتور", Data="ShomarehPishFaktor"},
                new Column{Title ="تاریخ پیش فاکتور", Data="TarikhVazeiat",
                    Functions = {new  MiladiToJalali{Output=MiladiToJalali.DateValue.DateOnly },
                    new TextFeature { Condition = "1==1", IsTrueText = "-", IsFalseText = "-", IsFalseCssClass= "", IsTrueCssClass = "", Section = Function.SectionValue.Tfoot }}},
                new Column {Title ="کد تامین کننده", Data="CodeTaminKonandeh"},
                new Column {Title ="نام تامین کننده", Data="NameTaminKonandeh"},
                new Column {Title ="کد کالا",Data="CodeKala"},
                new Column {Title ="نام کالا",Data="NameKala"},
                new Column {Title ="واحد شمارش",Data="NameVahedShomaresh"},
                new Column {Title ="تعداد در پیش فاکتور",Data="MizanPishFactor" ,
                  Functions = {
                        new Separator { DecimalPlaces = 0 }} },
             }
        };
        //--======================================>>> rptMizanVaredeSefaresh <<< ===============================
        oSGV.Grids["rptMizanVaredeSefaresh"] = new Grid()
        {
            ContainerId = "MyGridId",
            ContainerHeight = 400,
            Options = { Order = "[[2,'desc']]" },
            Columns = new List<Column>() {
                new Column {Title ="شماره سفارش", Data="ShomarehSefaresh"},
                new Column {Title ="شماره فرم", Data="ShomarehForm"},
                new Column{Title ="تاریخ فرم", Data="TarikhForm",
                    Functions = {new  MiladiToJalali{Output=MiladiToJalali.DateValue.DateOnly }}},
                new Column {Title ="کد تامین کننده", Data="CodeTaminKonandeh"},
                new Column {Title ="نام تامین کننده", Data="NameTaminKonandeh"},
                new Column {Title ="کد کالا",Data="CodeKala"},
                new Column {Title ="نام کالا",Data="NameKala"},
                new Column {Title ="واحد شمارش",Data="NameVahedShomaresh"},
                new Column{Title ="تعداد در وارده", Data ="Varede",
                 Functions = {
                        new Separator { DecimalPlaces = 0 }} }
             }
        };
    }
    #endregion

    [WebMethod]
    public static string SapGridEvent(string CallBackData)
    {
        SapGridCallBackEvent oData = JsonConvert.DeserializeObject<SapGridCallBackEvent>(CallBackData);
        List<string> DataKeys = oData.FuncArray.DataKeys;
        string NextGrid = oData.FuncArray.NextGrid;
        oSGV.Grids[NextGrid].GridParameters = new Dictionary<string, string>();
        foreach (KeyValuePair<string, string> item in oSGV.DefaultParameters)
        {
            oSGV.Grids[NextGrid].GridParameters[item.Key] = item.Value;
        }

        Dictionary<string, string> Clicked_GridParameters = oData.GridParameters;
        oSGV.Grids[NextGrid].GridParameters["Level"] = oData.FuncArray.Level;
        var RowData = oData.RowData;
        foreach (string DataKey in DataKeys)
        {
            if (RowData.Count != 0)
                oSGV.Grids[NextGrid].GridParameters[DataKey] = RowData[DataKey];
            else
            {
                oSGV.Grids[NextGrid].GridParameters[DataKey] = Clicked_GridParameters[DataKey];
            }
        }
        oSGV.Grids[NextGrid].Data = new Sefaresh().rpt_Sefaresh(oSGV.Grids[NextGrid].GridParameters);
        return oSGV.AjaxBind(NextGrid);

    }
}