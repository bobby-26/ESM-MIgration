<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseBulkPOLineItemGeneral.aspx.cs" Inherits="PurchaseBulkPOLineItemGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlPurchaseUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskedText" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>

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
        <table cellpadding="2" width="100%">
            <tr>
                <td width="15%">
                    <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    &amp;
                                    <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <span id="spnPickItem">
                        <%--<eluc:MaskedText ID="txtItemNumber" runat="server" MaskText="##.##.##" CssClass="input" Width="100px"></eluc:MaskedText>--%>
                        <telerik:RadMaskedTextBox ID="txtItemNumber" runat="server" Mask="##.##.##" Width="100px"></telerik:RadMaskedTextBox>
                        <telerik:RadTextBox ID="txtPartName" runat="server" CssClass="input_mandatory" Width="200px"></telerik:RadTextBox>
                        <asp:LinkButton ID="cmdShowItem" runat="server" ImageAlign="AbsMiddle" Text="..">
                            <span class="icon"><i class="fas fa-tasks"></i></span>
                        </asp:LinkButton>

                        <telerik:RadTextBox ID="txtPartId" runat="server" CssClass="hidden" Width="0px"></telerik:RadTextBox>
                    </span>
                </td>
                <td width="15%">
                    <telerik:RadLabel ID="lblUnit" runat="server" Text="Unit"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <eluc:Unit ID="ucUnit" runat="server" AppendDataBoundItems="true" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td width="15%">
                    <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <span id="spnPickListMainBudget">
                        <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="100px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="200px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                        <asp:LinkButton ID="btnShowBudget" runat="server" ImageAlign="AbsMiddle" Text="..">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                    </span>
                </td>
                <td width="15%">
                    <telerik:RadLabel ID="lblUnitPrice" runat="server" Text="Unit Price"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <eluc:Number ID="ucPrice" runat="server" CssClass="input_mandatory txtNumber" DecimalPlace="2"
                        IsPositive="true" MaxLength="9" Width="120px" />
                </td>
            </tr>
            <tr>
                <td width="15%">
                    <telerik:RadLabel ID="lblTotalOrderQuantity" runat="server" Text="Total Order Quantity"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <eluc:Number ID="ucTotalOrderQty" runat="server" CssClass="readonlytextbox txtNumber"
                        IsInteger="true" IsPositive="true" MaxLength="7" ReadOnly="true" Width="120px" />
                    <asp:ImageButton ID="ibtnRecord" runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ PhoenixTheme:images/Modify.png %>"
                        Text=".." ToolTip="Record Seal Numbers" />
                    <asp:ImageButton ID="ibtnInventoryUpdate" runat="server" ImageAlign="AbsMiddle"
                        ImageUrl="<%$ PhoenixTheme:images/save.png %>" Text=".." ToolTip="Update Inventory" />
                    <asp:ImageButton ID="ibtnMoreSeals" runat="server" Visible="false" ImageAlign="AbsMiddle" ImageUrl="<%$ PhoenixTheme:images/additional-order.png %>"
                        Text=".." ToolTip="Receive more seals" />
                </td>
                <td width="15%">
                    <telerik:RadLabel ID="lblTotalReceivedQuantity" runat="server" Text="Total Received Quantity"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <eluc:Number ID="ucTotalReceivedQty" runat="server" CssClass="readonlytextbox txtNumber"
                        IsInteger="true" IsPositive="true" MaxLength="7" ReadOnly="true" Width="120px" />
                </td>
            </tr>
            <tr>
                <td width="15%">
                    <telerik:RadLabel ID="lblTotal" runat="server" Text="Total"></telerik:RadLabel>
                </td>
                <td width="35%">
                    <eluc:Number ID="ucTotal" runat="server" CssClass="readonlytextbox txtNumber"
                        DecimalPlace="2" IsPositive="true" ReadOnly="true" Width="120px" />
                </td>
                <td width="15%">&nbsp;</td>
                <td width="35%">&nbsp;</td>
            </tr>
        </table>
    </form>
</body>
</html>
