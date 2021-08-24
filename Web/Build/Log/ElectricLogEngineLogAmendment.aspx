<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogEngineLogAmendment.aspx.cs" Inherits="Log_ElectricLogEngineLogAmendment" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Amendment</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/css") %>
        <%: Scripts.Render("~/bundles/js") %>
        <script type="text/javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
    </telerik:RadAjaxManager>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />
        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <table>
            <tr>
                <td>Original Value</td>
                <td><telerik:RadTextBox runat="server" ID="txtOrgValue" Width="150px" Enabled="false"></telerik:RadTextBox> </td>
            </tr>
            <tr>
                <td>Revised Value</td>
                <td><telerik:RadTextBox runat="server" ID="txtRevisedValue" Width="150px"></telerik:RadTextBox> </td>
            </tr>
            <tr>
                <td>Reason For Change</td>
                <td><telerik:RadTextBox runat="server" ID="txtReason" TextMode="MultiLine" Rows="5" Columns="5" Width="150px"></telerik:RadTextBox></td>
            </tr>
            <tr>
                <td><telerik:RadButton  runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click"></telerik:RadButton></td>
                <td><telerik:RadButton  runat="server" ID="btnConfirm" Text="Confirm" OnClick="btnConfirm_Click"></telerik:RadButton></td>
            </tr>
        </table>


    </form>
</body>
</html>
