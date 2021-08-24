<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryStoreTypeGeneral.aspx.cs"
    Inherits="InventoryStoreTypeGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUnit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskedTextBox" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInventoryStoreTypeGeneral" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:TabStrip ID="MenuStoreTypeGeneral" TabStrip="true" runat="server" OnTabStripCommand="StoreType_TabStripCommand"></eluc:TabStrip>
        <br clear="all" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="5" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MaskedTextBox runat="server" Width="100px" ID="txtStockTypeNumber" MaskText="##.##.##" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td >
                        <telerik:RadTextBox ID="txtStockTypeName" runat="server" CssClass="input_mandatory" MaxLength="200"
                            Width="240px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStoreType" runat="server" Text="Store Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlHard ID="ddlStockClass" runat="server" AppendDataBoundItems="true" Width="200px" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUnit" runat="server" Text="Unit"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlUnit ID="ddlStoreUnit" runat="server" AppendDataBoundItems="true"
                            CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListVendor">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" ReadOnly="false" CssClass="input"
                                MaxLength="20" Width="100px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVendorName" runat="server" ReadOnly="false" CssClass="input"
                                MaxLength="20" Width="170px">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="imgShowVendor" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListVendor', 'codehelp1', '', 'Common/CommonPickListAddress.aspx?addresstype=130,131&framename=ifMoreInfo', true);">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="cmdClear" ImageAlign="AbsMiddle" Text=".." OnClick="cmdClear_Click">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPurchasedPrice" runat="server" Text="Purchased Price"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:UserControlCurrency ID="ddlPurchasedPriceCurrency" runat="server" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            AppendDataBoundItems="true" CssClass="input" />
                        <eluc:MaskedTextBox ID="txtPurchasedPrice" runat="server" CssClass="input" MaskText="########.##"></eluc:MaskedTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblParentType" runat="server" Text="Parent Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListParentStockType">
                            <telerik:RadTextBox ID="txtParentStockTypeNumber" runat="server" ReadOnly="false" CssClass="input"
                                MaxLength="20" Width="100px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtParentStockTypeName" runat="server" ReadOnly="false" CssClass="input"
                                MaxLength="20" Width="170px">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="imgShowParentComponent" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListParentStockType', 'codehelp1', '', '../Common/CommonPickListStoreType.aspx', true);">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="cmdStockTypeClear" ImageAlign="AbsMiddle" Text=".." OnClick="cmdStockTypeClear_Click">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtParentStockTypeId" runat="server" CssClass="hidden">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPurchasedDate" runat="server" Text="Purchased Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtPurchaseDate" runat="server" Width="120px" CssClass="input"></eluc:Date>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
