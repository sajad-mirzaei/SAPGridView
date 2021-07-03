<%@ Page Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="GridTestPage.aspx.cs" Inherits="GridTestPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <ajax:UpdatePanel runat="server" ID="upForm" RenderMode="block" UpdateMode="Always">
        <ContentTemplate>
            <ajax:UpdateProgress class="loading" ID="UpdateProgressNoeLevelEtebarMoshtary" runat="server">
                <ProgressTemplate>
                    <asp:Label ID="Label001" Text="" runat="server"></asp:Label>
                    <img alt="لطفا چند لحظه صبر کنید..." src="../../../Assets/Styles/images/wait.gif" />
                </ProgressTemplate>
            </ajax:UpdateProgress>
            <div id="divDataEntry" runat="server" class="FormBox">
                <div class="HeaderBox">
                    <asp:Label class="label" ID="lblNameSystem" runat="server"></asp:Label>
                </div>
                <div id="dContentBox" class="ContentBox">
                    <asp:Label ID="lblErrorBox" Visible="false" runat="server"></asp:Label>
                    <div class="row">
                        <div class="form-group col-lg-2 col-md-2 col-sm-6 col-xs-12">
                            <span class="text-danger">*</span>
                            <asp:Label ID="lblAzTarikh" label-for="dtbAzTarikh" class="label" runat="server" Text="از تاریخ :"></asp:Label>
                            <SAP:DateBox ID="dtbAzTarikh" class="DateBox" runat="Server" />
                        </div>
                        <div class="form-group col-lg-2 col-md-2 col-sm-6 col-xs-12">
                            <span class="text-danger">*</span>
                            <asp:Label ID="lblTaTarikh" label-for="dtbTaTarikh" class="label" runat="server" Text="تا تاریخ :"></asp:Label>
                            <SAP:DateBox ID="dtbTaTarikh" class="DateBox" runat="Server" />
                        </div>
                        <div class="form-group col-lg-2 col-md-4 col-sm-6 col-12 ">
                            <span class="text-danger">*</span>
                            <asp:Label ID="lblMarkaz" label-for="ddlMarkaz" class="label" Text="نام مرکز:" runat="server"></asp:Label>
                            <asp:ListBox runat="server" ID="ddlMarkaz" Class="form-control selectpicker" data-selected-text-format="count" data-actions-box="true" data-size='7' title="انتخاب کنید" SelectionMode="Multiple"></asp:ListBox>
                        </div>
                        <div class="form-group col-lg-2 col-md-4 col-sm-6 col-12 ">
                            <span class="text-danger">*</span>
                            <asp:Label ID="lblNoeSefaresh" label-for="ddlNoeSefaresh" class="label" Text="نوع سفارش:" runat="server"></asp:Label>
                            <asp:ListBox class='form-control selectpicker' data-actions-box="true" data-size='7' runat='server' ID='ddlNoeSefaresh' title="انتخاب کنید" SelectionMode="Multiple">
                                <asp:ListItem Text="اضطراری" Value="0"></asp:ListItem>
                                <asp:ListItem Text="اتوماتیک" Value="1"></asp:ListItem>
                            </asp:ListBox>
                        </div>
                        <div class="form-group col-lg-2 col-md-4 col-sm-6 col-12">
                            <span class="text-danger">*</span>
                            <asp:Label class="lblNoeSys" label-for="ddlCodeNoeSys" ID="lblCodeNoeSys" Text="نوع  کالا :" runat="server"></asp:Label>
                            <asp:ListBox class='form-control selectpicker' data-actions-box="true" data-size='7' ID="ddlCodeNoeSys" runat="server" DataTextField="NameNoeSys"
                                DataValueField="CodeNoeSys" DataSourceID="odsCodeNoeSys" title="انتخاب همه" data-selected-text-format="count" SelectionMode="Multiple"></asp:ListBox>
                        </div>
                        <div class="form-group col-lg-1 col-md-2 col-sm-6 col-12 noLabelBottom ">
                            <asp:LinkButton ID="btnKalaSearch" CssClass="Button" Text="جستجوی کالا و تامین کننده" OnClick="btnKalaSearch_Click" runat="server"></asp:LinkButton>
                        </div>
                        <div class="form-group col-lg-2 col-md-4 col-sm-6 col-12 ">
                            <asp:Label ID="lblKala" label-for="ddlKala" class="label" Text="کالا:" runat="server"></asp:Label>
                            <asp:ListBox class='form-control selectpicker' data-live-search="true" data-selected-text-format="count" data-actions-box="true" data-size='7' runat='server' ID='ddlKala' title="انتخاب کنید" SelectionMode="Multiple"></asp:ListBox>
                        </div>
                        <div class="form-group col-lg-2 col-md-4 col-sm-6 col-12 ">
                            <asp:Label ID="lblTaminKonandeh" label-for="ddlTaminKonandeh" class="label" Text="تامین کننده:" runat="server"></asp:Label>
                            <asp:ListBox class='form-control selectpicker' data-live-search="true" AppendDataBoundItems="true" data-selected-text-format="count" data-actions-box="true" data-size='7' runat='server' ID='ddlTaminKonandeh' title="انتخاب کنید" SelectionMode="Multiple"></asp:ListBox>
                        </div>
                        <div class="form-group col-lg-2 col-md-2 col-sm-6 col-12 noLabelBottom">
                            <asp:LinkButton ID="btnView" Text="نمایش" CssClass="Button" OnClick="btnView_Click"
                                runat="server"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
            <div id="divGrid" runat="server" class="FormBox">
                <div class="HeaderBox">
                    <asp:Label ID="lblHeaderGrid" Text="لیست سفارشات" runat="server"></asp:Label>
                </div>
                <div class="ContentBox">
                    <div id="MyGridId">
                    </div>
                </div>
            </div>


            <div id="divGridb" runat="server" class="FormBox">
                <div class="HeaderBox">
                    <asp:Label ID="lblHeaderGridb" Text="لیست سفارشات" runat="server"></asp:Label>
                </div>
                <div class="ContentBox GridContainer">
                    <table id="exampleb" class="Grid" style="width: 100%">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Position</th>
                                <th>Office</th>
                                <th>Age</th>
                                <th>Start date</th>
                                <th>Salary</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Tiger Nixon</td>
                                <td>System Architect</td>
                                <td>Edinburgh</td>
                                <td>61</td>
                                <td>2011/04/25</td>
                                <td>$320,800</td>
                            </tr>
                            <tr>
                                <td>Garrett Winters</td>
                                <td>Accountant</td>
                                <td>Tokyo</td>
                                <td>63</td>
                                <td>2011/07/25</td>
                                <td>$170,750</td>
                            </tr>
                            <tr>
                                <td>Ashton Cox</td>
                                <td>Junior Technical Author</td>
                                <td>San Francisco</td>
                                <td>66</td>
                                <td>2009/01/12</td>
                                <td>$86,000</td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <th>Name</th>
                                <th>Position</th>
                                <th>Office</th>
                                <th>Age</th>
                                <th>Start date</th>
                                <th>Salary</th>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </div>

            

            <div id="divGridffa" runat="server" class="FormBox">
                <div class="HeaderBox">
                    <asp:Label ID="lblHeaderGrida" Text="لیست سفارشات" runat="server"></asp:Label>
                </div>
                <div class="ContentBox ">
                <div class=" CustomGrid">
                    <table id="exampledd" class="Grid" style="width: 100%">
                        
                        <tbody>
                            <tr class="GridRowHeader">
                                <th>Name</th>
                                <th>Position</th>
                                <th>Office</th>
                                <th>Age</th>
                                <th>Start date</th>
                                <th>Salary</th>
                            </tr>
                            <tr>
                                <td>Tiger Nixon</td>
                                <td>System Architect</td>
                                <td>Edinburgh</td>
                                <td>61</td>
                                <td>2011/04/25</td>
                                <td>$320,800</td>
                            </tr>
                            <tr>
                                <td>Garrett Winters</td>
                                <td>Accountant</td>
                                <td>Tokyo</td>
                                <td>63</td>
                                <td>2011/07/25</td>
                                <td>$170,750</td>
                            </tr>
                            <tr>
                                <td>Ashton Cox</td>
                                <td>Junior Technical Author</td>
                                <td>San Francisco</td>
                                <td>66</td>
                                <td>2009/01/12</td>
                                <td>$86,000</td>
                            </tr>
                        </tbody>
                        
                    </table>
                </div>
                </div>
            </div>

            
            

            <div id="div1" runat="server" class="FormBox">
                <div class="HeaderBox">
                    <asp:Label ID="Label2" Text="لیست سفارشات" runat="server"></asp:Label>
                </div>
                <div class="ContentBox DTGridContainer">
                    <table id="examplec" class="Grid" style="width: 100%">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Position</th>
                                <th>Office</th>
                                <th>Age</th>
                                <th>Start date</th>
                                <th>Salary</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Tiger Nixon</td>
                                <td>System Architect</td>
                                <td>Edinburgh</td>
                                <td>61</td>
                                <td>2011/04/25</td>
                                <td>$320,800</td>
                            </tr>
                            <tr>
                                <td>Garrett Winters</td>
                                <td>Accountant</td>
                                <td>Tokyo</td>
                                <td>63</td>
                                <td>2011/07/25</td>
                                <td>$170,750</td>
                            </tr>
                            <tr>
                                <td>Ashton Cox</td>
                                <td>Junior Technical Author</td>
                                <td>San Francisco</td>
                                <td>66</td>
                                <td>2009/01/12</td>
                                <td>$86,000</td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <th>Name</th>
                                <th>Position</th>
                                <th>Office</th>
                                <th>Age</th>
                                <th>Start date</th>
                                <th>Salary</th>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </div>



        </ContentTemplate>

    </ajax:UpdatePanel>

    <%--******************************* ObjectDataSource **********************************--%>
    <asp:ObjectDataSource runat="server" ID="odsCodeNoeSys" TypeName="Kala" SelectMethod="Get_NoeSysByNoe">
        <SelectParameters>
            <asp:Parameter Name="Noe" DefaultValue="2" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <%--******************************* script **********************************--%>
    <script>
        function ThisPageInfo(isAutoPostBack, PageInfo) {
            $("#<%= lblNameSystem.ClientID %>").text(PageInfo.NameLink);
        }
    </script>
</asp:Content>


