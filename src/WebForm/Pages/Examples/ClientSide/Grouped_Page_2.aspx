<%@ Page Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="Grouped_Page_2.aspx.cs" Inherits="Grid_Grouped_Page_2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <ajax:UpdatePanel runat="server" ID="upForm" RenderMode="block" UpdateMode="Always">
        <ContentTemplate>
            <ajax:UpdateProgress class="loading" ID="UpdateProgressNoeLevelEtebarMoshtary" runat="server">
                <ProgressTemplate>
                    <asp:Label ID="Label001" Text="" runat="server"></asp:Label>
                    <img alt="لطفا چند لحظه صبر کنید..." src="../../../Assets/Styles/images/wait.gif" />
                </ProgressTemplate>
            </ajax:UpdateProgress>
            <style>
                tr.group,
                tr.group:hover {
                    background-color: #ddd !important;
                }
            </style>
            <div id="divData1" runat="server" class="FormBox">
                <div class="HeaderBox">aaaa</div>
                <div id="ContentBoxId1" class="ContentBox MyDTContainer">
                    <div id="MyGridId"></div>

                    <asp:GridView ID="aaa" gridheight="350" runat="server" AllowSorting="true" AutoGenerateColumns="false"
                        GridLines="both" CssClass="Grid" DataKeyNames="id">
                        <AlternatingRowStyle CssClass="GridAlternateRow" />
                        <SelectedRowStyle CssClass="GridSelectedRow text-center" />
                        <RowStyle CssClass="GridRow" />
                        <HeaderStyle CssClass="GridRowHeader" />
                        <PagerStyle CssClass="GridPager" />
                        <Columns>
                            <asp:TemplateField HeaderText="aa">
                                <ItemTemplate>
                                    <%#Eval("first_name") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="bb">
                                <ItemTemplate>
                                    <%#Eval("last_name") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="cc">
                                <ItemTemplate>
                                    <%#Eval("position") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="dd">
                                <ItemTemplate>
                                    <%#Eval("office") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="ee">
                                <ItemTemplate>
                                    <%#Eval("salary") %>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </div>
                <script>
                    function onDRPagesLoad() {
                        myCustomDataTable(".DTGridContainer .Grid", {});
                        $(document).ready(function () {
                            var groupColumn = 2;
                            var table = $('#cphMain_aaa11').DataTable({
                                "columnDefs": [
                                    { "visible": false, "targets": groupColumn }
                                ],
                                "order": [[groupColumn, 'asc']],
                                "displayLength": 25,
                                "drawCallback": function (settings) {
                                    var api = this.api();
                                    var rows = api.rows({ page: 'current' }).nodes();
                                    var last = null;

                                    api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                                        if (last !== group) {
                                            $(rows).eq(i).before(
                                                '<tr class="group"><td colspan="5">' + group + '</td></tr>'
                                            );

                                            last = group;
                                        }
                                    });
                                }
                            });

                            // Order by the grouping
                            $('#cphMain_aaa11 tbody').on('click', 'tr.group', function () {
                                var currentOrder = table.order()[0];
                                if (currentOrder[0] === groupColumn && currentOrder[1] === 'asc') {
                                    table.order([groupColumn, 'desc']).draw();
                                }
                                else {
                                    table.order([groupColumn, 'asc']).draw();
                                }
                            });
                        });
                    }
                </script>
            </div>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
