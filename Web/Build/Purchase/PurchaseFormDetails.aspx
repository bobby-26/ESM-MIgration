<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormDetails.aspx.cs" Inherits="Purchase_PurchaseFormDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudget.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OwnerBudgetCode" Src="~/UserControls/UserControlOwnerBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnVendorKeyPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Form</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmOrderForm" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlFormGeneral" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlFormGeneral">
            <eluc:TabStrip ID="MenuFormGeneral" Title="Form Details" runat="server" OnTabStripCommand="MenuFormGeneral_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                    </td>
                    <td width="30%">
                        <telerik:RadTextBox ID="txtFormNumber" runat="server" Width="187px" CssClass="input_mandatory"
                            Enabled="False"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFormTypeCaption" runat="server" Text="Form Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtType" runat="server" Text=" " Width="120px" CssClass="readonlytextbox"
                            Enabled="False"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStockType" runat="server" Text="Stock Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlStockType" Width="123px" CssClass="readonlytextbox">
                            <Items>
                            <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" Selected="True"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Spares" Value="SPARE"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Stores" Value="STORE"></telerik:RadComboBoxItem>
                            <telerik:RadComboBoxItem Text="Service" Value="SERVICE"></telerik:RadComboBoxItem>
                                </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFromTitle" runat="server" Width="187px" CssClass="input_mandatory"
                            MaxLength="50"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCreated" runat="server" Text="Created"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtCreatedDate" runat="server" CssClass="readonlytextbox" Width="120px" DatePicker="true" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOrdered" runat="server" Text="Ordered"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtOrderDate" runat="server" CssClass="readonlytextbox" Width="120px" DatePicker="true" Enabled="false" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                    </td>
                    <td>
                       <span>
                            <eluc:MultiAddress ID="ucMultiVendor" AddressType="130,131,132" runat="server" CssClass="input" Width="200px" />
                        
                        <asp:Image runat="server" ID="cmdvendorAddress" ImageUrl="<%$ PhoenixTheme:images/supplier-address.png %>"
                            ToolTip="Address" Style="display: inline-block; z-index: 20; margin-left: -12%; position: relative;"></asp:Image>
                           </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblImportedDate" runat="server" Text="Imported"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtRecivedDate" runat="server" CssClass="input" Width="120px" DatePicker="true" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblConfirmed" runat="server" Text="Confirmed"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtConfirmDate" runat="server" CssClass="input" Width="120px" DatePicker="true"  />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeliveryLocation" runat="server" Text="Delivery Location"></telerik:RadLabel>
                    </td>
                    <td>
                            <eluc:Multiport ID="ucMultiPortAddress" AddressType='<%# ((int)PhoenixAddressType.PRINCIPAL).ToString() %>' runat="server" CssClass="input" Width="200px" />

                        <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>" Style="display: inline-block; margin-left: -12%; position: relative;" ToolTip="Clear"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdClear_Click" Visible="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBudget" runat="server" Text="Budget"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtBugetDate" runat="server" CssClass="input" Width="120px" DatePicker="true"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDelivered" runat="server" Text="Delivered"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtlLastDeliveryDate" runat="server" CssClass="input" Width="120px" DatePicker="true"  />
                        <telerik:RadLabel ID="lblFormType" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeliveryAddress" runat="server" Text="Delivery Address"></telerik:RadLabel>
                    </td>
                    <td>
                         <span>
                            <eluc:MultiAddress ID="ucMultiDeliveryAddress" AddressType="141" runat="server" CssClass="input" Width="200px" />

                        <asp:ImageButton ID="imgClear2" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>" ToolTip="Clear"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdClearAddress_Click" />

                        <asp:Image runat="server" ID="cmdDeliveryAddress" ImageUrl="<%$ PhoenixTheme:images/supplier-address.png %>"
                            ToolTip="Address" ></asp:Image>
                             </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblApproved" runat="server" Text="Approved"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtApproveDate" runat="server" CssClass="readonlytextbox" Width="120px" DatePicker="true" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReceived" runat="server" Text="Received"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtVenderDelveryDate" runat="server" CssClass="input" Width="120px" DatePicker="true" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                         <span>
                        <eluc:BudgetCode ID="ucBudgetCode" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="190px" AutoPostBack="true" OnTextChangedEvent="ucBudgetCode_TextChangedEvent" />
                        <asp:ImageButton ID="imgClear1" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>" ToolTip="Clear"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdClearBudget_Click" />
                             </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblComponentClass" runat="server" Text=" Component Class "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ddlComponentClass" runat="server" CssClass="input" Width="123px" AppendDataBoundItems="true" />
                        <eluc:Hard ID="ddlStockClassType" runat="server" CssClass="input" Width="123px" AppendDataBoundItems="true"
                            Visible="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtStatus" runat="server" Text="" Width="120px" CssClass="readonlytextbox"
                            ReadOnly="True" Enabled="False"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAgentAddress" runat="server" Text="Agent Address"></telerik:RadLabel>
                    </td>
                    <td>
                         <span>
                            <eluc:MultiAddress ID="ucMultiAgentAddress" AddressType="135" runat="server" CssClass="input" Width="200px" />

                        <asp:Image runat="server" ID="cmdForwarderAddress" ImageUrl="<%$ PhoenixTheme:images/supplier-address.png %>"
                            ToolTip="Address" ></asp:Image>
                             </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPaymentTerms" runat="server" Text="Payment Terms"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="UCPaymentTerms" AppendDataBoundItems="true" Width="123px" Enabled="false" CssClass="readonlytextbox"
                            runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="UCPriority" AppendDataBoundItems="true" Width="123px" CssClass="input" runat="server" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOwnerbudgetCode" runat="server" Text="Owner budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                         <span>
                        <eluc:OwnerBudgetCode ID="ucOwnerBudgetCode" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="190px" />
                        <asp:ImageButton ID="imgClearOwnerBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>" ToolTip="Clear"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdClearOwnerBudget_Click" />
                             </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDeliveryTerms" runat="server" Text="Delivery Terms"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="UCDeliveryTerms" AppendDataBoundItems="true" Width="123px" Enabled="false" CssClass="readonlytextbox"
                            runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBillTo" runat="server" Text="Bill To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucPayCompany" AppendDataBoundItems="true" Width="123px" runat="server" CssClass="input_mandatory" />
                        <telerik:RadLabel ID="lblBillToCompanyName" runat="server" Text=""></telerik:RadLabel>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReason4Requisition" runat="server" Text="Reason for Requisition"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucReason4Requisition" runat="server" CssClass="input"
                            AppendDataBoundItems="true" QuickTypeCode="147" Width="190px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPartPaid" runat="server">  Part Paid  </telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPicPartPaid">
                            <eluc:Decimal ID="txtPartPaid" runat="server" Width="100px" CssClass="input" ReadOnly="true" />
                            <asp:ImageButton ID="cmdPicPartPaid" runat="server" OnClick="cmdPicPartPaid_Click" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFinalTotal" runat="server">   Final Total</telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtFinalTotal" runat="server" Width="120px" Mask="99,999,999.99"
                            ReadOnly="true" CssClass="readonlytextbox" />
                    </td>
                </tr>

                <tr>

                    <td>
                        <telerik:RadLabel ID="lblStandardComments" runat="server" Text="PO Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucPOStatus" runat="server" CssClass="input"
                            AppendDataBoundItems="true" QuickTypeCode="150" Width="123px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlVesselAccount" runat="server" CssClass="input" TextField="FLDDESCRIPTION" AutoPostBack="true"
                            ValueField="FLDACCOUNTID" OnTextChanged="ddlVeselAccount_TextChanged" Width="123px">
                        </telerik:RadComboBox>
                        <asp:HiddenField ID="hdnPrincipalId" runat="server" />
                    </td>

                </tr>

                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ucCurrency" AppendDataBoundItems="true" Width="190" CssClass="readonlytextbox" Enabled="False"
                            runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceNo" runat="server" Text="Invoice No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtInvoiceNo" CssClass="readonlytextbox" Width="120px"
                            ReadOnly="true"></telerik:RadTextBox>
                        <eluc:Decimal ID="txtEstimeted" runat="server" Visible="false" Width="120px" Mask="99,999,999.99"
                            CssClass="input" />
                        <%--<asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgPPClip" runat="server" />--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text="Invoice Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtInvoiceStatus" CssClass="readonlytextbox" Width="120px"
                            ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr id="trPay" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblVenderEsmeted" runat="server">  Vendor Estimate </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtVenderEsmeted" runat="server" Width="187px" Mask="99,999,999.99"
                            CssClass="input" ReadOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceAmount" runat="server" Text="Invoice Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtInvoiceAmount" runat="server" Width="120px" CssClass="input"
                            ReadOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceCurrency" runat="server" Text="Invoice Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtInvoiceCurrency" CssClass="input" Width="120px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFormCreatedBy" runat="server" Text="Form Created By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFormCreatedBy" runat="server" Width="187px" CssClass="input"
                            Enabled="False"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPOOrderedBy" runat="server" Text="PO Ordered By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPOorderedBy" runat="server" Width="120px" CssClass="readonlytextbox" Enabled="False"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPurchaseApprovedBy" runat="server" Text="Purchase Approved By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPurchaseAppovedBy" runat="server" Width="120px" CssClass="readonlytextbox"
                            Enabled="False"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <%-- Requisition Approved By--%>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReqApprovedBy" runat="server" Visible="false" Width="120px" CssClass="input"
                            Enabled="False"></telerik:RadTextBox>
                    </td>
                    <td>
                        <%-- Accumulated Budget--%>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAccumulatedBudget" Visible="false" runat="server" Width="90px"
                            Style="text-align: right" CssClass="input" Enabled="False"></telerik:RadTextBox>
                    </td>
                    <td>
                        <%--Accumulated Total--%>
                    </td>
                    <td>                        
                        <telerik:RadTextBox ID="txtAccumulatedTotal" runat="server" Width="90px" Style="text-align: right"
                            CssClass="input" Visible="false" Enabled="False"></telerik:RadTextBox>  
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
