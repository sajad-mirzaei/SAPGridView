using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Services;

public partial class Grid_AjaxPagination_Page : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod]
    public static string TestMethod(string CallBackData)
    {
        var oData = JsonConvert.DeserializeObject<AjaxPaginationProperty1>(CallBackData);
        ArrayTest1 oArrayTest = new ArrayTest1();
        for (int i = oData.start; i < (oData.length + oData.start); i++)
        {
            oArrayTest.data.Add(
                new Dictionary<string, string>() {
                    { "first_name", "FirstName " + i },
                    { "last_name", "LastName " + i },
                    { "position", "Position " + i },
                    { "office", "Office " + i },
                    { "start_date", "1st Jan 70" },
                    { "salary", "$40000" + i }
                }
                );
        }
        var JsonData = JsonConvert.SerializeObject(oArrayTest);
        return JsonData;
    }
}

class ArrayTest1
{
    public int draw = 0;
    public int recordsTotal = 56;
    public int recordsFiltered = 56;
    public List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
}
class AjaxPaginationProperty1
{
    public int draw;
    public int length;
    public int start;
    public Dictionary<string, string> search;
    public List<Dictionary<string, string>> order;
    /*public string columns;*/
}