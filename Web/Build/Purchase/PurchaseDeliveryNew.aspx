<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseDeliveryNew.aspx.cs"
    Inherits="PurchaseDeliveryNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseFormType" runat="server" autocomplete="off">
     <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlOrderForm" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuStockItemGeneral" Title="New Delivery" runat="server" OnTabStripCommand="InventoryStockItemGeneral_TabStripCommand">
            </eluc:TabStrip>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input_mandatory"
                            AssignedVessels="true" VesselsOnly="true" 
                            AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                       <%-- Stock Type--%>
                    </td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlStockType" CssClass="input_mandatory" Visible ="false"   AutoPostBack="true">
                            <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" Selected="True"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Spares" Value="SPARE"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Stores" Value="STORE"></telerik:RadComboBoxItem>
                                </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>               
                </table>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
