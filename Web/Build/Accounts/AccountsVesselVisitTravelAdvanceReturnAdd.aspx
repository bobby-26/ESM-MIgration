<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselVisitTravelAdvanceReturnAdd.aspx.cs"
    Inherits="AccountsVesselVisitTravelAdvanceReturnAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Travel Advance Return</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmITVisit" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Title runat="server" ID="Title1" Text="Travel Advance Return Add" ShowMenu="false" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuTravelAdvance" runat="server" OnTabStripCommand="MenuTravelAdvance_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                    <td style="width: 40%">
                        <telerik:RadLabel ID="lblEmployee" runat="server" Text="Employee ID/Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployee" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="40%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblApprovedAmount" runat="server" Text="Total Approved Amount (USD)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtApprovedAmount" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="40%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Advance Approved Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency runat="server" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' Width="40%"
                            ID="ucCurrency" AppendDataBoundItems="true" AutoPostBack="false" CssClass="dropdown_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPaymentAmount" runat="server" Text="Payment Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtPaymentAmount" runat="server" CssClass="input_mandatory" Width="40%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReturnAmount" runat="server" Text="Return Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtReturnAmount" runat="server" CssClass="input" Width="40%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReturnDate" runat="server" Text="Return Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtReturnDate" runat="server" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
