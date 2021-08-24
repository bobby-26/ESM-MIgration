<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsRemittanceFilter.aspx.cs"
    Inherits="AccountsRemittanceFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Remittance Filter" Visible="false"></asp:Label>
            <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand" Title="Remittance Filter"></eluc:TabStrip>
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
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlBankAccount ID="ddlBankAccount"
                            AppendDataBoundItems="true" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemittanceFromDate" runat="server" Text="Remittance From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadTextBox ID="txtRemittenceFromdateSearch" runat="server" Width="90px" CssClass="input"></telerik:RadTextBox>--%>
                        <%-- <ajaxtoolkit:calendarextender id="CalendarExtender2" runat="server" format="dd/MMM/yyyy"
                                    enabled="True" targetcontrolid="txtRemittenceFromdateSearch" popupposition="TopLeft">
                            </ajaxtoolkit:calendarextender>--%>
                        <eluc:UserControlDate ID="txtRemittenceFromdateSearch" runat="server" ReadOnly="false" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemittanceToDate" runat="server" Text="Remittance To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadTextBox ID="txtRemittenceTodateSearch" runat="server" Width="90px" CssClass="input"></telerik:RadTextBox>--%>
                        <%--  <ajaxtoolkit:calendarextender id="CalendarExtender1" runat="server" format="dd/MMM/yyyy"
                                    enabled="True" targetcontrolid="txtRemittenceTodateSearch" popupposition="TopLeft">
                            </ajaxtoolkit:calendarextender>--%>
                        <eluc:UserControlDate ID="txtRemittenceTodateSearch" runat="server" ReadOnly="false" DatePicker="true" />

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
                        <eluc:Hard ID="ucRemittanceStatus" AppendDataBoundItems="true" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" Width="60px" ReadOnly="false"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" Width="200px"
                                ReadOnly="false">
                            </telerik:RadTextBox>
                            <img runat="server" id="ImgSupplierPickList" style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;"
                                src="<%$ PhoenixTheme:images/picklist.png %>" />
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="40px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIsZeroAmount" runat="server" Text="Show Remittances With Zero Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkZeroAmount" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPaymentMode" runat="server" Text="Payment Mode"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlPaymentmode" runat="server" HardTypeCode="132"
                            HardList='<%# PhoenixRegistersHard.ListHard(1, 132) %>' AppendDataBoundItems="true" ShortNameFilter="CHQ,TT,ACH,MTT,MCH,CSH,ACHS"
                            Width="150px" />
                    </td>
                    <%--  <td>
                            Remittance Status
                        </td>
                        <td>
                            <eluc:Hard ID="ucRemittanceStatus" AppendDataBoundItems="true" runat="server" />
                        </td>--%>
                      <td>
                        <telerik:RadLabel ID="lblbatchno" runat="server" Text="Batch Number"></telerik:RadLabel>
                    </td>
                    <td>
                     <telerik:RadTextBox runat="server" ID="txtbatchno" MaxLength="200" CssClass="input"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    
                </tr>
                <tr>
                      <td>
                        <telerik:RadLabel ID="lblvouchernumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                    </td>
                    <td>
                     <telerik:RadTextBox runat="server" ID="txtvouchernumber" MaxLength="200" CssClass="input"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
