<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceAdjustment.aspx.cs"
    Inherits="AccountsInvoiceAdjustment" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tax" Src="~/UserControls/UserControlTaxMaster.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoice" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <eluc:TabStrip ID="MenuInvoice1" runat="server" OnTabStripCommand="Invoice_TabStripCommand"></eluc:TabStrip>
                <div class="navSelect3" style="position: relative;">
                    <table cellpadding="2" cellspacing="1" style="width: 100%">
                        <div id="RefDuplicate" style="top: 0px; right: 0px; position: absolute;" visible="false">
                            <tr>
                                <td colspan="4">
                                    <asp:HyperLink ID="HlinkRefDuplicate" runat="server" Text="Possible Duplicate Invoices exist for this Supplier. Click here to view the Invoice List"
                                        ToolTip="Attachments" Visible="False" Font-Bold="False" Font-Size="Large" Font-Underline="True"
                                        ForeColor="Red" BorderColor="Red"></asp:HyperLink>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:HyperLink ID="HlinkClassG" runat="server" Text="Urgent to Clear"
                                        Visible="False" Font-Bold="False" Font-Size="Large" Font-Underline="True"
                                        ForeColor="Red" BorderColor="Red"></asp:HyperLink>
                                </td>
                            </tr>
                        </div>
                        <tr>
                            <td width="15%">
                                <telerik:RadLabel ID="lblESMRegisterNumber" runat="server" Text="Register Number"></telerik:RadLabel>
                            </td>
                            <td width="35%">
                                <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" MaxLength="25" ReadOnly="true"
                                    CssClass="readonlytextbox" Width="150px">
                                </telerik:RadTextBox>
                                <asp:HyperLink ID="imgAttachment" runat="server" ImageUrl="<%$ PhoenixTheme:images/attachment.png%>"
                                    Visible="false" ToolTip="Attachments"></asp:HyperLink>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtStatus" runat="server" TextMode="SingleLine" ReadOnly="true"
                                    CssClass="readonlytextbox" Width="150px">
                                </telerik:RadTextBox>
                                <eluc:ToolTip ID="ucToolTipStatus" runat="server" Width="300px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                        <tr valign="top">
                            <td width="15%">
                                <telerik:RadLabel ID="lblDateReceived" runat="server" Text="Date Received"></telerik:RadLabel>
                            </td>
                            <td width="35%">
                                <eluc:UserControlDate ID="txtInvoiceReceivedDateEdit" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                            </td>
                            <td>
                                <span id="spnPickListMaker">
                                    <telerik:RadTextBox ID="txtVendorCode" runat="server" Width="90px" CssClass="input_mandatory"
                                        ReadOnly="false">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtVenderName" runat="server" Width="210px" CssClass="input_mandatory"
                                        ReadOnly="false">
                                    </telerik:RadTextBox>
                                    <img id="ImgSupplierPickList" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                        style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                                    <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlDate ID="txtInvoiceDateEdit" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVendorInvoiceNumber" runat="server" Text="Vendor Invoice Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtSupplierRefEdit" runat="server" CssClass="input_mandatory" MaxLength="200"
                                    Width="240px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Hard ID="ddlInvoiceType" runat="server" CssClass="dropdown_mandatory" HardTypeCode="59"
                                    HardList='<%# PhoenixRegistersHard.ListHard(1, 59) %>' AppendDataBoundItems="true"
                                    Width="300px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblAdvancepayment" runat="server" Text="Advance payment"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTotalAmount" runat="server" Style="text-align: right;" CssClass="input" Enabled="false"
                                    MaxLength="25" Width="240px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                            </td>
                            <td>
                                <%--     <asp:TextBox ID="txtInvoiceAmoutEdit" runat="server" CssClass="input_mandatory txtNumber"
                                    Width="120px"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditInvoiceAmout" runat="server" AutoComplete="true"
                                    InputDirection="RightToLeft" Mask="999,999,999,999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtInvoiceAmoutEdit" />--%>
                                <eluc:Number ID="ucInvoiceAmoutEdit" runat="server" CssClass="input_mandatory txtNumber"
                                    DecimalPlace="2" IsPositive="true" Width="150px"></eluc:Number>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCurrencyExchangeRate" runat="server" Text="Currency Exchange Rate"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCurrency" runat="server" Width="82px" CssClass="readonlytextbox txtCurreny"
                                    ReadOnly="true" Enabled="False">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtExchangeRateEdit" runat="server" CssClass="readonlytextbox txtNumber"
                                    Wrap="False" Width="150px" ReadOnly="true">
                                </telerik:RadTextBox>
                                <%--<ajaxToolkit:MaskedEditExtender ID="MaskedEditExchangeRate" runat="server" AutoComplete="true"
                                    InputDirection="RightToLeft" Mask="9,999.999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtExchangeRateEdit" /> --%>
                                <eluc:UserControlCurrency ID="ddlCurrencyCode" runat="server" AppendDataBoundItems="true"
                                    AutoPostBack="true" CssClass="dropdown_mandatory" CurrencyList="<%# PhoenixRegistersCurrency.ListCurrency(1)%>"
                                    Enabled="false" OnTextChangedEvent="Invoice_SetExchangeRate" Visible="False" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAdjustmentAmount" runat="server" Text="Adjustment Amount"></telerik:RadLabel>
                            </td>
                            <td>
                                <%--<asp:TextBox ID="txtAdjustmentAmount" runat="server" CssClass="input txtNumber" Width="150px"
                                    OnTextChanged="InvoiceAdjustmentAmountChanged" AutoPostBack="true"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AutoComplete="true"
                                    InputDirection="RightToLeft" Mask="999,999,999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtAdjustmentAmount" />--%>
                                <eluc:Number ID="ucAdjustmentAmount" runat="server" CssClass="input txtNumber" DecimalPlace="2"
                                    IsPositive="true" Width="150px"></eluc:Number>
                                <asp:ImageButton ID="imgAdjustmentAttachment" runat="server" AlternateText="Attachment"
                                    ImageUrl="<%$ PhoenixTheme:images/attachment.png%>" ToolTip="Attachment" />
                            </td>
                            <td>
                                <asp:Literal ID="lblReasonforHolding" runat="server" Text="Present Status"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Quick ID="ddlReason" runat="server" CssClass="input" QuickTypeCode="58" QuickList='<%# PhoenixRegistersQuick.ListQuick(1, 58) %>'
                                    AppendDataBoundItems="true" Width="300px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPayableAmount" runat="server" Text="Payable Amount"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTotalPayableAmoutEdit" runat="server" CssClass="readonlytextbox txtNumber"
                                    Width="150px" ReadOnly="true">
                                </telerik:RadTextBox>
                                <%--<ajaxToolkit:MaskedEditExtender ID="MaskedEditTotalPayableAmout" runat="server" AutoComplete="true"
                                    InputDirection="RightToLeft" Mask="999,999,999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtTotalPayableAmoutEdit" /> --%>
                                
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblLiabilityCompany" runat="server" Text="Liability Company"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlCompany ID="ddlLiabilitycompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                    Readonly="true" CssClass="input" runat="server" AppendDataBoundItems="true" />
                                <asp:Literal ID="lblBillToCompanyName" runat="server" Text=""></asp:Literal>
                                <telerik:RadTextBox ID="TextBox1" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px" Visible="false">
                                </telerik:RadTextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <telerik:RadLabel ID="lblPOPayableAmount" runat="server" Text="PO Payable Amount"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPOPaybleAmount" runat="server" Style="text-align: right;" CssClass="readonlytextbox txtPOPaybleAmount"
                                    ReadOnly="True" Width="180px">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <%--Discount Amount--%>
                                <telerik:RadLabel ID="lblDiscount" runat="server" Text="Discount Amount"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTotalDiscountAmount" runat="server" Style="text-align: right;" CssClass="readonlytextbox txtNumber"
                                    ReadOnly="true" Width="150px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <%--<tr>
                            <td>
                                Physical Location
                            </td>
                            <td>
                                <eluc:UserControlCompany ID="ddlPhysicalLocation" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                    CssClass="input" runat="server" AppendDataBoundItems="true" />
                                &nbsp;
                            </td>
                            <td>
                                Ear Marked Company
                            </td>
                            <td>
                                <eluc:UserControlCompany ID="ddlEarmarkedCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                    CssClass="input" runat="server" AppendDataBoundItems="true" />
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                           
                            <td></td>
                            <td></td>
                           
                        </tr>--%>
                        <tr valign="top">
                            <td>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="3" Width="240px"
                                    CssClass="input">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtDTKey" runat="server" Visible="false"></telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtTotalGSTAmount" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="150px" Visible="false">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblAdjustmentRemarks" runat="server" Text="Adjustment Remarks"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtAdjustmentRemarks" runat="server" CssClass="input" Rows="3" TextMode="MultiLine"
                                    Width="240px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPriorityInvoice" runat="server" Text="Priority Invoice"></telerik:RadLabel>
                            </td>
                            <td>
                                <input type="checkbox" id="chkPriorityInv" value="0" runat="server" />
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <br />
            </div>
        </telerik:RadAjaxPanel>
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
