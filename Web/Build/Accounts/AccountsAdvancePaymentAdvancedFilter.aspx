<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAdvancePaymentAdvancedFilter.aspx.cs"
    Inherits="AccountsAdvancePaymentAdvancedFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Advance Payment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" Title="Advance Payment" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtPaymentNumber" MaxLength="200" CssClass="input"
                        Width="150px">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPaymentReference" runat="server" Text="PO Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtSupplierReferenceSearch" MaxLength="100" CssClass="input"
                        Width="150px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <%-- <tr>
                        <td>
                            <telerik:RadLabel ID="lblFinancialYear" runat="server" Text="Year"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ddlYearlist" runat="server" QuickTypeCode="55" AppendDataBoundItems="true"
                                AutoPostBack="true" CssClass="input_mandatory" OnTextChangedEvent="ucMonthHard_Changed" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblmonth" runat="server" Text="Month"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucMonthHard" runat="server" HardTypeCode="55" Width="20%" AppendDataBoundItems="true"
                                AutoPostBack="true" CssClass="input_mandatory" SortByShortName="true" OnTextChangedEvent="ucMonthHard_Changed" />
                        </td>
                    </tr>--%>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserControlDate ID="txtPaymentFromdate" runat="server" CssClass="input" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserControlDate ID="txtPaymentTodate" runat="server" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPaymentCurrency" runat="server" Text="Payment Currency"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                        runat="server" AppendDataBoundItems="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPaymentStatus" runat="server" Text="Payment Status"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ucPaymentStatus" AppendDataBoundItems="true" HardTypeCode="127" ShortNameFilter="DRF,APD,OMA,ARM" CssClass="input" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ucType" AppendDataBoundItems="true" CssClass="input" runat="server" HardTypeCode="124" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblSupplierName" runat="server" Text="Supplier Name"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListMaker">
                        <telerik:RadTextBox ID="txtVendorCode" runat="server" Width="60px" CssClass="input" ReadOnly="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVenderName" runat="server" Width="200px"
                            CssClass="input" ReadOnly="false">
                        </telerik:RadTextBox>
                        <img runat="server" id="ImgSupplierPickList" style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;"
                            src="<%$ PhoenixTheme:images/picklist.png %>" />
                        <telerik:RadTextBox ID="txtVendorId" runat="server" Width="40px"></telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <div id="dvVessel" runat="server" class="input" style="overflow: auto; width: 40%; height: 80px">
                        <telerik:RadCheckBoxList ID="chkVesselList" runat="server" Height="100%" OnSelectedIndexChanged="chkVesselList_Changed"
                            RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        </telerik:RadCheckBoxList>
                    </div>
                </td>
                <td>
                    <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text="Invoice Status"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ucInvoiceStatus" AppendDataBoundItems="true" CssClass="input" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPOCancelled" runat="server" Text="PO Cancelled"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkPOCancelled" runat="server" Checked="false" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
