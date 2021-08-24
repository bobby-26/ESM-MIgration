<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterCrewTravelAgentConfigurationEdit.aspx.cs" Inherits="RegisterCrewTravelAgentConfigurationEdit" %>

<!DOCTYPE html>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAgent" Src="~/UserControls/UserControlMultiColumnAgent.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Travel Agent</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Localization-OK="Yes" Localization-Cancel="No" Width="100%" />
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand"></eluc:TabStrip>
            <table id="table1" rules="server" cellpadding="2" cellspacing="2" width="80%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblagent" runat="server" Text="Agent"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiAgent runat="server" ID="ucAddrAgent" ProductShortCode="TAG"
                            Width="80%" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblisactive" runat="server" Text="Default YN"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkactive" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
