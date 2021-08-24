<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceReportSurveyDue.aspx.cs"
    Inherits="PlannedMaintenanceReportSurveyDue" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSurveyDueFilter" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
            <eluc:TabStrip ID="MenuReportSurveyDue" runat="server" OnTabStripCommand="MenuReportSurveyDue_TabStripCommand"></eluc:TabStrip>
        </telerik:RadCodeBlock>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblComponentNumber" runat="server" Text="Component Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentNumber" runat="server" CssClass="input"
                            MaxLength="20" Width="120px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblComponentName" runat="server" Text="Component Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input"
                            MaxLength="20" Width="160px"></telerik:RadTextBox>
                      
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblJobClasses" runat="server" Text="Job Classes"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucJobClass" runat="server" CssClass="input" AppendDataBoundItems="true" SelectedQuick="119" Enabled="false" />
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblJobCode" runat="server" Text="Job Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtJobCode" runat="server" CssClass="input" MaxLength="50" Width="264px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastDoneDateBetween" runat="server" Text="Last Done Date Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtDateFrom" CssClass="input" />
                        -
                        <eluc:Date runat="server" ID="txtDateTo" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblClassCode" runat="server" Text="Class Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtClassCode" runat="server" CssClass="input" MaxLength="50" Width="264px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
