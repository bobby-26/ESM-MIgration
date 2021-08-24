<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCTaskListFilter.aspx.cs"
    Inherits="InspectionMOCTaskListFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MOC Task Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSealReqFilter" runat="server">
    <eluc:TabStrip ID="MenuMOCTaskFilterMain" runat="server" OnTabStripCommand="MenuMOCTaskFilterMain_TabStripCommand">
    </eluc:TabStrip>
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input" Width="150px"
                        AssignedVessels="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblStatus" runat="server" Text="Status">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" CssClass="input"
                        HardTypeCode="146" Width="150px" ShortNameFilter="OPN,CMP,CLD" CommandName="TASKSTATUS" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblTargetDateFrom" runat="server" Text="Target Date From">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtFrom" CssClass="input" Width="150px" runat="server" DatePicker="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblTo" runat="server" Text="Target Date To">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtTo" CssClass="input" Width="150px" runat="server" DatePicker="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCompletionDateFrom" runat="server" Text="Completion Date From">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtDoneDateFrom" CssClass="input" Width="150px" runat="server" DatePicker="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblCompletionDateTo" runat="server" Text="Completion Date To">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtDoneDateTo" CssClass="input" Width="150px" runat="server" DatePicker="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblMOCRefNo" runat="server" Text="Reference Number">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtMOCRefNo" runat="server" CssClass="input" Width="150px   "
                        MaxLength="100">
                    </telerik:RadTextBox>
                </td>
            </tr>
        </table>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
