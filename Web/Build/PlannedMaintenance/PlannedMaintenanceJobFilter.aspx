<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceJobFilter.aspx.cs"
    Inherits="PlannedMaintenanceJobFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        document.onkeydown = function (e) {
            var keyCode = (e) ? e.which : event.keyCode;
            if (keyCode == 13) {
                __doPostBack('MenuJobFilter$dlstTabs$ctl00$btnMenu', '');
            }
        }
    </script>
</head>
<body>
    <form id="frmJobFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <eluc:TabStrip ID="MenuJobFilter" runat="server" OnTabStripCommand="JobFilter_TabStripCommand"></eluc:TabStrip>
        <table width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblJobCode" runat="server" Text="Job Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtJobCode" CssClass="input" MaxLength="20" Width="240px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblJobTitle" runat="server" Text="Job Title"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtJobTitle" CssClass="input" MaxLength="200" Width="240px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblJobClass" runat="server" Text="Job Class"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucJobClass" runat="server" QuickTypeCode="34" AppendDataBoundItems="true" Width="240px"/>
                </td>

            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblJobCategory" runat="server" Text="Job Category"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucJobCategory" runat="server" QuickTypeCode="165" AppendDataBoundItems="true" Width="240px"/>
                </td>

            </tr>
        </table>
    </form>
</body>
</html>
