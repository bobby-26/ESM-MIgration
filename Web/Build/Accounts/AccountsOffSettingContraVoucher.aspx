<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOffSettingContraVoucher.aspx.cs" Inherits="AccountsOffSettingContraVoucher" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register src="../UserControls/UserControlBankAccount.ascx" tagname="UserControlBankAccount" tagprefix="eluc" %>
<%@ Register src="../UserControls/UserControlCurrency.ascx" tagname="UserControlCurrency" tagprefix="eluc" %>
<%@ Register src="../UserControls/UserControlCompany.ascx" tagname="UserControlCompany" tagprefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
       <%: Scripts.Render("~/bundles/js") %>
      <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmContraVoucher" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
           
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden"/>
                   
                    <eluc:TabStrip ID="MenuVoucher" runat="server" OnTabStripCommand="Voucher_TabStripCommand">
                    </eluc:TabStrip>
            
                    <table cellpadding="1" cellspacing="1" style="width: 100%">
                        <tr>
                            <td>
                              <telerik:RadLabel ID="lblVoucherType" runat="server" Text="Voucher Type"></telerik:RadLabel></td>
                            <td>
                                  <telerik:RadComboBox ID="ddlVoucherType" runat="server" AutoPostBack="True" 
                                    CssClass="input_mandatory" DataTextField="FLDVOUCHERTYPE" 
                                    DataValueField="FLDVOUCHERTYPECODE" 
                                    OnSelectedIndexChanged="ddlVoucherType_SelectedIndexChanged">
                                </telerik:RadComboBox>
                            <%--    <asp:DropDownList ID="ddlVoucherType" runat="server" AutoPostBack="True" 
                                    CssClass="input_mandatory" DataTextField="FLDVOUCHERTYPE" 
                                    DataValueField="FLDVOUCHERTYPECODE" 
                                    OnSelectedIndexChanged="ddlVoucherType_SelectedIndexChanged">
                                </asp:DropDownList>--%>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBankAccount" runat="server" Text="BankAccount"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlBankAccount ID="ddlBankAccount" runat="server" 
                                    AppendDataBoundItems="true" AutoPostBack="true"                                    
                                    CssClass="input" OnTextChangedEvent="ddlBankAccount_SelectedIndexChanged" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCashAccount" runat="server" Text="Cash Account"></telerik:RadLabel> </td>
                            <td style="width: 50%">
                                <span id="spnPickListCashAccount">
                                    <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input" MaxLength="20" Width="20%" style=" text-align:right;"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="input" MaxLength="50"
                                        Width="36%"></telerik:RadTextBox>
                                    <img runat="server" id="imgShowAccount" style="cursor: pointer; vertical-align: top"
                                        src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListCashAccount', 'codehelp1', '', '../Common/CommonPickListCashAccount.aspx',true); " />
                                    <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="input" MaxLength="20" Width="10px"></telerik:RadTextBox>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtVoucherNumber" runat="server" CssClass="readonlytextbox" 
                                    MaxLength="50" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlDate ID="txtVoucherDate" runat="server" 
                                    CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblReferenceNumber" runat="server" Text="Reference Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtReferenceNumber" runat="server" CssClass="input_mandatory" MaxLength="50"
                                    Width="240px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblLocked" runat="server" Text="Locked"></telerik:RadLabel>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkLocked" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text="Due Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlDate ID="ucDueDate" runat="server" CssClass="input" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtStatus" CssClass="readonlytextbox" Width="160px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblLongDescription" runat="server" Text="Long Description"></telerik:RadLabel>
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox runat="server" ID="txtLongDescription" TextMode="MultiLine" Width="270px"
                                    Height="45px" CssClass="input"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblLastUpdatedBy" runat="server" Text="Last Updated By"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtUpdatedBy" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="160px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblLastUpdatedDate" runat="server" Text="Last Updated Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtUpdatedDate" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="160px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                        <tr>
                            <td>
                                <b><telerik:RadLabel ID="lblEntryToBeOffset" runat="server" Text="Entry To Be Off-set:"></telerik:RadLabel></b>&nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblOffSettingVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel></td>
                            <td>
                                <telerik:RadTextBox ID="txtOffSettingVoucherNumber" runat="server" 
                                    CssClass="readonlytextbox" MaxLength="50" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblOffSettingReferenceNumber" runat="server" Text="Reference Number"></telerik:RadLabel></td>
                            <td>
                                <telerik:RadTextBox ID="txtReferenceNo" runat="server" CssClass="readonlytextbox" 
                                    MaxLength="50" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:UserControlCurrency ID="ddlCurrencyCode" runat="server" 
                                    AppendDataBoundItems="true" CssClass="dropdown_input" 
                                    CurrencyList="<%# PhoenixRegistersCurrency.ListCurrency(1)%>" Enabled="false" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtAmount" runat="server" CssClass="readonlytextbox" style=" text-align:right;"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAllocatedVoucherAmount" runat="server" Text="Allocated Voucher Amount"></telerik:RadLabel></td>
                            <td>
                                <telerik:RadTextBox ID="txtAllocatedVoucherAmount" runat="server" style=" text-align:right;"
                                    CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblBalanceVoucherAmount" runat="server" Text="Balance Voucher Amount"></telerik:RadLabel></td>
                            <td>
                                <telerik:RadTextBox ID="txtBalanceVoucherAmount" runat="server" style=" text-align:right;"
                                    CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
             
                <br />
                <br />
                 <eluc:Confirm ID="ucConfirm" runat="server" OKText="Create" CancelText="Cancel" Visible="false" OnConfirmMesage="Approve" /> 
            </telerik:RadAjaxPanel>
     <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
