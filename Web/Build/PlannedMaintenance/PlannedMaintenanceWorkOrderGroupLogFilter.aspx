<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderGroupLogFilter.aspx.cs" Inherits="PlannedMaintenanceWorkOrderGroupLogFilter" %>

<!DOCTYPE html>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Work Order Group Filter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:tabstrip id="MenuWorkOrderReportLogFilter" runat="server" ontabstripcommand="MenuWorkOrderReportLogFilter_TabStripCommand"></eluc:tabstrip>
        <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
        <br clear="all" />
        <table width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblWorkOrderNumber" runat="server" Text="Work Order Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" RenderMode="Lightweight" ID="txtWorkOrderNumber" CssClass="input" MaxLength="20"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblWorkOrderTitle" runat="server" Text="Work Order Title"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" RenderMode="Lightweight" ID="txtWorkOrderTitle" CssClass="input" MaxLength="100" Width="280px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblComponentCategory" runat="server" Text="Job Category"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucCompCategory" AppendDataBoundItems="true" CssClass="input" Width="300px" runat="server" />
                </td>
                 <td>
                    <telerik:RadLabel ID="lblWorkDoneDateBetween" runat="server" Text="Work Done Date Between"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date runat="server" ID="txtWorkDoneDateFrom" CssClass="input" />
                    &nbsp;-&nbsp;
                    <eluc:Date runat="server" ID="txtWorkDoneDateTo" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblIsUnPlanned" runat="server" Text="Routine WO."></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkRoutine" runat="server"></telerik:RadCheckBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
