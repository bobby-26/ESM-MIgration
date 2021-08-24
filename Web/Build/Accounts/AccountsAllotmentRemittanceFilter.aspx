<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRemittanceFilter.aspx.cs" Inherits="AccountsAllotmentRemittanceFilter" %>


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remittance Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">

        <eluc:TabStrip ID="MenuOfficeFilterMain" Title="Remittance Filter" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlRemittance" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlRemittence">
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemittanceNumber" runat="server" Text="Remittance Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRemittenceNumberSearch" MaxLength="200" CssClass="input"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" Text="Name" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input" Width="240px" MaxLength="200"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFileNumber" Text="File No." runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileNumber" runat="server" CssClass="input" Width="240px" MaxLength="10"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemittanceCurrency" runat="server" Text="Remittance Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemittanceStatus" runat="server" Text="Remittance Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucRemittanceStatus" AppendDataBoundItems="true" CssClass="input" runat="server" Width="200px" />
                    </td>
                    <td id="tdvessel">
                        <telerik:RadLabel ID="lblVessel" Text="Vessel" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlVessel ID="ddlVessel" runat="server" AppendDataBoundItems="true"
                            Width="240px" CssClass="input" VesselsOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemittanceFromDate" runat="server" Text="Remittance From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtRemittenceFromdateSearch" runat="server" CssClass="input" DatePicker="true" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemittanceToDate" runat="server" Text="Remittance To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtRemittenceTodateSearch" runat="server" CssClass="input" DatePicker="true" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" CssClass="input"
                            AppendDataBoundItems="true" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPaymentMode" runat="server" Text="Payment Mode"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlPaymentmode" runat="server" CssClass="input" HardTypeCode="132"
                            HardList='<%# PhoenixRegistersHard.ListHard(1, 132) %>' AppendDataBoundItems="true" ShortNameFilter="ATT,NLT,ALT" Width="150px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Bank Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlBankAccount ID="ddlBankAccount" AppendDataBoundItems="true"  AutoPostBack="true" runat="server" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
