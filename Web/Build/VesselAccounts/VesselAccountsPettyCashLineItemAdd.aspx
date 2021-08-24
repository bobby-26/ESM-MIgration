<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsPettyCashLineItemAdd.aspx.cs"
    Inherits="VesselAccountsPettyCashLineItemAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlVesselMappingCurrency.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Petty Cash Expenses</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="97%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
           
            <eluc:TabStrip ID="MenuPettyCash1" runat="server" OnTabStripCommand="MenuPettyCash1_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ddlSeaPortAdd" runat="server" CssClass="input_mandatory" Width="300px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblExpensesOn" runat="server" Text="Expenses On"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateAdd" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPurpose" runat="server" Text="Purpose"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPurposeAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="500"
                            Width="300px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rbPayment" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Payment" Value="0" Selected="true" />
                                <telerik:ButtonListItem Text="Receipt" Value="1" />
                            </Items>
                        </telerik:RadRadioButtonList>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblcurrency" runat="server" Text="Base Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ddlCurrency" runat="server" CssClass="input_mandatory"
                            AutoPostBack="true"  Width="140px"
                            OnTextChangedEvent="ddlCurrency_SelectedIndexChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAmount" runat="server" Text="Base Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAmountAdd" runat="server" CssClass="input_mandatory"  Width="140px"
                            MaxLength="8" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblExchageRate" runat="server" Text="ExChange Rate"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtExchangeRate" runat="server" CssClass="input" DecimalPlace="17" MaxLength="25"
                            Width="140px" Text="" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrencyvessel" runat="server" Text="Vessel Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtvesselcurrency" runat="server" Text="" ReadOnly="true" Width="140px" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrencyvesselamount" runat="server" Text="Vessel Currency Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVCAmount" runat="server" Text="" ReadOnly="true" Width="140px" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
