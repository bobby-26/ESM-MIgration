<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseDeliveryFilter.aspx.cs"
    Inherits="PurchaseDeliveryFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MakerList" Src="~/UserControls/UserControlMaker.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="../UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
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
    <form id="frmStockItemFilter" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel runat="server" ID="pnlDiscussion">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuFormFilter" runat="server" OnTabStripCommand="MenuFormFilter_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeliveryNumber" runat="server" Text="Delivery Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDeliveryNumber" runat="server" Text="" CssClass="input"></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblDocumentNumber" runat="server" Text="Order Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDocumentNumber" runat="server" Text="" CssClass="input"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" VesselsOnly="true" CssClass="input" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblFormType" runat="server" Text="Delivery Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucFormType" AppendDataBoundItems="true" CssClass="input" runat="server" />
                    </td>

                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input" DatePicker="true" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtTodate" runat="server" CssClass="input" DatePicker="true" />
                    </td>

                </tr>

            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
