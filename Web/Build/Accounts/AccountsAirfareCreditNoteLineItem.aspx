<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAirfareCreditNoteLineItem.aspx.cs" Inherits="AccountsAirfareCreditNoteLineItem" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInvoice" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlVoucher">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="ttlVoucher" Text="Voucher" ShowMenu="false"></eluc:Title>
                </div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuVoucherLineItem" OnTabStripCommand="Voucher_TabStripCommand"
                        runat="server"></eluc:TabStrip>
                </div>
                <div>
                    <table cellpadding="2" cellspacing="1" style="width: 100%">
                        <tr>
                            <td width="15%">
                                <asp:Literal ID="lblVoucherNumber" runat="server" Text="Credit Note"></asp:Literal>
                            </td>
                            <td width="35%">
                                <asp:TextBox ID="txtVoucherNumber" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                    Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblRowNumber" runat="server" Text="Row Number"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRowNumber" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                    Width="50px"></asp:TextBox>
                            </td>
                            <td rowspan="2">
                                 <asp:Literal ID="lblAccount" runat="server" Text="Account"></asp:Literal>
                                <br>                               
                                <asp:Literal ID="lblSourceUsage" runat="server" Text="Source/Usage"></asp:Literal>
                                <br />
                                <asp:Literal ID="lblSubAccount" runat="server" Text="Sub Account"></asp:Literal> </br> 
                            </td>
                            <td rowspan="2">
                                <span id="spnPickListExpenseAccount">
                                    <asp:TextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                         MaxLength="20" Width="30%" OnTextChanged="txtAccountCode_changed"></asp:TextBox>
                                    <asp:TextBox ID="txtAccountDescription" runat="server" CssClass="input_mandatory"
                                         ReadOnly="false" MaxLength="50"
                                        Width="55%"></asp:TextBox>
                                    <asp:ImageButton runat="server" id="imgShowAccount" style="cursor: pointer; vertical-align: top"
                                        ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListAccount.aspx',true); " />
                                    <asp:TextBox ID="txtAccountId" runat="server" CssClass="input" MaxLength="20" Width="10px"></asp:TextBox>
                                    <asp:TextBox ID="txtAccountSource" CssClass="readonlytextbox" runat="server"></asp:TextBox>
                                    /
                                    <asp:TextBox ID="txtAccountUsage" CssClass="readonlytextbox" runat="server"></asp:TextBox>
                                    <br />
                                    <asp:TextBox ID="txtBudgetCode" runat="server" Width="60px" CssClass="input_mandatory"></asp:TextBox>
                                    <asp:TextBox ID="txtBudgetName" runat="server" Width="180px" CssClass="input_mandatory"></asp:TextBox>
                                    <asp:ImageButton ID="imgShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" OnClientClick="return showSubAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListSubAccount.aspx',true); " />
                                    <asp:TextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input_mandatory"></asp:TextBox>
                                    <asp:TextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input_mandatory"></asp:TextBox>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblChequeRemittanceNo" runat="server" Text="Cheque/Remittance No"></asp:Literal></td>
                            <td>
                                <asp:TextBox ID="txtChequeno" runat="server" CssClass="input" MaxLength="100" 
                                    Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                            </td>
                            <td>
                                <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                    CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                    OnTextChangedEvent="Voucher_SetExchangeRate" />
                            </td>
                            <td>
                                <asp:Literal ID="lblPrimeAmount" runat="server" Text="Prime Amount"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtPrimeAmoutEdit" runat="server" CssClass="input_mandatory txtNumber"
                                    Width="120px"></eluc:Number>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblBaseExchangeRate" runat="server" Text="Base Exchange Rate"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtExchangeRate" runat="server" CssClass="input_mandatory txtNumber"
                                    Width="120px" AutoPostBack="true" OnTextChanged="txtExchangeRate_TextChanged"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AutoComplete="true"
                                    InputDirection="RightToLeft" Mask="99999.99999999999999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtExchangeRate" />
                            </td>
                            <td>
                                <asp:Literal ID="lblReportExchangeRate" runat="server" Text="Report Exchange Rate"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txt2ndExchangeRate" runat="server" CssClass="input_mandatory txtNumber"
                                    Width="120px" AutoPostBack="true" OnTextChanged="txt2ndExchangeRate_TextChanged"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" AutoComplete="true"
                                    InputDirection="RightToLeft" Mask="99999.99999999999999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txt2ndExchangeRate" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblLongDescription" runat="server" Text="Long Description"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtLongDescription" Width="270px" TextMode="MultiLine"
                                    Height="50px" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblUpdatedBy" runat="server" Text="Updated By"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUpdatedBy" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                    Width="160px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblUpdatedDate" runat="server" Text="Updated Date"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUpdatedDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                    Width="160px"></asp:TextBox>
                            </td>
                        <tr />
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
