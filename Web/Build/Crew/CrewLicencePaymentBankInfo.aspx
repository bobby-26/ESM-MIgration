<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicencePaymentBankInfo.aspx.cs"
    Inherits="Crew_CrewLicencePaymentBankInfo" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bank Info</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAddress" runat="server" Text="Address"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtConsulate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="100%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBankName" runat="server" Text="Bank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBank" runat="server" CssClass="readonlytextbox" ReadOnly="true"  Width="30%"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCurrency" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="10%"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtBeneficiaryName" runat="server" CssClass="readonlytextbox" ReadOnly="true" 
                            ToolTip="Beneficiary Name"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtBankAccount" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            ToolTip="Bank Account"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <eluc:Address runat="server" ID="ucAddress"></eluc:Address>
                        <eluc:Status runat="server" ID="ucStatus" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
