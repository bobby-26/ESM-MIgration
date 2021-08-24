<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersTaxAndChargesAdd.aspx.cs" Inherits="Registers_RegistersTaxAndChargesAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TaxAndCharges</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersTaxAndCharges" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuTaxAndCharges" runat="server" OnTabStripCommand="MenuTaxAndCharges_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuTaxAndCharges1" runat="server" OnTabStripCommand="MenuTaxAndCharges1_TabStripCommand"></eluc:TabStrip>
            <table id="tblTaxAndCharges" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPaymentmode" runat="server" Text="Payment Mode"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlPaymentmode" runat="server" CssClass="input_mandatory" HardTypeCode="132"
                            HardList='<%# PhoenixRegistersHard.ListHard(1, 132) %>' AppendDataBoundItems="true" ShortNameFilter="ATT,NLT,ALT"
                            Width="300px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBankAccount" runat="server" Text="Bank Account "></telerik:RadLabel></td>
                    <td>
                        <eluc:UserControlBankAccount ID="ddlBankAccount"
                            AppendDataBoundItems="true" runat="server" OnTextChangedEvent="ddlBankAccount_SelectedIndexChanged" AutoPostBack="true" CssClass="input_mandatory" />
                    </td>

                </tr>
                <tr>
                    <td>

                        <telerik:RadLabel ID="lblaccnopattern" runat="server" Text="Range Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtRangeFrom" runat="server" CssClass="input_mandatory" MaxLength="50" Text="" />
                        <eluc:Number ID="txtRangeTo" runat="server" CssClass="input_mandatory" MaxLength="50" Text="" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbltaxpercent" runat="server" Text="Tax %"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtTaxPercent" runat="server" CssClass="input_mandatory" MaxLength="8" Text="" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaxGrossAmount" runat="server" Text="Max Gross Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtMaxGrossAmount" runat="server" CssClass="input_mandatory" MaxLength="50" Text="" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblactiveyn" runat="server" Text="Active YN"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkActiveYN" runat="server"></asp:CheckBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
