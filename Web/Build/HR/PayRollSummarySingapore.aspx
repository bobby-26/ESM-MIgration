<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollSummarySingapore.aspx.cs" Inherits="PayRoll_PayRollSummarySingapore" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Summary</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        .container {
            width: 30%;
            /*margin: 0 auto;*/
        }

        table {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
   <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="gvTabStrip" runat="server"></eluc:TabStrip>
        <div class="container">
            <table>
                <tr>
                    <td>
                        <h3>Total Income : </h3>
                    </td>
                    <td style="text-align: right;">
                        <telerik:RadLabel ID="lbltotalsalary" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
               
                <tr>
                    <td>
                        <h3>Total Deduction : </h3>
                    </td>
                    <td style="text-align: right;">
                        <telerik:RadLabel ID="lbldeduction" runat="server"></telerik:RadLabel>

                    </td>
                </tr>
            </table>
            <hr />
            <table>
                <tr>
                    <td>
                        <h3>Total Tax Payable for the Employee: </h3>
                    </td>
                    <td style="text-align: right;">
                        <telerik:RadLabel ID="lbltotaltax" runat="server"></telerik:RadLabel>
                    </td>
                </tr>

                <tr>
                    <td>
                        <h3>Total TDS:</h3>
                    </td>
                    <td style="text-align: right;">
                        <telerik:RadLabel ID="lbltds" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <hr />
            <table>
                <tr>
                    <td>
                        <h3>Total Net Tax Payable Amount: </h3>
                    </td>
                    <td style="text-align: right;">
                        <telerik:RadLabel ID="lbltotalnettax" runat="server"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </div>

    </form>
</body>
</html>
