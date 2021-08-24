<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceRWAForPurchase.aspx.cs"
    Inherits="AccountsInvoiceRWAForPurchase" %>

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
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVesselList" Src="../UserControls/UserControlVesselList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
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
        <telerik:RadAjaxPanel ID="pnlInvoice" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <%--<eluc:Title runat="server" ID="ttlInvoice" Text="Invoice" ShowMenu="false"></eluc:Title> --%>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuInvoice1" runat="server" OnTabStripCommand="Invoice_TabStripCommand" Visible="false"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <div id="RefDuplicate" style="top: 0px; right: 0px; position: absolute;" visible="false">
                    <tr>
                        <td colspan="5">
                            <asp:HyperLink ID="HlinkRefDuplicate" runat="server" Text="Vendor Invoice Number already exists for this Supplier. Click here to view the Invoice List "
                                ToolTip="Vendor Invoice Duplicate" Visible="False" Font-Bold="False" Font-Size="Large"
                                Font-Underline="True" ForeColor="Red" BorderColor="Red"></asp:HyperLink>
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
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtStatus" runat="server" TextMode="SingleLine" ReadOnly="true"
                            CssClass="readonlytextbox" Width="150px">
                        </telerik:RadTextBox>
                        <eluc:ToolTip ID="ucToolTipStatus" runat="server" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr style="height: -15px" />
                    </td>
                </tr>
                <tr valign="top">
                    <td width="15%">
                        <telerik:RadLabel ID="lblDateReceived" runat="server" Text="Date Received"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <eluc:UserControlDate ID="txtInvoiceReceivedDateEdit" runat="server" ReadOnly="true"
                            CssClass="readonlytextbox" />
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                    </td>
                    <td width="35%" colspan="2">
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input" ReadOnly="false"
                                Width="90px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input" ReadOnly="false"
                                Width="200px">
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
                        <eluc:UserControlDate ID="txtInvoiceDateEdit" runat="server" ReadOnly="true" CssClass="readonlytextbox" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVendorInvoiceNumber" runat="server" Text="Vendor Invoice Number"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtSupplierRefEdit" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            MaxLength="25" Width="294px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlInvoiceType" runat="server" CssClass="dropdown_mandatory" HardTypeCode="59"
                            AutoPostBack="TRUE" HardList='<%# PhoenixRegistersHard.ListHard(1, 59) %>' AppendDataBoundItems="true"
                            OnTextChangedEvent="ddlInvoice_SelectedIndexChanged" Width="300px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDispatchStatus" runat="server" Text="Dispatch Status"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtDispatchstatus" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="294px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <hr style="height: -15px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            Enabled="false" CssClass="readonlytextbox" runat="server" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="Invoice_SetExchangeRate" />
                        <eluc:Number ID="ucInvoiceAmoutEdit" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            DecimalPlace="2" IsPositive="true" Width="120px"></eluc:Number>
                        <%-- <telerik:RadTextBox ID="txtInvoiceAmoutEdit" runat="server" CssClass="input_mandatory txtNumber"
                                    Width="120px"></telerik:RadTextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditInvoiceAmout" runat="server" AutoComplete="true"
                                    InputDirection="RightToLeft" Mask="999,999,999,999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtInvoiceAmoutEdit" />--%>
                        <eluc:Number ID="txtExchangeRateEdit" runat="server" ReadOnly="true" CssClass="readonlytextbox txtNumber"
                            DecimalPlace="4" IsPositive="true" Visible="false" Width="120px"></eluc:Number>
                        <%--<telerik:RadTextBox ID="txtExchangeRateEdit" runat="server" CssClass="readonlytextbox txtNumber"
                            Visible="false" Wrap="False" Width="150px" ReadOnly="true"></telerik:RadTextBox>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExchangeRate" runat="server" AutoComplete="true"
                            InputDirection="RightToLeft" Mask="9,999.999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                            TargetControlID="txtExchangeRateEdit" /> --%>
                        <span id="spnPickListMaker0">
                            <telerik:RadTextBox ID="txtInvoiceAdjustmentAmount" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                Visible="False" Width="60px">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLiabilityCompany" runat="server" Text="Liability Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCompany ID="ddlLiabilitycompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            Enabled="false" CssClass="readonlytextbox" runat="server" AppendDataBoundItems="true" />
                        &nbsp;
                    </td>
                    <td rowspan="3">
                        <div id="dvClass" runat="server" class="input" style="overflow: auto; width: 100%; height: 95px">
                            <asp:CheckBoxList ID="chkVesselList" runat="server" CssClass="input" DataTextField="FLDVESSELNAME"
                                DataValueField="FLDVESSELID" Height="100%" RepeatDirection="Vertical" Width="100%">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPhysicalLocation" runat="server" Text="Physical Location"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCompany ID="ddlPhysicalLocation" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            Enabled="false" CssClass="readonlytextbox" runat="server" AppendDataBoundItems="true" />
                        &nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEarMarkedCompany" runat="server" Text="Ear Marked Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCompany ID="ddlEarmarkedCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            Enabled="false" CssClass="readonlytextbox" runat="server" AppendDataBoundItems="true" />
                        &nbsp;
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="3" Width="240px"
                            ReadOnly="true" CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtDTKey" runat="server" Visible="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselsInvolved" runat="server" Text="Vessels Involved"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselNameList" runat="server" TextMode="MultiLine" Rows="3"
                            ReadOnly="true" Width="240px" CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBankInformationPageNumber" runat="server" Text="Bank Information Page Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucBankInfoPageNumber" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            MaxLength="3" Width="20px"></eluc:Number>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPurchaseInvoiceVoucherNumber" runat="server" Text="Purchase Invoice Voucher Number"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtPurchaseInvoiceVoucherNumber" runat="server" CssClass="readonlytextbox"
                            MaxLength="50" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPriorityInvoice" runat="server" Text="Priority Invoice"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <input type="checkbox" id="chkPriorityInv" value="0" runat="server" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <telerik:RadTextBox ID="txtETA" runat="server" CssClass="input" ReadOnly="false" Visible="False"
                    Width="60px">
                </telerik:RadTextBox>
                <telerik:RadTextBox ID="txtETD" runat="server" CssClass="input" ReadOnly="false" Visible="False"
                    Width="60px">
                </telerik:RadTextBox>
                <telerik:RadTextBox ID="txtPort" runat="server" CssClass="input" ReadOnly="false" Visible="False"
                    Width="60px">
                </telerik:RadTextBox>
            </table>
        </telerik:RadAjaxPanel>
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
