<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseDeliveryFormsGeneral.aspx.cs"
    Inherits="PurchaseDeliveryFormsGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="../UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Form</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmOrderForm" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="Title1" Text="General" ShowMenu="false"></eluc:Title>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuFormGeneral" runat="server" OnTabStripCommand="MenuFormGeneral_TabStripCommand">
        </eluc:TabStrip>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlFormGeneral">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
            <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
                border: none; width: 100%">
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                           <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFormNumber" runat="server" Width="120px" CssClass="input_mandatory"
                                Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                          <asp:Literal ID="lblFormTypeCaption" runat="server" Text="Form Type"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtType" runat="server" Text=" " Width="120px" CssClass="input"
                                Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblStockType" runat="server" Text="Stock Type"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlStockType" CssClass="input">
                                <asp:ListItem Text="--Select--" Value="Dummy" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Spares" Value="SPARE"></asp:ListItem>
                                <asp:ListItem Text="Stores" Value="STORE"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblTitle" runat="server" Text="Title"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromTitle" runat="server" Width="240px" CssClass="input_mandatory"  Enabled="False"
                                MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblCreated" runat="server" Text="Created"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCreatedDate" runat="server" Width="120px" CssClass="input" Enabled="false"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtCreatedDate" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td>
                        <asp:Literal ID="lblOrdered" runat="server" Text="Ordered"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderDate" runat="server" Width="90px" CssClass="input" Enabled="False" ReadOnly="true"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="ceOrderDate" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtOrderDate" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVendor" runat="server" Text="Vendor"></asp:Literal>
                            
                        </td>
                        <td>
                            <span id="spnPickListMaker">
                                <asp:TextBox ID="txtVendorNumber" runat="server" Width="60px" CssClass="input" Enabled="False"></asp:TextBox>
                                <asp:TextBox ID="txtVenderName" runat="server" Width="180px" CssClass="input" Enabled="False"></asp:TextBox>
                                 <asp:TextBox ID="txtVendor" runat="server" Width="1px" CssClass="input"></asp:TextBox>
                            </span>
                        </td>
                        <td>
                            <asp:Label ID="lblImportedDate" runat ="server" Text ="Imported"></asp:Label>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtRecivedDate" runat="server" Width="120px" CssClass="input" Enabled="False" ReadOnly="true"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="ceRecivedDate" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtRecivedDate" PopupPosition="TopRight">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:Literal ID="lblConfirmed" runat="server" Text="Confirmed"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtConfirmDate" runat="server" Width="90px" CssClass="input"  Enabled="False"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="ceConfirmDate" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtConfirmDate" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:Literal ID="lblDeliveryLocation" runat="server" Text="Delivery Location"></asp:Literal>
                            
                        </td>
                        <td>
                            <span id="spnDLocation">
                                <asp:TextBox ID="txtDeliveryLocationCode" runat="server" Width="60px" CssClass="input"
                                    Enabled="False"></asp:TextBox>
                                <asp:TextBox ID="txtDeliveryLocationName" runat="server" Width="180px" CssClass="input"
                                    Enabled="False"></asp:TextBox>
                              <asp:TextBox ID="txtDeliveryLocationId" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                            </span>
                        </td>
                        <td>
                        <asp:Literal ID="lblBudget" runat="server" Text="Budget"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtBugetDate" runat="server" Width="120px" CssClass="input"  Enabled="False"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="ceBugetDate" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtBugetDate" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td>
                        <asp:Literal ID="lblDelivered" runat="server" Text="Delivered"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtlLastDeliveryDate" runat="server" Width="90px" CssClass="input" Enabled="False"
                                ReadOnly="false"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtlLastDeliveryDate" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                            <asp:Label ID="lblFormType" runat="server" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:Literal ID="lblDeliveryAddress" runat="server" Text="Delivery Address"></asp:Literal>
                            
                        </td>
                        <td>
                            <span id="spnDAddress">
                                <asp:TextBox ID="txtDeliveryAddressCode" runat="server" Width="60px" CssClass="input"
                                    Enabled="False"></asp:TextBox>
                                <asp:TextBox ID="txtDeliveryAddressName" runat="server" Width="180px" CssClass="input"
                                    Enabled="False"></asp:TextBox>
                                <asp:TextBox ID="txtDeliveryAddressId" runat="server" Width="1px" CssClass="input"></asp:TextBox>
                            </span>
                        </td>
                        <td>
                        <asp:Literal ID="lblApproved" runat="server" Text="Approved"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtApproveDate" runat="server" Width="120px" CssClass="input" Enabled="false"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="ceApproveDate" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtApproveDate" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td>
                        <asp:Literal ID="lblReceived" runat="server" Text="Received"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtVenderDelveryDate" runat="server" Width="90px" CssClass="input"  Enabled="False"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="CEVenderDelveryDate" runat="server" Format="dd/MMM/yyyy"
                                Enabled="True" TargetControlID="txtVenderDelveryDate" PopupPosition="TopLeft">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                            
                        </td>
                        <td>
                            <span id="spnPickListMainBudget">
                                <asp:TextBox ID="txtBudgetCode" runat="server" Width="60px" CssClass="input" Enabled="False"></asp:TextBox>
                                <asp:TextBox ID="txtBudgetName" runat="server" Width="180px" CssClass="input" Enabled="False"></asp:TextBox>
                                <asp:TextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                                <asp:TextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input"></asp:TextBox>
                            </span>
                        </td>
                        <td>
                        <asp:Literal ID="lblComponentClass" runat="server" Text="Component Class"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStockClass" runat="server" Width="120px" CssClass="input" ReadOnly="True"
                                Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                        <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtStatus" runat="server" Text="" Width="120px" CssClass="input"
                                ReadOnly="True" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblDeliveryTerms" runat="server" Text="Delivery Terms"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Quick ID="UCDeliveryTerms" AppendDataBoundItems="true" Enabled="false" CssClass="input"
                                runat="server" />
                        </td>
                        <td>
                        <asp:Literal ID="lblPaymentTerms" runat="server" Text="Payment Terms"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Quick ID="UCPaymentTerms" AppendDataBoundItems="true" Enabled="false" CssClass="input"
                                runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblPriority" runat="server" Text="Priority"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Quick ID="UCPeority" AppendDataBoundItems="true" CssClass="input" runat="server" Enabled="False" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Currency ID="ucCurrency" AppendDataBoundItems="true" CssClass="input" Enabled="False"
                                runat="server" />
                        </td>
                        <td>
                            <asp:Literal ID="lblBillTo" runat="server" Text="Bill To"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Company ID="ucPayCompany" AppendDataBoundItems="true" runat="server" Enabled="False" />
                        </td>
                        <td>
                            <%-- Estimate<asp:Label ID="lblCurrencyEstimet" runat="server" BorderWidth="1px" Text="USD"
                                CssClass="input" Visible="False"></asp:Label>--%>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtEstimeted" runat="server" Visible="false" Width="120px" Mask="99,999,999.99"
                                CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVendorEstimate" runat="server" Text="Vendor Estimate"></asp:Literal>
                            <asp:Label ID="lblCurrencyVEstimet" runat="server" BorderWidth="1px" Text="USD" CssClass="input"
                                Visible="False"></asp:Label>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtVenderEsmeted" runat="server" Width="120px" Mask="99,999,999.99"
                                CssClass="input" ReadOnly="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblPartPaid" runat="server" Text="Part Paid"></asp:Literal>
                            <asp:Label ID="lblCurrencyPart" runat="server" BorderWidth="1px" Text="USD" CssClass="input"
                                Visible="False"></asp:Label>
                        </td>
                        <td>
                            <span id="spnPicPartPaid">
                                <eluc:Decimal ID="txtPartPaid" runat="server" Width="100px" CssClass="input" ReadOnly="true" />
                                <asp:ImageButton ID="cmdPicPartPaid" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" Text=".." />
                            </span>
                        </td>
                        <td>
                            <asp:Literal ID="lblFinalTotal" runat="server" Text="Final Total"></asp:Literal>
                            <asp:Label ID="lblCurrencyTotal" runat="server" BorderWidth="1px" Text="USD" CssClass="input"
                                Visible="False"></asp:Label>
                        </td>
                        <td>
                            <eluc:Decimal ID="txtFinalTotal" runat="server" Width="90px" Mask="99,999,999.99"
                                ReadOnly="true" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblFormCreatedBy" runat="server" Text="Form Created By"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFormCreatedBy" runat="server" Width="120px" CssClass="input"
                                Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblPOOrderedBy" runat="server" Text="PO Ordered By"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPOorderedBy" runat="server" Width="120px" CssClass="input" Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblPurchaseApprovedBy" runat="server" Text="Purchase Approved By"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPurchaseAppovedBy" runat="server" Width="120px" CssClass="input"
                                Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%-- Requisition Approved By--%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReqApprovedBy" runat="server" Visible="false" Width="120px" CssClass="input"
                                Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <%-- Accumulated Budget--%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccumulatedBudget" Visible="false" runat="server" Width="90px"
                                Style="text-align: right" CssClass="input" Enabled="False"></asp:TextBox>
                        </td>
                        <td>
                            <%--Accumulated Total--%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccumulatedTotal" runat="server" Width="90px" Style="text-align: right"
                                CssClass="input" Visible="false" Enabled="False"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender4" runat="server" TargetControlID="txtAccumulatedTotal"
                                Mask="9,99,999.99" MaskType="Number" InputDirection="RightToLeft">
                            </ajaxToolkit:MaskedEditExtender>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <eluc:Status runat="server" ID="ucStatus" />
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
