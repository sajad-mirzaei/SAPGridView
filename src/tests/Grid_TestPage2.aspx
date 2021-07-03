<%@ Page Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="Grid_TestPage2.aspx.cs" Inherits="GridTestPage_Simple" %>

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
                    Test1
                </div>
                <div class="ContentBox">


                    <div id="MyGridId"></div>



                </div>
            </div>

            <script>
                function DataTableCallBackData(oData) {
                    console.log(oData);
                }
            </script>


        </ContentTemplate>
    </ajax:UpdatePanel>
</asp:Content>