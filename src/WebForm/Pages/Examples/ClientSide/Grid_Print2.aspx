<%@ Page Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="Grid_Print2.aspx.cs" Inherits="Grid_Print2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <ajax:UpdatePanel runat="server" ID="upForm" RenderMode="block" UpdateMode="Always">
        <ContentTemplate>
            <ajax:UpdateProgress class="loading" ID="UpdateProgressNoeLevelEtebarMoshtary" runat="server">
                <ProgressTemplate>
                    <asp:Label ID="Label001" Text="" runat="server"></asp:Label>
                    <img alt="لطفا چند لحظه صبر کنید..." src="../../../Assets/Styles/images/wait.gif" />
                </ProgressTemplate>
            </ajax:UpdateProgress>

            <div class="FormBox">
                <div class="HeaderBox">
                    Test1 <i class='fa fa-solid fa-filter-circle-xmark'></i>
                </div>
                <div class="ContentBox">
                    <div id="MyGridId"></div>
                </div>
            </div>
            <script>
                function MyPrintButtonMethod(oData) {
                    var orientation = {
                        portrait: "200mm",
                        landscape: "290mm"
                    };
                    var printWith = orientation.portrait;
                    var btnOptions = {
                        extend: 'print',
                        titleAttr: "پرینت11",
                        autoPrint: false,
                        text: '<i class="fa fa-print DataTableIcons"></i>',
                        footer: true,
                        title: "",
                        //customize main data grid
                        customize: function (win, obj, options) {
                            var dataGrid = $(win.document.body).find('.Grid');
                            dataGrid.attr("class", "").css("width", printWith).css('font-size', 'inherit')
                            dataGrid.find("thead").css("background-color", "#dddddd !important");
                            dataGrid.find("tbody").find("tr:odd").css("background-color", "#eeeeee !important");
                        },
                        //customize report info
                        messageTop: function () {
                            var str = "<style>";
                            str += "@page { size: A4; margin: 5mm; width: " + printWith + "; }";
                            str += "@media print { html, body { width: " + printWith + "; height: 290mm; } }";
                            str += "td, th { border: 1px solid #dddddd; padding: 5px; }";
                            str += ".headerTable { width: calc(100% - 116px) !important; height: 3cm; border: 1px solid #dddddd; float: right; }";
                            str += ".mainPage { width: " + printWith + " !important; position: relative; }";
                            str += ".logoImg { height: 111px; width: 114px; }";
                            str += "</style>";
                            str += '<div class="mainPage">';
                            str += '<table class="headerTable">';
                            str += '<tr><td colspan="2" class="text-center">' + oData.YourData.sherkatName + '</td></tr>';
                            str += '<tr><td> گزارش گیرنده: ' + oData.YourData.user + '</td> <td> نام گزارش: ' + oData.YourData.reportName + '</td></tr>';
                            str += '<tr><td> از تاریخ: ' + oData.YourData.fromDate + '</td> <td> تا تاریخ: ' + oData.YourData.toDate + '</td></tr>';
                            str += '</table>';
                            str += '<div style="float:left; border: 1px solid #dddddd;"><img class="logoImg" src="' + oData.YourData.logo + '" /></div>';
                            str += '</div>';
                            return str;

                        }
                    };
                    return btnOptions;
                }
            </script>

        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
