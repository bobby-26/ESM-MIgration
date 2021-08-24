<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportRecruitmentTarget.aspx.cs" Inherits="CrewReportRecruitmentTarget" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bond For Charterers And Owners</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewReport" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RecruitmentTarget" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuCrewRecruitmentTarget" runat="server" OnTabStripCommand="MenuCrewRecruitmentTarget_TabStripCommand"></eluc:TabStrip>

        <table width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td width="15%">
                    <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Zone ID="ddlZone" runat="server" AppendDataBoundItems="true" />
                </td>
            </tr>
        </table>
        <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 85%;
            width: 100%;" class="style3"></iframe>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
