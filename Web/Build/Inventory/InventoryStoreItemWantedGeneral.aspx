<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryStoreItemWantedGeneral.aspx.cs" Inherits="InventoryStoreItemWantedGeneral" %>
    
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Spare Item Wanted</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server"  autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>   
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlStoreItemWantedGeneral">
        <ContentTemplate>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                       <eluc:MaskNumber runat="server" ID="txtNumber" MaskText="##.##.##" CssClass="input readonlytextbox" 
                                MaxLength="50" Width="90px" ReadOnly="true"></eluc:MaskNumber>                     
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input readonlytextbox" MaxLength="200" Width="240px"  ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="width: 35">
                        <telerik:RadLabel ID="lblPreferredVendor" runat="server" Text="Preferred Vendor"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPreferredVendorCode" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                            Width="90px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtPreferredVendorName" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                            Width="210px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblUnit" runat="server" Text="Unit"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUnit" runat="server" CssClass="input readonlytextbox" Width="80px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr valign="top" style="width: 15%">
                    <td>
                        <telerik:RadLabel ID="lblPrice" runat="server" Text="Price"></telerik:RadLabel>
                    </td>
                    <td>
                         <eluc:UserControlCurrency ID="ddlStockAveragePriceCurrency" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            runat="server" AppendDataBoundItems="true" Width="90px" />
                        <telerik:RadTextBox runat="server" ID="txtPrice" CssClass="input txtNumber readonlytextbox" Width="210px" ReadOnly="true"></telerik:RadTextBox>                            
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblReorderLevel" runat="server" Text="Reorder Level"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReOrderLevel" runat="server" CssClass="input readonlytextbox" Width="80px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
