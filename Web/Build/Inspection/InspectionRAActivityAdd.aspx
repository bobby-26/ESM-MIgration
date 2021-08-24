<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAActivityAdd.aspx.cs" Inherits="InspectionRAActivityAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategoryExtn.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Activity</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionIncidentCriticalFactor" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <eluc:TabStrip ID="MenuCARGeneral" runat="server" OnTabStripCommand="MenuCARGeneral_TabStripCommand" Title="Activity"></eluc:TabStrip>
            <table id="tblDetails" runat="server" width="100%">
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCode" runat="server" Text="Short Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtcode" runat="server" CssClass="input_mandatory" Width="90px" MaxLength="3" >
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblActivity" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtlActivity" runat="server" CssClass="input_mandatory" Width="360px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblprocess" runat="server" Text="Process"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Category ID="ucCategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true" Width="250px" OnTextChangedEvent="ucCategory_TextChangedEvent" CssClass="input_mandatory" />
                    </td>
                </tr>    
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblEvent" runat="server" Text="Event"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="chkevent" runat="server" RepeatColumns="3" RepeatLayout="Table" CssClass="input" Width="50%"></asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblNatureofWok" runat="server" Text="Nature of Work"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucNatureofWork" runat="server" QuickTypeCode="84" AppendDataBoundItems="true" Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDMRTimeSheet" runat="server" Text="DMR Time Sheet Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkDMRYN" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOperationalSummaryyn" runat="server" Text="Operational Summary Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkOperatiponalsummary" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

