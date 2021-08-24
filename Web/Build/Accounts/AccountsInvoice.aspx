<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoice.aspx.cs"
    Inherits="AccountsInvoice" %>

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
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript">
            function SetPONumber(pickspan, url) {
                if (document.getElementById(pickspan) != null) {
                    var elem = document.getElementById(pickspan).childNodes;
                    var args = 'ponumber=';
                    for (var i = 0; i < elem.length; i++) {
                        if (elem[i].type == 'text') {
                            args += elem[i].value;
                        }
                    }
                    parent.Openpopup('codehelp1', '', url + args);
                    return;
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoice" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <asp:Button runat="server" Visible="false" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <eluc:TabStrip ID="MenuInvoice1" runat="server" OnTabStripCommand="Invoice_TabStripCommand"></eluc:TabStrip>
                <eluc:TabStrip ID="MenuInvoice2" runat="server" OnTabStripCommand="Invoice_TabStripCommand"></eluc:TabStrip>

                <%--<div class="navSelect1" style="right: 0px; position: relative;">
                    <eluc:TabStrip ID="MenuInvoice2" runat="server" OnTabStripCommand="Invoice_TabStripCommand">
                    </eluc:TabStrip>
                </div>--%>
                <div class="navSelect3" style="position: relative;">
                    <table cellpadding="2" cellspacing="1" style="width: 100%">
                        <tr>
                            <td colspan="5">
                                <asp:HyperLink ID="HlinkRefDuplicate" runat="server" Text="Possible Duplicate Invoices exist for this Supplier. Click here to view the Invoice List "
                                    ToolTip="Vendor Invoice Duplicate" Visible="False" Font-Bold="False" Font-Size="Large"
                                    Font-Underline="True" ForeColor="Red" BorderColor="Red"></asp:HyperLink>
                                <asp:Label ID="lblMismatch" runat="server" Text="Supplier/Currency mismatched" Visible="false" Font-Bold="false" Font-Size="Large"
                                    Font-Underline="true" ForeColor="Red" BorderColor="Red"></asp:Label>
                            </td>
                        </tr>
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
                                <eluc:ToolTip ID="ucToolTipStatus" runat="server" TargetControlId="txtStatus" Width="300px" />
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
                                <eluc:UserControlDate ID="txtInvoiceReceivedDateEdit" runat="server" CssClass="input_mandatory" />
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
                                <eluc:UserControlDate ID="txtInvoiceDateEdit" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVendorInvoiceNumber" runat="server" Text="Vendor Invoice Number"></telerik:RadLabel>
                            </td>
                            <td colspan="2">
                                <telerik:RadTextBox ID="txtSupplierRefEdit" runat="server" CssClass="input_mandatory" MaxLength="200"
                                    Width="293px">
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
                                    Width="300px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblDispatchStatus" runat="server" Text="Dispatch Status"></telerik:RadLabel>
                            </td>
                            <td colspan="2">
                                <telerik:RadTextBox ID="txtDispatchstatus" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="293px">
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
                                    CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    OnTextChangedEvent="Invoice_SetExchangeRate" />
                                <eluc:Number ID="ucInvoiceAmoutEdit" runat="server" CssClass="input_mandatory txtNumber"
                                    DecimalPlace="2" IsPositive="true" Width="120px"></eluc:Number>

                                <%-- <telerik:RadTextBox ID="txtInvoiceAmoutEdit" runat="server" CssClass="input_mandatory txtNumber"
                                    Width="120px"></telerik:RadTextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditInvoiceAmout" runat="server" AutoComplete="true"
                                    InputDirection="RightToLeft" Mask="999,999,999,999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtInvoiceAmoutEdit" />--%>
                                <eluc:Number ID="txtExchangeRateEdit" runat="server" CssClass="readonlytextbox txtNumber"
                                    Visible="false" Wrap="False" Width="150px" ReadOnly="true"></eluc:Number>

                                <span id="spnPickListMaker0">
                                    <telerik:RadTextBox ID="txtInvoiceAdjustmentAmount" runat="server" CssClass="input" ReadOnly="false"
                                        Visible="False" Width="60px">
                                    </telerik:RadTextBox>
                                </span>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblLiabilityCompany" runat="server" Text="Liability Company"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlCompany ID="ddlLiabilitycompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                    Readonly="true" CssClass="input" runat="server" AppendDataBoundItems="true" />
                                <telerik:RadLabel ID="lblBillToCompanyName" runat="server" Text=""></telerik:RadLabel>
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
                                    CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" />
                                &nbsp;
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblEarMarkedCompany" runat="server" Text="Ear Marked Company"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlCompany ID="ddlEarmarkedCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                    CssClass="input" runat="server" AppendDataBoundItems="true" />
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="3" Width="240px"
                                    CssClass="input">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtDTKey" runat="server" Visible="false"></telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtReasonHolding" runat="server" Visible="false" Width="0px"></telerik:RadTextBox>
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
                        <tr valign="top">
                            <td>
                                <telerik:RadLabel ID="lblAccountsVoucherNumber" runat="server" Text="Accounts Voucher Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPurchaseInvoiceVoucherNumber" runat="server" CssClass="readonlytextbox"
                                    MaxLength="100" ReadOnly="true" Width="400px">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPONumber" runat="server" Text="PO Number"></telerik:RadLabel>
                            </td>
                            <td colspan="2">
                                <span id="spanponumber">
                                    <telerik:RadTextBox ID="txtPONumber" runat="server" CssClass="input" MaxLength="100" Width="150px"></telerik:RadTextBox>
                                    <asp:ImageButton ID="ibtnInvoiceTypeByPO" runat="server" AlternateText="Query the Invoice Type Based on PO Number"
                                        ImageUrl="<%$ PhoenixTheme:images/tableviewobservation.png%>" CommandName="DISCOUNTUPDATEFORALL"
                                        Style="vertical-align: middle; padding-bottom: 3px;" OnClientClick="SetPONumber('spanponumber','../Accounts/AccountsInvoiceTypeByPO.aspx?')" />
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBankInformationPageNumber" runat="server" Text="Bank Information Page Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Number ID="ucBankInfoPageNumber" runat="server" CssClass="input txtNumber"
                                    MaxLength="3" Width="20px"></eluc:Number>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPriorityInvoice" runat="server" Text="Priority Invoice"></telerik:RadLabel>
                            </td>
                            <td colspan="2">
                                <input type="checkbox" id="chkPriorityInv" value="0" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblETA" runat="server" Text="ETA"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtETA" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblETD" runat="server" Text="ETD"></telerik:RadLabel>
                            </td>
                            <td colspan="2">
                                <eluc:Date ID="txtETD" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                            </td>
                            <td colspan="3">
                                <eluc:Multiport ID="ucPortMulti" runat="server" CssClass="input" Width="400px" />
                                <%-- <div id="divFleet" runat="server" class="input" style="overflow: auto; width: 60%;
                                    height: 80px">
                                    <eluc:Multiport ID="ucPortMulti" runat="server" CssClass="input" Width="400px" />
                                    <asp:CheckBoxList ID="chkPortList" runat="server" AutoPostBack="true" Height="100%"
                                        RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                    </asp:CheckBoxList>
                                </div>--%>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
            </div>
        </telerik:RadAjaxPanel>
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
