<%@ Page Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="UpdatePanel_1.aspx.cs" Inherits="UpdatePanel_1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">

    <div id="divData1" runat="server" class="FormBox">
        <div class="HeaderBox">Title1 Update Time: <%= DateTime.Now.ToString() %></div>
        <div id="ContentBoxId1" class="ContentBox">
            <ajax:UpdatePanel runat="server" ID="UpdatePanel1" RenderMode="block" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <ajax:UpdateProgress class="loading" ID="UpdateProgressNoeLevelEtebarMoshtary" runat="server">
                        <ProgressTemplate>
                            <asp:Label ID="Label001" Text="" runat="server"></asp:Label>
                            <img alt="لطفا چند لحظه صبر کنید..." src="../../../Assets/Styles/images/wait.gif" />
                        </ProgressTemplate>
                    </ajax:UpdateProgress>


                    <div class="row">
                        <div class="form-group col-md-3">
                            <asp:Label ID="lblMultipeTest1" runat="server" Text="dropdownlist MultipeTest1"></asp:Label>
                            <asp:ListBox ID="ddlMultipeTest1" runat="server" class="DropDownList selectpicker" Multiple="Multiple" data-live-search="true" data-size="5"></asp:ListBox>
                        </div>
                        <div class="form-group col-md-3">
                            <asp:Label ID="lblMultipeTest2" runat="server" Text="dropdownlist MultipeTest2"></asp:Label>
                            <select name="ddlMultipeTest2" id="cphMain_ddlMultipeTest2" runat="server" class="DropDownList selectpicker" multiple="true" data-live-search="true" data-size="5" tabindex="-98">
                                <option value="Key_0">Value_0</option>
                                <option value="Key_1">Value_1</option>
                                <option value="Key_2">Value_2</option>
                                <option value="Key_3">Value_3</option>
                                <option value="Key_4">Value_4</option>
                                <option value="Key_5">Value_5</option>
                                <option value="Key_6">Value_6</option>
                                <option value="Key_7">Value_7</option>
                                <option value="Key_8">Value_8</option>
                                <option value="Key_9">Value_9</option>
                                <option value="Key_10">Value_10</option>
                            </select>
                        </div>
                        <div class="form-group col-md-3">
                            <asp:Label ID="lblAutoPostBackTest1" runat="server" Text="dropdownlist AutoPostBackTest1"></asp:Label>
                            <asp:DropDownList ID="ddlAutoPostBackTest1" runat="server" CssClass="selectpicker" AutoPostBack="true">
                                <asp:ListItem Text="Text1" Value="Value1"></asp:ListItem>
                                <asp:ListItem Text="Text2" Value="Value2"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group col-md-3">
                            <asp:Label ID="lblAutoPostBackTest2" runat="server" Text="dropdownlist AutoPostBackTest2"></asp:Label>
                            <asp:DropDownList ID="ddlAutoPostBackTest2" runat="server" CssClass="selectpicker" AutoPostBack="true" multiple="true">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="row">
                        <div class="form-group col-md-12 text-left">
                            <asp:LinkButton runat="server" ID="BindGrid2" CssClass="btn btn-primary2" OnClick="BindGrid2_Click">Bind Grid 2</asp:LinkButton>
                        </div>
                    </div>

                </ContentTemplate>
            </ajax:UpdatePanel>
            <ajax:UpdatePanel runat="server" ID="UpdatePanel2" RenderMode="block" UpdateMode="Conditional" ChildrenAsTriggers="false">
                <ContentTemplate>
                    <div class="row">
                        <div class="form-group col-md-3">
                            <asp:Label ID="Label2" runat="server" Text="dropdownlist Multipe ListBoxChain1"></asp:Label>
                            <asp:ListBox ID="ListBoxChain1" runat="server" class="DropDownList selectpicker" data-live-search="true" data-size="5" AutoPostBack="true" OnSelectedIndexChanged="ListBoxChain1_SelectedIndexChanged"></asp:ListBox>
                        </div>
                        <div class="form-group col-md-3">
                            <asp:Label ID="Label3" runat="server" Text="dropdownlist Multipe ListBoxChain2"></asp:Label>
                            <asp:ListBox ID="ListBoxChain2" runat="server" class="DropDownList selectpicker" Multiple="Multiple" data-live-search="true" data-size="5"></asp:ListBox>
                        </div>
                    </div>
                </ContentTemplate>
            </ajax:UpdatePanel>
        </div>
    </div>
    <div class="FormBox">
        <div class="HeaderBox">
            Grid Test1 ( Bind in Page Load ) 
        </div>
        <div class="ContentBox">
            <div id="MyGridId1"></div>
        </div>
    </div>
    <div class="FormBox">
        <div class="HeaderBox">
            Grid Test2 ( Bind with BindGrid2 button ) 
        </div>
        <div class="ContentBox">
            <div id="MyGridId2"></div>
        </div>
    </div>
</asp:Content>
