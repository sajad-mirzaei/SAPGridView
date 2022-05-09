<%@ Page ValidateRequest="false" Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="SoalatMotadavel.aspx.cs" Inherits="SoalatMotadavelPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <ajax:UpdatePanel runat="server" ID="upForm" RenderMode="block" UpdateMode="Always">
        <ContentTemplate>
            <link href="<%= Page.ResolveClientUrl("~/Assets/Plugins/froala_editor/v2.4.0") %>/css/froala_editor.pkgd.min.css" rel="stylesheet" type="text/css" />
            <link href="<%= Page.ResolveClientUrl("~/Assets/Plugins/froala_editor/v2.4.0") %>/css/froala_style.min.css" rel="stylesheet" type="text/css" />
            <ajax:UpdateProgress class="loading" ID="UpdateProgressNoeLevelEtebarMoshtary" runat="server">
                <ProgressTemplate>
                    <asp:Label ID="Label001" Text="" runat="server"></asp:Label>
                    <img alt="لطفا چند لحظه صبر کنید..." src="<%= Page.ResolveClientUrl("~/Assets/Styles/images/wait.gif") %>" />
                </ProgressTemplate>
            </ajax:UpdateProgress>
            <div id="divDataEntry" runat="server" class="FormBox">
                <div class="HeaderBox">
                    <asp:Label class="label" ID="lblNameSystem" runat="server"></asp:Label>
                </div>
                <div id="ContentBoxId1" class="ContentBox">
                    <div runat="server" id="MessageBox"></div>
                    <div class="row">
                        <div class="form-group col-lg-12 col-md-12 col-sm-12 col-12 ">
                            <span class="text-danger">*</span>
                            <asp:Label ID="lblSoal" label-for="txtSoal" class="label" Text="عنوان سوال:" runat="server"></asp:Label>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="text-danger" runat="server" ErrorMessage="عنوان سوال را وارد کنید" ControlToValidate="txtSoal"></asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" ID="txtSoal" Class="form-control" placeholder="عنوان سوال"></asp:TextBox>
                        </div>
                        <div class="form-group col-lg-10 col-md-10 col-sm-10 col-12 ">
                            <span class="text-danger">*</span>
                            <asp:Label ID="lblPasokh" label-for="txtPasokh" class="label" Text="جواب:" runat="server"></asp:Label>
                            <asp:TextBox class="form-control editor" runat="server" ID="txtPasokh" TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="text-danger" runat="server" ErrorMessage="متن جواب را وارد کنید" ControlToValidate="txtPasokh"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-lg-2 col-md-2 col-sm-6 col-12 noLabelBottom text-left">
                            <asp:LinkButton ID="btnSabt" Text="ثبت" CssClass="Button" OnClick="btnSabt_Click" runat="server"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
            <div id="divDataDisplay" runat="server" class="FormBox">
                <div class="HeaderBox">
                    <asp:Label class="label" ID="Label2" runat="server"></asp:Label>
                </div>
                <div id="ContentBoxId2" class="ContentBox">
                    <div id="MyGridId"></div>
                </div>
            </div>
            <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Assets/Plugins/froala_editor/v2.4.0") %>/js/froala_editor.pkgd.min.js"></script>
            <script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Assets/Plugins/froala_editor/v2.4.0") %>/js/languages/ro.js"></script>
            <script>                function onPagesLoad() {                    $('.editor').froalaEditor({                        /* language: 'fa',                        direction: 'rtl' */                        placeholderText: 'لطفا متن خود را اینجا وارد کنید',                        toolbarStickyOffset: 60,                        imageAllowedTypes: ['jpeg', 'jpg', 'png', 'gif'],                        htmlAllowedTags: ['link', 'script', "div", ".*"],                        htmlRemoveTags: [''],                        useClasses: true,                        fontSize: ['8', '9', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '22', '24', '26', '28', '30', '32', '34', '40', '42', '50', '52', '70', '90'],                        dragDrop: false                    });                }            </script>
        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>
