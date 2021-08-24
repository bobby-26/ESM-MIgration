<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCreditDebitLineItem.aspx.cs"
    Inherits="AccountsCreditDebitLineItem" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoice" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlVoucher">
            <asp:Button runat="server" ID="cmdHiddenPick" CssClass="hidden" OnClick="cmdHiddenPick_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuCreditDebitNoteLineItem" OnTabStripCommand="CreditDebitNote_TabStripCommand"
                runat="server"></eluc:TabStrip>

            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblCreditDebitNoteNumber" runat="server" Text="CreditDebitNoteNumber"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <telerik:RadTextBox ID="txtCreditDebitNoteNumber" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td rowspan="3">
                        <telerik:RadLabel ID="lblAccount" runat="server" Text="Account"></telerik:RadLabel>
                        <br />
                        <telerik:RadLabel ID="lblSourceUsage" runat="server" Text="Source/Usage"></telerik:RadLabel>
                        <br />
                        <telerik:RadLabel ID="lblSubAccount" runat="server" Text="Sub Account"></telerik:RadLabel>
                        </br> 
                    </td>
                    <td rowspan="3">
                        <span id="spnPickListExpenseAccount">
                            <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                MaxLength="20" Width="30%" OnTextChanged="txtAccountCode_changed">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="input_mandatory"
                                ReadOnly="false" MaxLength="50"
                                Width="55%">
                            </telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowAccount" Style="cursor: pointer; vertical-align: top"
                                ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" />
                                                                                                        <%--"javascript:Openpopup('filter', '',                            '../Accounts/AccountsBankPaymentVoucherMaster.aspx?type=2&voucherid=" + drv["FLDVOUCHERID"].ToString() + "');return false;");--%>
                            <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountSource" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox>
                            /
                                    <telerik:RadTextBox ID="txtAccountUsage" CssClass="readonlytextbox" runat="server"></telerik:RadTextBox>
                            <br />
                            <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="30%" CssClass="input_mandatory"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="55%" CssClass="input_mandatory"></telerik:RadTextBox>
                            <img runat="server" id="imgShowBudget" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListSubAccount.aspx',true); " />
                            <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input_mandatory" OnTextChanged="txtBudgetId_Changed"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input_mandatory"></telerik:RadTextBox>
                        </span>
                    </td>
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

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblChequeRemittanceNo" runat="server" Text="Cheque/Remittance No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtChequeno" runat="server" CssClass="input" MaxLength="100"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            OnTextChangedEvent="CreditDebitNote_SetExchangeRate" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPrimeAmount" runat="server" Text="Prime Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtPrimeAmoutEdit" runat="server" CssClass="input_mandatory txtNumber"
                            Width="120px"></eluc:Number>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBaseExchangeRate" runat="server" Text="Base Exchange Rate"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtExchangeRate" runat="server" CssClass="input_mandatory txtNumber"
                            Width="150px" AutoPostBack="true" OnTextChanged="txtExchangeRate_TextChanged">
                        </telerik:RadTextBox>
                        <%--<telerik:RadTextBox ID="txtExchangeRate" runat="server" CssClass="input_mandatory txtNumber"
                            Width="120px" AutoPostBack="true" OnTextChanged="txtExchangeRate_TextChanged"></telerik:RadTextBox>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AutoComplete="true"
                            InputDirection="RightToLeft" Mask="99999.99999999999999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                            TargetControlID="txtExchangeRate" />--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportExchangeRate" runat="server" Text="Report Exchange Rate"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txt2ndExchangeRate" runat="server" CssClass="input_mandatory txtNumber"
                            Width="150px" AutoPostBack="true" OnTextChanged="txt2ndExchangeRate_TextChanged">
                        </telerik:RadTextBox>
                        <%-- <telerik:RadTextBox ID="txt2ndExchangeRate" runat="server" CssClass="input_mandatory txtNumber"
                            Width="120px" AutoPostBack="true" OnTextChanged="txt2ndExchangeRate_TextChanged"></telerik:RadTextBox>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" AutoComplete="true"
                            InputDirection="RightToLeft" Mask="99999.99999999999999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                            TargetControlID="txt2ndExchangeRate" />--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedBy" runat="server" Text="Updated By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUpdatedBy" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="160px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUpdatedDate" runat="server" Text="Updated Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUpdatedDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="160px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblvesselid" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblaccountusage" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblbudgetid" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblNotIncludeYNHeader" runat="server" Text="Included in Owner SOA"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIncludeYNEdit" runat="server"></asp:CheckBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblownerbudgetcode" runat="server" Text="Owners Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListOwnerBudgetEdit">
                            <telerik:RadTextBox ID="txtAccountCode1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERACCOUNT") %>'
                                MaxLength="20" CssClass="input" Width="100px">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="imgShowAccount1" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text="..." />
                            <telerik:RadTextBox ID="txtOwnerBudgetNameEdit" runat="server" CssClass="hidden"
                                MaxLength="50" Width="0px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtownerbudgetedit" runat="server" CssClass="hidden" MaxLength="20"
                                Width="0px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="TextBox1" runat="server" CssClass="hidden" MaxLength="20"
                                Width="0px">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLongDescription" runat="server" Text="Long Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLongDescription" Width="270px" TextMode="MultiLine"
                            Height="50px" CssClass="input">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
