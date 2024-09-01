<%@ Page Title="گزارش تست" Language="C#" MasterPageFile="~/MainBoard.master" AutoEventWireup="true" CodeFile="CheckBoxWithFormula.aspx.cs" Inherits="CheckBoxWithFormula" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div>
        <div class="FormBox">
            <div class="HeaderBox">
                Test1
            </div>
            <div class="ContentBox">
                <div id="MyGridId"></div>
                <div class="alert alert-info mt-2 text-left" dir="ltr">
                    <span class="text-success">Count of selected items :</span>
                    <span class="text-dark countSelectedItems">0</span>
                    <br />
                    <span class="text-success">Sum of selected items After applying formula: (price * 2) - 1 :</span>
                    <span class="text-dark sumSelectedItems">0</span>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            function myJavascriptMethodName(oData) {
                $(".countSelectedItems").text(oData.result.count);
                $(".sumSelectedItems").text(oData.result.sum);
            }
        </script>
    </div>
</asp:Content>
