<%@ Page Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="Grid_Print1.aspx.cs" Inherits="Grid_Print1" %>

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
                    var btnOptions = {
                        extend: 'print',
                        titleAttr: "پرینت11",
                        autoPrint: false,
                        text: '<i class="fa fa-print DataTableIcons"></i>',
                        footer: true,
                        title: "",
                        customize: function (win, obj, options) {
                            $(win.document.body)
                                .css('font-size', '10pt')
                                .prepend(
                                    '<img src="' + oData.YourData.ImageAddress + '" style="top:0; left:0;" />'
                                );

                            $(win.document.body).find('table')
                                .addClass('compact')
                                .addClass('rtl')
                                .addClass('table table-striped')
                                .css('font-size', 'inherit');
                        },
                        messageTop: function () {
                            var str = '<div class="container mt-3">';
                            str += '<div class="d-flex justify-content-center bg-secondary mb-3">';
                            str += '<div class="p-2 bg-info">' + oData.YourData.SherkatName + '</div>';
                            str += '<div class="p-2 bg-warning">' + oData.YourData.User + ' </div>';
                            str += '<div class="p-2 bg-primary">متن تست 1</div>';
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
