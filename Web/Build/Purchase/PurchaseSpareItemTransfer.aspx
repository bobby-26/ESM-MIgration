<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseSpareItemTransfer.aspx.cs" Inherits="PurchaseSpareItemTransfer" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Spare Item Transfer</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormDetail" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="panel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="panel1" Height="98%">
            <eluc:TabStrip ID="MenuFormDetail" runat="server" OnTabStripCommand="MenuFormDetail_TabStripCommand" Title=""></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" Width="90%" ID="txtVesselName" runat="server" Enabled="false">
                        </telerik:RadTextBox>
                        <telerik:RadLabel ID="lblvesselid" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPartNumber" runat="server" Text="Part Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" Width="90%" ID="txtPartNumber" runat="server" Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPartName" runat="server" Text="Part Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" Width="90%" ID="txtPartName" runat="server" Enabled="false">
                        </telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblMaker" runat="server" Text="Maker"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" Width="90%" ID="txtMaker" runat="server" Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPreferedVendor" runat="server" Text="Prefered Vendor"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" Width="90%" ID="txtPreferedVendor" runat="server" Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                    <td colspan="2"></td>

                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblFromVessel" runat="server" Text="From Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlvessel" runat="server" Width="90%" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblQuantity" runat="server" Text="Quantity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtQuantity" runat="server" Width="90%" CssClass="input" DecimalDigits="0" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
