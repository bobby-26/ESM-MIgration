<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsProjectLineItemPurchaseOrderFilter.aspx.cs" Inherits="Accounts_AccountsProjectLineItemPurchaseOrderFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purchase Order Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Title runat="server" ID="Title" Text="Purchase Order Filter" ShowMenu="false" Visible="false"></eluc:Title>
        <eluc:TabStrip ID="ProjectCodeList" runat="server" OnTabStripCommand="ProjectCodeList_TabStripCommand"></eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table cellpadding="7" cellspacing="4" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDropDownList ID="ddlPOType" runat="server" OnDataBound="ddlPOType_DataBound" CssClass="input" DataTextField="FLDTYPENAME"
                        DataValueField="FLDTYPEID" Width="26%">
                    </telerik:RadDropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="ltPONumber" runat="server" Text="PO Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtPONumber" runat="server" CssClass="input" Width="26%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListMaker">
                        <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input" ReadOnly="false"
                            Width="60px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input" ReadOnly="false"
                            Width="160px">
                        </telerik:RadTextBox>
                        <img id="ImgSupplierPickList" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                            style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                        <telerik:RadTextBox ID="txtVendorId" runat="server" Width="0px"></telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <%--<tr>
                                <td>
                                    <telerik:RadLabel ID="ltInvoiceNo" runat="server" Text="Invoice Number"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtInvoiceNo" runat="server" CssClass="input"></telerik:RadTextBox>
                                </td>
                            </tr>--%>
        </table>
    </form>
</body>
</html>
