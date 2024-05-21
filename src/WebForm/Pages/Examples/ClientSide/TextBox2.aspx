<%@ Page Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="TextBox2.aspx.cs" Inherits="TextBox2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <ajax:UpdatePanel runat="server" ID="upForm" RenderMode="block" UpdateMode="Always">
        <ContentTemplate>
            <ajax:UpdateProgress class="loading" ID="UpdateProgressNoeLevelEtebarMoshtary" runat="server">
                <ProgressTemplate>
                    <asp:Label ID="Label001" Text="" runat="server"></asp:Label>
                    <img alt="لطفا چند لحظه صبر کنید..." src="../../../Assets/Styles/images/wait.gif" />
                </ProgressTemplate>
            </ajax:UpdateProgress>

            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:LinkButton ID="LinkButtonHidden" runat="server" CssClass="d-none" OnClick="LinkButtonHidden_Click">LinkButtonHidden</asp:LinkButton>
        </ContentTemplate>
    </ajax:UpdatePanel>
    <div class="FormBox">
        <div class="HeaderBox">
            Test1 <i class='fa fa-solid fa-filter-circle-xmark'></i>
        </div>
        <div class="ContentBox">
            <div id="MyGridId"></div>
        </div>
    </div>
    <script>
        function FunctionTest1(id) {
            var v = $("#TextBox-" + id).val();
            $("#<%=HiddenField1.ClientID%>").val(v);
            __doPostBack('ctl00$cphMain$LinkButtonHidden', '');
        }
    </script>
</asp:Content>
