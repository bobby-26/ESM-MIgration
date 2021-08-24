<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOwnerOfficeSingleDepartmentConvertCurrency.aspx.cs" Inherits="AccountsOwnerOfficeSingleDepartmentConvertCurrency" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Single Department</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSingleDepartment" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Title runat="server" ID="ucTitle" Text="" ShowMenu="false" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuSingleDepartment" runat="server" OnTabStripCommand="MenuSingleDepartment_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                    <td width="20%">
                        <font size="2px">
                            <telerik:RadLabel ID="lblBankCurrency" runat="server" Text="Bank Account Currency"></telerik:RadLabel>
                        </font>
                    </td>
                    <td width="10%">
                        <telerik:RadTextBox ID="txtBankCurrency" runat="server" CssClass="readonlytextbox" Enabled="false" Width="100%"></telerik:RadTextBox>
                    </td>
                    <td width="30%">
                        <eluc:Number ID="ucBankAmount" runat="server" CssClass="input_mandatory" Width="50%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <font size="2px">
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text="Debit / Credit Note Currency"></telerik:RadLabel>
                        </font>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCurrency" runat="server" CssClass="readonlytextbox" Enabled="false" Width="100%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <eluc:Number ID="ucAmount" runat="server" CssClass="readonlytextbox" Width="50%" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
