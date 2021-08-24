<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormSplit.aspx.cs"
    Inherits="PurchaseFormSplit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Split Requisition</title>
       <telerik:RadCodeBlock ID="RadCodeBlock" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/jquery-1.12.4.min.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormType" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuStockItemGeneral">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuStockItemGeneral"/>
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
    
        <eluc:TabStrip ID="MenuStockItemGeneral" runat="server" OnTabStripCommand="InventoryStockItemGeneral_TabStripCommand">
                        </eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblFormType" runat="server" Text="Form Type"></telerik:RadLabel>
                        
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rdoFormType" runat="server"  Direction="Horizontal" RenderMode="Lightweight"
                              Width="360px">
                            <DataBindings DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" />
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblSplitTo" runat="server" Text="Split To"></telerik:RadLabel>
                        
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblCreation" runat="server" Direction="Horizontal" RenderMode="Lightweight"
                            Width="394px" AutoPostBack="true" OnSelectedIndexChanged="rblCreation_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Text="Sub Form" Value="0" Selected="true" />
                                <telerik:ButtonListItem Text="New Form - Automatic" Value="1" />
                                <telerik:ButtonListItem Text="Existing Form" Value="2" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                 <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblordernumber" runat="server" Visible="false">Order Number</telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListOrder" runat="server" visible="false">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtOrderNumber" runat="server" ReadOnly="false"  
                                MaxLength="20" Width="120px"></telerik:RadTextBox>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtOrderName" runat="server" ReadOnly="false" 
                                MaxLength="200" Width="210px"></telerik:RadTextBox>
                            <%--<asp:ImageButton runat="server" ID="imgOrder" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />--%>
                             <asp:LinkButton ID="imgOrder" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtOrderId" runat="server" CssClass="input" MaxLength="50" Width="0px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                       <telerik:RadLabel RenderMode="Lightweight" ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                        
                    </td>
                    <td>
                        <span id="spnPickListContract">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtContractorCode" runat="server" Width="120px" CssClass="input readonlytextbox"></telerik:RadTextBox>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtContractorName" runat="server" Width="210px" CssClass="input readonlytextbox"></telerik:RadTextBox>
                            <%--<asp:ImageButton runat="server" ID="btnContractPickList" Text=".." ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" ImageAlign="AbsMiddle" />--%>
                             <asp:LinkButton ID="btnContractPickList" runat="server" ImageAlign="AbsMiddle" Text="..">
                                <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtContractorId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtQuotationId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
               
            </table>
       <%-- <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
            border: none; width: 100%">
                        
        </div>--%>
    </form>
</body>
</html>
