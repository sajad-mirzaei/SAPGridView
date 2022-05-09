<%@ Page Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="Modal_1.aspx.cs" Inherits="Modal_1" Title="Test Modal 1" %>

<asp:Content ID="Content" ContentPlaceHolderID="cphMain" runat="Server">
    <ajax:UpdatePanel runat="server" ID="upForm" RenderMode="block" UpdateMode="Always" OnLoad="Page_Load">
        <ContentTemplate>
            <ajax:UpdateProgress class="loading" ID="UpdateProgressLogin" runat="server" UpdateMode="Always">
                <ProgressTemplate>
                    <asp:Label runat="server" ID="Label001" Text=""></asp:Label>
                    <img alt="لطفا چند لحظه صبر کنید..." src='<%= Page.ResolveClientUrl("~/Assets/Styles/images/wait.gif") %>' />
                </ProgressTemplate>
            </ajax:UpdateProgress>
            <div class="FormBox">
                <div class="HeaderBox">
                    <asp:Label ID="lblNameSystem" Text=" Grid_Levels1 " runat="server"></asp:Label>
                </div>
                <div class="ContentBox">
                    <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#ChangeMarakez1111">
                        Open modal
                    </button>

                </div>
            </div>


        </ContentTemplate>
    </ajax:UpdatePanel>

    <div class="modal fade" id="ChangeMarakez1111">
        <div class="modal-dialog modal-lg">
            <div class="modal-content ContentBox">
                <div class="modal-header">
                    <h4 class="modal-title">انتخاب مراکز</h4>
                    <i class="fa fa-remove close closeModal" data-dismiss="modal" aria-hidden="true"></i>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel class="row" ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="form-group col-lg-2 col-md-3 col-sm-12 col-xs-12">
                                <asp:Label runat="server" ID="lblNoeMarkaz" class="lblNoeMarkaz" Text=" نوع مرکز : "></asp:Label>
                                <asp:DropDownList class="form-control selectpicker" ID="ddlNoeMarkaz" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNoeMarkaz_SelectedIndexChanged">
                                    <asp:ListItem Value="1" Text="Text 1"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Text 2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="col-3 noLabelBottom2 mr-0">
                                <asp:LinkButton ID="btnTaeed" OnClick="btnTaeed_Click" runat="server" CssClass="Button" Text="تائید"></asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">بستن پنجره</button>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
