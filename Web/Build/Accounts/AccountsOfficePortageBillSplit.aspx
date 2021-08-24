<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOfficePortageBillSplit.aspx.cs"
    Inherits="AccountsOfficePortageBillSplit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Split</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuPB" runat="server" OnTabStripCommand="MenuPB_TabStripCommand"></eluc:TabStrip>

            <table cellpadding="1" cellspacing="1" width="50%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTotalSplitAmount" runat="server" Text="Total Split Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSplitAmount" runat="server" CssClass="txtNumber readonlytextbox" ReadOnly="true" Width="80px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>

                        <telerik:RadLabel ID="lblSplit1" runat="server" Text="Split 1"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAmount1" runat="server" CssClass="input" DecimalPlace="2" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSplit2" runat="server" Text="Split 2"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAmount2" runat="server" CssClass="input" DecimalPlace="2" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSplit3" runat="server" Text="Split 3"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAmount3" runat="server" CssClass="input" DecimalPlace="2" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSplit4" runat="server" Text="Split 4"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAmount4" runat="server" CssClass="input" DecimalPlace="2" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSplit5" runat="server" Text="Split 5"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAmount5" runat="server" CssClass="input" DecimalPlace="2" Width="80px" />
                    </td>
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
