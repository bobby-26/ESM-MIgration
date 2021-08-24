<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOffSettingEntriesAdd.aspx.cs"
    Inherits="AccountsOffSettingEntriesAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Off-Setting Line Item Add</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmOffSettingAdd" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAdvancePaymen">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="ttlCreateVoucherAdd" Text="Contra Voucher Line Item" ShowMenu="false">
                        </eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuOffSettingAdd" runat="server" OnTabStripCommand="MenuOffSettingAdd_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="MenuContraVoucher" runat="server" OnTabStripCommand="MenuContraVoucher_TabStripCommand">
                        </eluc:TabStrip>
                    </span>
                </div>
                <table cellpadding="2" cellspacing="1" style="width: 100%">
                    <tr>
                        <td rowspan="2">
                            <asp:Literal ID="lblAccount" runat="server" Text="Account"></asp:Literal>
                            <br><asp:Literal ID="lblSourceUsage" runat="server" Text="Source/Usage"></asp:Literal>
                                <br />
                                <asp:Literal ID="lblSubAccount" runat="server" Text="Sub Account"></asp:Literal> </br>
                        </td>
                        <td rowspan="2">
                            <span id="spnPickListExpenseAccount">
                                <asp:TextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                    OnTextChanged="txtAccountCode_changed" AutoPostBack="true" MaxLength="20" Width="30%"></asp:TextBox>
                                <asp:TextBox ID="txtAccountDescription" runat="server" CssClass="input_mandatory"
                                    OnTextChanged="txtAccountCode_changed" AutoPostBack="true" ReadOnly="false" MaxLength="50"
                                    Width="55%"></asp:TextBox>
                                <img runat="server" id="imgShowAccount" style="cursor: pointer; vertical-align: top"
                                    src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showAccountPickList('spnPickListExpenseAccount', 'codehelp1', '', '../Common/CommonPickListAccount.aspx',true); " />
                                <asp:TextBox ID="txtAccountId" runat="server" CssClass="input" MaxLength="20" Width="10px"></asp:TextBox>
                                <asp:TextBox ID="txtAccountSource" CssClass="readonlytextbox" runat="server"></asp:TextBox>
                                /
                                <asp:TextBox ID="txtAccountUsage" CssClass="readonlytextbox" runat="server"></asp:TextBox>
                                <br />
                                <asp:TextBox ID="txtBudgetCode" runat="server" Width="60px" CssClass="input_mandatory"></asp:TextBox>
                                <asp:TextBox ID="txtBudgetName" runat="server" Width="180px" CssClass="input_mandatory"></asp:TextBox>
                                <asp:ImageButton ID="btnShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" Text=".." />
                                <asp:TextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="input_mandatory"></asp:TextBox>
                                <asp:TextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="input_mandatory"></asp:TextBox>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                        </td>
                        <td>
                            <eluc:UserControlCurrency ID="ddlCurrencyCode" runat="server" AppendDataBoundItems="true"
                                AutoPostBack="true" CssClass="dropdown_mandatory" CurrencyList="<%# PhoenixRegistersCurrency.ListCurrency(1)%>"
                                Enabled="false" Visible="True" OnTextChangedEvent="Voucher_SetExchangeRate" />
                            <%-- <asp:TextBox ID="txtCurrencyID" runat="server" CssClass="input_mandatory" 
                                ReadOnly="true"></asp:TextBox>--%>
                        </td>
                        <td>
                            <asp:Literal ID="lblPrimeAmount" runat="server" Text="Prime Amount"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="input_mandatory txtNumber" Width="120px"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditPrimeAmout" runat="server" AcceptNegative="Left"
                                AutoComplete="true" InputDirection="RightToLeft" Mask="999,999,999,999.99" MaskType="Number"
                                OnInvalidCssClass="MaskedEditError" TargetControlID="txtAmount" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblBaseExchangeRate" runat="server" Text="Base Exchange Rate"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtExchangeRate" runat="server" CssClass="readonlytextbox" Width="120px"
                                AutoPostBack="true" OnTextChanged="txtBaseRate_TextChanged"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AutoComplete="true"
                                InputDirection="RightToLeft" Mask="999.99999999999999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                TargetControlID="txtExchangeRate" />
                        </td>
                        <td>
                            <asp:Literal ID="lblReportExchangeRate" runat="server" Text="Report Exchange Rate"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txt2ndExchangeRate" runat="server" CssClass="readonlytextbox" Width="120px"
                                AutoPostBack="true" OnTextChanged="txtReportRate_TextChanged"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender2" runat="server" AutoComplete="true"
                                InputDirection="RightToLeft" Mask="999.99999999999999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                TargetControlID="txt2ndExchangeRate" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblLongDescription" runat="server" Text="Long Description"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtLongDescription" TextMode="MultiLine" Width="270px"
                                Height="75px" CssClass="input_mandatory"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
