<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseBulkPOLineItemSealLocationUpdate.aspx.cs" Inherits="PurchaseBulkPOLineItemSealLocationUpdate" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Location Update</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblSealLocation" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%" EnableAJAX="false">
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:TabStrip ID="MenuLocation" runat="server" OnTabStripCommand="MenuLocation_TabStripCommand">
        </eluc:TabStrip>
    <table id="tblSealLocation" cellpadding="1" cellspacing="1" width="30%">
        <tr>
            <td colspan="2">
                    <telerik:RadLabel ID="lblSelectLocationtoupdate" runat="server" Text="Select Location to update" Font-Bold="true"></telerik:RadLabel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadLabel ID="lblReceivedQuantity" runat="server" Text="Received Quantity"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadTextBox ID="txtReceivedQty" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>
            </td>
            <td>
                <telerik:RadComboBox ID="ddlLocation" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" ></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small"></asp:Label>
            </td>
        </tr>
    </table>
       
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
