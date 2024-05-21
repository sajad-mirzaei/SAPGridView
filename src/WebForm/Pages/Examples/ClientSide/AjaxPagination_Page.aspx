<%@ Page Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="AjaxPagination_Page.aspx.cs" Inherits="AjaxPagination_Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <ajax:UpdatePanel runat="server" ID="upForm" RenderMode="block" UpdateMode="Always">
        <ContentTemplate>
            <ajax:UpdateProgress class="loading" ID="UpdateProgressNoeLevelEtebarMoshtary" runat="server">
                <ProgressTemplate>
                    <asp:Label ID="Label001" Text="" runat="server"></asp:Label>
                    <img alt="لطفا چند لحظه صبر کنید..." src="../../../Assets/Styles/images/wait.gif" />
                </ProgressTemplate>
            </ajax:UpdateProgress>
            <div id="divData2" runat="server" class="FormBox">
                <div class="HeaderBox">aaaa</div>
                <div id="ContentBoxId2" class="ContentBox">
                    <table id="example" class="display" style="width: 100%">
                        <thead>
                            <tr>
                                <th>First name</th>
                                <th>Last name</th>
                                <th>Position</th>
                                <th>Office</th>
                                <th>Start date</th>
                                <th>Salary</th>
                            </tr>
                        </thead>
                        <tfoot>
                            <tr>
                                <th>First name</th>
                                <th>Last name</th>
                                <th>Position</th>
                                <th>Office</th>
                                <th>Start date</th>
                                <th>Salary</th>
                            </tr>
                        </tfoot>
                    </table>

                </div>
            </div>
            <script>
                function onPagesLoad() {
                    $("#example").DataTable({
                        "processing": true,
                        "serverSide": true,
                        "columns": [
                            { "data": "first_name" },
                            { "data": "last_name" },
                            { "data": "position" },
                            { "data": "office" },
                            { "data": "start_date" },
                            { "data": "salary" }
                        ],
                        "ajax": {
                            "type": "POST",
                            "contentType": "application/json; charset=utf-8",
                            "url": document.location.origin + document.location.pathname + "/TestMethod",
                            "data": function (d) {
                                return "{CallBackData:'" + JSON.stringify(d) + "'}";
                            },
                            "dataType": "text",
                            "dataSrc": function (data) {
                                if (Array.isArray(data) !== true) {
                                    data = JSON.parse(data);
                                    if (Array.isArray(data.d) !== true)
                                        data.d = JSON.parse(data.d);
                                }
                                return data.d.data;
                            },
                            "cache": false
                        }
                    });
                }
            </script>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
