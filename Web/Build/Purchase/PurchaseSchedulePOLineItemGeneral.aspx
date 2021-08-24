<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseSchedulePOLineItemGeneral.aspx.cs" Inherits="PurchaseSchedulePOLineItemGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit"  Src="~/UserControls/UserControlPurchaseUnit.ascx"%>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purchase Bulk Purchase</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmBulkPurchase" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
        position: absolute;">
        <asp:UpdatePanel runat="server" ID="pnlBulkPurchase">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <div id="div1" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ttlInvoice" Text="General" ShowMenu="false"></eluc:Title>
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBulkPurchase" runat="server" OnTabStripCommand="MenuBulkPurchase_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div class="navSelect3" style="position: relative; z-index: +2">
                    <table cellpadding="2" width="100%">
                        <tr>
                            <td width="15%">
                               <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                            </td>
                            <td width="35%">
                                <asp:TextBox ID="txtItemName" runat="server" CssClass="input_mandatory" Width="240px"></asp:TextBox>
                            </td>
                            <td width="15%">
                               <asp:Literal ID="lblUnit" runat="server" Text="Unit"></asp:Literal>
                                
                            </td>
                            <td width="35%">
                                <eluc:Unit ID="ucUnit" runat="server" AppendDataBoundItems="true" CssClass="input" />
                            </td>
                        </tr>
                        <tr>
                            <td width="15%">
                               <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                                
                            </td>
                            <td width="35%">
                                <span id="spnPickListMainBudget">
                                    <asp:TextBox ID="txtBudgetCode" runat="server" Width="60px" CssClass="input" Enabled="False"></asp:TextBox>
                                    <asp:TextBox ID="txtBudgetName" runat="server" Width="180px" CssClass="input" Enabled="False"></asp:TextBox>
                                    <asp:ImageButton ID="btnShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." />
                                    <asp:TextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                                    <asp:TextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                                </span>
                            </td>
                            <td width="15%">
                               <asp:Literal ID="lblUnitPrice" runat="server" Text="Unit Price"></asp:Literal>
                               
                            </td>
                            <td width="35%">
                                <eluc:Number ID="ucPrice" runat="server" CssClass="input_mandatory txtNumber" DecimalPlace="2"
                                    IsPositive="true" MaxLength="10" Width="90px" />
                            </td>
                        </tr>
                        <tr>
                            <td width="15%">
                               <asp:Literal ID="lblTotalOrderQuantity" runat="server" Text="Total Order Quantity"></asp:Literal>
                                
                            </td>
                            <td width="35%">
                                <eluc:Number ID="ucRequestedQty" runat="server" CssClass="input_mandatory txtNumber"
                                    MaxLength="7" IsInteger="true" IsPositive="true" Width="90px" />
                            </td>
                            <td width="15%">
                                &nbsp;
                            </td>
                            <td width="35%">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
