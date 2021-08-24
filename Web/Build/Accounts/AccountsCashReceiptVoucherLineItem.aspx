<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCashReceiptVoucherLineItem.aspx.cs"
    Inherits="AccountsCashReceiptVoucherLineItem" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="split" Src="~/usercontrols/usercontrolsplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .rwContent {
                color: red !important;
            }

            .rwTitle {
                font-weight: 600 !important;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoice" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlVoucher">
            <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuVoucherLineItem" OnTabStripCommand="Voucher_TabStripCommand"
                runat="server"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <telerik:RadTextBox ID="txtVoucherNumber" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="270px">
                        </telerik:RadTextBox>
                    </td>
                    <td width="15%"></td>
                    <td width="35%"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRowNumber" runat="server" Text="Row Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRowNumber" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="50px">
                        </telerik:RadTextBox>
                    </td>
                    <td rowspan="2">
                        <telerik:RadLabel ID="lblAccount" runat="server" Text="Account"></telerik:RadLabel>
                        <br>
                        <telerik:RadLabel ID="lblSourceUsage" runat="server" Text="Source/Usage"></telerik:RadLabel>
                        <br />
                        <telerik:RadLabel ID="lblSubAccount" runat="server" Text="Sub Account"></telerik:RadLabel>
                    </td>
                    <td rowspan="2">
                        <span id="spnPickListExpenseAccount">
                            <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                MaxLength="20" Width="30%" OnTextChanged="txtAccountCode_changed">
                            </telerik:RadTextBox>&nbsp;&nbsp;
                            <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="input_mandatory"
                                ReadOnly="false" MaxLength="50" Width="55%">
                            </telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowAccount" Style="cursor: pointer; vertical-align: top" OnClick="cmdHiddenPick_Click"
                                ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', 'Common/CommonPickListAccount.aspx?iframename=true',true); " />
                            <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountSource" CssClass="readonlytextbox" runat="server" Width="30%"></telerik:RadTextBox>
                            /
                            <telerik:RadTextBox ID="txtAccountUsage" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox>
                            <br />
                            <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="30%" CssClass="input"></telerik:RadTextBox>&nbsp;&nbsp;
                            <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="55%" CssClass="input"></telerik:RadTextBox>
                            <asp:ImageButton ID="imgShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClick="cmdHiddenPick_Click"
                                ImageAlign="AbsMiddle" OnClientClick="return showSubAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', 'Common/CommonPickListSubAccount.aspx',true); " />
                            <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input_mandatory" OnTextChanged="txtBudgetId_Changed"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input_mandatory"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblChequeRemittanceNo" runat="server" Text="Cheque/Remittance No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtChequeno" runat="server" CssClass="input" MaxLength="100" Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            OnTextChangedEvent="Voucher_SetExchangeRate" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPrimeAmount" runat="server" Text="Prime Amount"></telerik:RadLabel>
                    </td>
                    <td>

                        <eluc:Decimal ID="txtPrimeAmoutEdit" runat="server" DecimalPlace="2" Width="270px" CssClass="input_mandatory txtNumber" />

                        <%-- <eluc:Number ID="txtPrimeAmoutEdit" runat="server" CssClass="input_mandatory txtNumber"
                            Width="270px"></eluc:Number>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBaseExchangeRate" runat="server" Text="Base Exchange Rate"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtExchangeRate" runat="server" CssClass="input_mandatory txtNumber"
                            Width="270px" AutoPostBack="true" OnTextChanged="txtExchangeRate_TextChanged">
                        </telerik:RadTextBox>
                        <%-- <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AutoComplete="true"
                                        InputDirection="RightToLeft" Mask="99999.99999999999999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                        TargetControlID="txtExchangeRate" />--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportExchangeRate" runat="server" Text="Report Exchange Rate"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txt2ndExchangeRate" runat="server" CssClass="input_mandatory txtNumber"
                            Width="270px" AutoPostBack="true" OnTextChanged="txt2ndExchangeRate_TextChanged">
                        </telerik:RadTextBox>
                        <%--    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" AutoComplete="true"
                                        InputDirection="RightToLeft" Mask="99999.99999999999999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                        TargetControlID="txt2ndExchangeRate" />--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLongDescription" runat="server" Text="Long Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLongDescription" Width="270px" TextMode="MultiLine" Resize="Both"
                            Height="50px" CssClass="input">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedBy" runat="server" Text="Updated By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUpdatedBy" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="270px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedDate" runat="server" Text="Updated Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUpdatedDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="270px">
                        </telerik:RadTextBox>
                        <telerik:RadLabel ID="lblvesselid" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblaccountusage" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblbudgetid" runat="server" Visible="false"></telerik:RadLabel>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSOAreference" runat="server" Text="SOA Reference"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSOAreference" runat="server" Width="270px" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>

                <tr>
                    <td>
                        <br />
                        <telerik:RadLabel ID="lblNotIncludeYNHeader" runat="server">Included in Owner SOA</telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkIncludeYNEdit" runat="server"></telerik:RadCheckBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblownerbudgetcode" runat="server">Owners Budget Code</telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListOwnerBudgetEdit">
                            <telerik:RadTextBox ID="txtAccountCode1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERACCOUNT") %>'
                                MaxLength="20" CssClass="input" Width="100px">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="imgShowAccount1" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text="..." OnClick="cmdHiddenPick_Click" />
                            <telerik:RadTextBox ID="txtOwnerBudgetNameEdit" runat="server" CssClass="hidden"
                                MaxLength="50" Width="0px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtownerbudgetedit" runat="server" CssClass="hidden" MaxLength="20"
                                Width="0px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="TextBox1" runat="server" CssClass="hidden" MaxLength="20"
                                Width="100px">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShowInSummaryBalance" runat="server" Text="Show separately in Vessel Summary Balance"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkShowInSummaryBalance" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks to print in SOA Attachment"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" MaxLength="100" Resize="Both" Width="270px" Height="50px" TextMode="MultiLine"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

