<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationCompareSplit.aspx.cs"
    Inherits="PurchaseQuotationCompareSplit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseFormType" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Split Form" ShowMenu="false"></eluc:Title>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuStockItemGeneral" runat="server" OnTabStripCommand="InventoryStockItemGeneral_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
            border: none; width: 100%">
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                       <asp:Literal ID="lblFormType" runat="server" Text="Form Type"></asp:Literal>
                        
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rdoFormType" runat="server" DataTextField="FLDHARDNAME"
                            DataValueField="FLDHARDCODE" RepeatDirection="Horizontal"  Width="360px">
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                       <asp:Literal ID="lblSplitTo" runat="server" Text="Split To"></asp:Literal>
                        
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblCreation" runat="server" RepeatDirection="Horizontal"
                            Width="394px" AutoPostBack="true" OnSelectedIndexChanged="rblCreation_SelectedIndexChanged">
                            <asp:ListItem Text="Sub Form" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="New Form - Automatic" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Existing Form" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:Label ID="lblordernumber" runat="server" Visible="false">Order Number</asp:Label>
                    </td>
                    <td>
                        <span id="spnPickListOrder" runat="server" visible="false">
                            <asp:TextBox ID="txtOrderNumber" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="120px"></asp:TextBox>
                            <asp:TextBox ID="txtOrderName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="200" Width="210px"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="imgOrder" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <asp:TextBox ID="txtOrderId" runat="server" CssClass="input" MaxLength="50" Width="10px"></asp:TextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                       <asp:Literal ID="lblVendor" runat="server" Text="Vendor"></asp:Literal>
                        
                    </td>
                    <td>
                        <span id="spnPickListContract">
                            <asp:TextBox ID="txtContractorCode" runat="server" Width="120px" CssClass="input readonlytextbox"></asp:TextBox>
                            <asp:TextBox ID="txtContractorName" runat="server" Width="210px" CssClass="input readonlytextbox"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="btnContractPickList" Text=".." ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" ImageAlign="AbsMiddle" />
                            <asp:TextBox ID="txtContractorId" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                            <asp:TextBox ID="txtQuotationId" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                        </span>
                    </td>
                </tr>
               
            </table>
        </div>
    </div>
   <%-- <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />--%>
    </form>
</body>
</html>
