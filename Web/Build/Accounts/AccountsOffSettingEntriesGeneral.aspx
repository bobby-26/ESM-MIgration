<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOffSettingEntriesGeneral.aspx.cs"
    Inherits="AccountsOffSettingEntriesGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register src="../UserControls/UserControlBankAccount.ascx" tagname="UserControlBankAccount" tagprefix="eluc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Off-Setting General</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmOffSettingEntries" runat="server" autocomplete="off">
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
                        <eluc:Title runat="server" ID="ttlCreateVoucherGeneral" Text="General" ShowMenu="false">
                        </eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuOffSettingGeneral" runat="server" OnTabStripCommand="MenuOffSettingGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table cellpadding="2" cellspacing="1" style="width: 100%">
                    <tr>
                        <td colspan="4" align="left">
                            <b><asp:Literal ID="lblEntryToBeOffset" runat="server" Text="Entry To Be Off-set:"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVoucherNumber" runat="server" Text="Voucher Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVoucherNumber" runat="server" MaxLength="50" ReadOnly="true"
                                Width="240px" CssClass="readonlytextbox"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblOffSettingReferenceNumber" runat="server" Text="Reference Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReferenceNo" runat="server" MaxLength="50" ReadOnly="true" Width="240px"
                                CssClass="readonlytextbox"></asp:TextBox>
                        </td>
                        <td rowspan="3">
                            <asp:Literal ID="lblOffSettingLongDescription" runat="server" Text="Long Description"></asp:Literal>
                        </td>
                        <td rowspan="3">
                            <asp:TextBox ID="txtLongDescription" runat="server" CssClass="readonlytextbox" Height="75px"
                                ReadOnly="true" TextMode="MultiLine" Width="270px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                        </td>
                        <td>
                            <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAmount" runat="server" Text="Amount"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmount" runat="server" CssClass="readonlytextbox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblContraVoucher" runat="server" Text="Contra Voucher :"></asp:Literal></b>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVoucherType" runat="server" Text="Voucher Type"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlVoucherType" runat="server" AutoPostBack="True" CssClass="input_mandatory"
                                DataTextField="FLDVOUCHERTYPE" DataValueField="FLDVOUCHERTYPECODE" OnSelectedIndexChanged="ddlVoucherType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="input_mandatory" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblReferenceNumber" runat="server" Text="Reference Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReferenceNumber" runat="server" CssClass="input_mandatory" MaxLength="50"
                                Width="240px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblBankAccount" runat="server" Text="Bank Account"></asp:Literal>
                        </td>
                        <td>
                            <eluc:UserControlBankAccount ID="ddlBankAccount" runat="server" AppendDataBoundItems="true"
                                AutoPostBack="true" 
                                CssClass="input" OnTextChangedEvent="ddlBankAccount_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;<asp:Literal ID="lblLongDescription" runat="server" Text="Long Description"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtContraLongDescription" runat="server" CssClass="input_mandatory"
                                Height="75px" TextMode="MultiLine" Width="270px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblCashAccount" runat="server" Text="Cash Account"></asp:Literal>
                        </td>
                        <td style="width: 50%">
                            <span id="spnPickListCashAccount">
                                <asp:TextBox ID="txtAccountCode" runat="server" CssClass="input" MaxLength="20" Width="20%"></asp:TextBox>
                                <asp:TextBox ID="txtAccountDescription" runat="server" CssClass="input" MaxLength="50"
                                    Width="36%"></asp:TextBox>
                                <img runat="server" id="imgShowAccount" style="cursor: pointer; vertical-align: top"
                                    src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListCashAccount', 'codehelp1', '', '../Common/CommonPickListCashAccount.aspx',true); " />
                                <asp:TextBox ID="txtAccountId" runat="server" CssClass="input" MaxLength="20" Width="10px"></asp:TextBox>
                            </span>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
