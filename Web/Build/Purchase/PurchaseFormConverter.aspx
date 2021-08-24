<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormConverter.aspx.cs" Inherits="Purchase_PurchaseFormConverter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormType" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <eluc:TabStrip ID="MenuStockItemGeneral" runat="server" OnTabStripCommand="InventoryStockItemGeneral_TabStripCommand"></eluc:TabStrip>

        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <asp:Literal ID="lblFormType" runat="server" Text="Form Type"></asp:Literal>
                </td>
                <td>
                    <telerik:RadRadioButtonList ID="rdoListFormType" runat="server" DataTextField="FLDHARDNAME"  Direction="Horizontal" RepeatLayout="Table"
                        DataValueField="FLDHARDCODE" RepeatDirection="Horizontal" ValidationGroup="FormType">
                        <Items>
                            <telerik:ButtonListItem Text="Unchange" Value="0" Selected="True"></telerik:ButtonListItem>
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
            </tr>
            <tr>
                <td></td><td></td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblFormStatus" runat="server" Text="Form Status"></asp:Literal></td>
                <td>
                    <telerik:RadRadioButtonList ID="rdoListFormStatus" runat="server" DataTextField="FLDHARDNAME"  Direction="Horizontal" RepeatLayout="Table"
                        DataValueField="FLDHARDCODE" RepeatDirection="Horizontal">
                        <Items>
                            <telerik:ButtonListItem Text="Unchange" Value="0" Selected="True"></telerik:ButtonListItem>
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
            </tr>
        </table>

        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
