<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsProjectListFilter.aspx.cs" Inherits="Accounts_AccountsProjectListFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Project Code List Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:TabStrip ID="ProjectCodeList" runat="server" OnTabStripCommand="ProjectCodeList_TabStripCommand"></eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table cellpadding="7" cellspacing="4" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="ltTitle" runat="server" Text="Title"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtTitle" runat="server" CssClass="input"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="ltProjectCode" runat="server" Text="Project Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtProjectCode" runat="server" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ddltype" runat="server" AppendDataBoundItems="true" CssClass="input"
                        QuickTypeCode="156" Width="200px" />
                </td>

            </tr>
        </table>
    </form>
</body>
</html>
