<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAdvanceRemittanceRequest.aspx.cs"
    Inherits="AccountsAdvanceRemittanceRequest" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remittence Line Item</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Remittance" ShowMenu="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
        </div>
        <table cellpadding="1" cellspacing="1" style="width: 100%">
            <tr>
                <td>
                    <asp:Literal ID="lblRemittanceNumber" runat="server" Text="Remittance Number"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtRemittanceNumber" ReadOnly="true" Width="180px" runat="server"
                        CssClass="input"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblSupplier" runat="server" Text="Supplier"></asp:Literal>
                </td>
                <td>
                    <span id="spnPickListSupplier">
                        <asp:TextBox ID="txtSupplierCode" runat="server" Width="60px" CssClass="readonlytextbox"></asp:TextBox>
                        <asp:TextBox ID="txtSupplierName" runat="server" BorderWidth="1px" Width="180px"
                            CssClass="readonlytextbox"></asp:TextBox>
                        <asp:ImageButton ID="btnPickSupplier" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            OnClientClick="return showPickList('spnPickListSupplier', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131', true);" />
                        <asp:TextBox ID="txtSupplierId" runat="server" Width="1" CssClass="input"></asp:TextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblAccountCode" runat="server" Text="Account Code"></asp:Literal>
                </td>
                <td>
                    <eluc:UserControlBankAccount ID="ddlBankAccount" BankAccountList='<%# PhoenixRegistersAccount.ListBankAccount(null,null,iCompanyid)%>'
                        AppendDataBoundItems="true" OnTextChangedEvent="ddlBankAccount_SelectedIndexChanged"
                        AutoPostBack="true" runat="server" CssClass="input_mandatory" />
                </td>
                <td style="width: 380px">
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtSubAccountCode" Visible="false" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtAccountId" Visible="false" runat="server"> </asp:TextBox>
                    <asp:TextBox ID="txtCurrencyId" Visible="FALSE" ReadOnly="true" runat="server" CssClass="input"></asp:TextBox>
                    <asp:TextBox ID="txtCurrencyCode" Visible="true" ReadOnly="true" runat="server" CssClass="input"></asp:TextBox>
                </td>
                <td style="width: 380px">
                </td>
            </tr>          
        </table>
     
    </div>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
