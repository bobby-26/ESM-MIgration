<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPEARSRAConsequenceImpactAdd.aspx.cs" Inherits="InspectionPEARSRAConsequenceImpactAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <eluc:TabStrip ID="MenuRAImpactAdd" runat="server" OnTabStripCommand="MenuRAImpactAdd_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr>
                    <td Width="38%">
                        <telerik:RadLabel ID="lblHazard" runat="server" Text="Hazard Category"></telerik:RadLabel>
                    </td>
                    <td Width="60%">
                        <telerik:RadComboBox ID="ddlHazard" runat="server" CssClass="input_mandatory" AutoPostBack="true" DataTextField="FLDNAME" DataValueField="FLDHAZARDCATEGORYID" EmptyMessage="Type to select" Filter="Contains" Width="260px"></telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSeverity" runat="server" Text="Severity"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSeverity" runat="server" CssClass="input_mandatory" AutoPostBack="true" DataTextField="FLDNAME" DataValueField="FLDSEVERITYID" EmptyMessage="Type to select" Filter="Contains" Width="260px"></telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblConsequence" runat="server" Text="Consequence Impact"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtImpact" runat="server" CssClass="input_mandatory" TextMode="MultiLine" Resize="Both" Width="350px" Height="70px" MaxLength="500"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActiveYN" runat="server" Text="Active Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="cbActiveyn" runat="server"></telerik:RadCheckBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
