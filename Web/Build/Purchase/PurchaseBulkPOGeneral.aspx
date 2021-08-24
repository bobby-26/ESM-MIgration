<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseBulkPOGeneral.aspx.cs"
    Inherits="PurchaseBulkPOGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purchase Bulk Purchase</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBulkPurchase" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" Visible="false" />
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuBulkPurchase" runat="server" OnTabStripCommand="MenuBulkPurchase_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <table cellpadding="2" width="100%" height="30%">
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblBulkPurchaseReferenceNumber" runat="server" Text="Bulk Purchase Reference Number"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <telerik:RadTextBox ID="txtBulkPORefNo" runat="server" CssClass="readonlytextbox" Width="240px"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblFormTitle" runat="server" Text="Form Title"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <telerik:RadTextBox ID="txtFormTitle" runat="server" CssClass="input_mandatory" Width="282px"
                            MaxLength="50">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <span id="spnPickListMainBudget">
                            <telerik:RadTextBox ID="txtBudgetCode" runat="server" CssClass="input_mandatory" Enabled="False" Width="60px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetName" runat="server" CssClass="input_mandatory" Enabled="False" Width="180px"></telerik:RadTextBox>
                            <%--<asp:ImageButton ID="btnShowBudget" runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" Text=".." />--%>
                             <asp:LinkButton ID="btnShowBudget" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtBudgetId" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtVendorNumber" runat="server" Width="100px" CssClass="input_mandatory"></telerik:RadTextBox>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtVenderName" runat="server" Width="180px" CssClass="input_mandatory"></telerik:RadTextBox>
                            <%--<asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', 'Common/CommonPickListAddress.aspx?addresstype=130,131,132&framename=ifMoreInfo', true);"
                                Text=".." />--%>
                            <asp:LinkButton ID="cmdShowMaker" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', 'Common/CommonPickListAddress.aspx?addresstype=130,131,132&framename=ifMoreInfo', true);">
                                <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtVendor" runat="server" Width="1px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <eluc:UserControlCurrency ID="ddlCurrencyCode" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" CurrencyList="<%# PhoenixRegistersCurrency.ListCurrency(1)%>" />
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblStockType" runat="server" Text="Stock Type"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <telerik:RadDropDownList ID="ddlStockType" runat="server" CssClass="dropdown_mandatory"
                            AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlStockType_SelectedIndexChanged"
                            Width="90px">
                            <Items>
                                <telerik:DropDownListItem Text="SERVICE" Value="SERVICE"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="STORE" Value="STORE"></telerik:DropDownListItem>
                            </Items>
                        </telerik:RadDropDownList>
                        <eluc:Hard ID="ddlStockClass" runat="server" CssClass="dropdown_mandatory" HardTypeCode="97"
                            HardList='<%# PhoenixRegistersHard.ListHard(1, 97) %>' AppendDataBoundItems="true"
                            Width="185px" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblPartPaid" runat="server" Text="Part Paid"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <span id="spnPicPartPaid">
                            <telerik:RadTextBox ID="txtPartPaid" runat="server" CssClass="readonlytextbox" Width="100px"
                                ReadOnly="true">
                            </telerik:RadTextBox>
                            <%--<asp:ImageButton ID="cmdPicPartPaid" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />--%>
                             <asp:LinkButton ID="cmdPicPartPaid" runat="server" ImageAlign="AbsMiddle" Text="..">
                            <span class="icon"><i class="fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        </span>
                    </td>
                    <td width="15%">
                        <%--<telerik:RadLabel ID="lblPoType" runat="server" Text="PO Type"></telerik:RadLabel>--%>
                    </td>
                    <td width="35%">
                        <asp:DropDownList ID="ddlPOType" runat="server" CssClass="input_mandatory" Visible="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <span style="color: Black; font-weight: bold">Invoice Details</span>
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblInvoiceReferenceNumber" runat="server" Text="Invoice Reference Number"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <telerik:RadTextBox ID="txtInvoiceRefNo" runat="server" CssClass="input" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblInvoiceDate" runat="server" Text="Invoice Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtInvoiceDate" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblInvoiceReceivedDate" runat="server" Text="Invoice Received Date"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <eluc:UserControlDate ID="txtInvoiceReceivedDate" runat="server" CssClass="input"
                            DatePicker="true" />
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                           Width="130px" >
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlInvoiceType" runat="server" CssClass="input" HardTypeCode="59"
                            AutoPostBack="TRUE" HardList='<%# PhoenixRegistersHard.ListHard(1, 59) %>' AppendDataBoundItems="true"
                            Width="240px" />
                    </td>
                </tr>
            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
