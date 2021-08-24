<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2TankAdd.aspx.cs" Inherits="Log_ElectricLogORB2TankAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VerticalSplitter" Src="~/UserControls/UserControlVerticalSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:TabStrip ID="logTankEdit" runat="server" OnTabStripCommand="logTankEdit_TabStripCommand"></eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table cellspacing="1" width="100%">
            <tr>
                <br />
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblTankName" runat="server" Text="Tank Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtTankName" CssClass="input_mandatory" Width="150px" runat="server"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblIoppName" runat="server" Text="IOPP Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtIoppName" CssClass="input_mandatory" Width="150px" runat="server"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblCapacity" runat="server" Text="100% Vol (m3)"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Decimal ID="txtCapacity" runat="server" Width="150px" CssClass="input_mandatory"/>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblTankType" runat="server" Text="Tank Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" Style="width: auto" ID="ddlTankType" runat="server" EnableLoadOnDemand="True"
                         EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblFrameFrom" runat="server" Text="Frame From-To"></telerik:RadLabel>
                </td>
                <td>
                 <telerik:RadTextBox runat="server" ID="txtFrameFrom" Width="150px" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblLateralPosition" runat="server" Text="Lateral Position"></telerik:RadLabel>
                </td>
                <td>
                 <telerik:RadTextBox runat="server" ID="txtLateralPosition" Width="150px" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

