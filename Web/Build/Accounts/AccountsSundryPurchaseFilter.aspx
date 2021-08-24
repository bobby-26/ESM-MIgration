<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSundryPurchaseFilter.aspx.cs" Inherits="AccountsSundryPurchaseFilter" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sundry Purchase Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

       <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
      
            <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"></eluc:TabStrip>
      
          <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
       <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="98%">
                <table width="50%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="txtRefNo" MaxLength="50" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblStockType" runat="server" Text="Stock Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ddlStockType" CssClass="input" runat="server" AppendDataBoundItems="true"
                                HardTypeCode="97" ShortNameFilter="HOT,STA" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOrderStatus" runat="server" Text="Order Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" CssClass="input"
                                HardTypeCode="41" />
                        </td>
                    </tr>
                </table>
          </telerik:RadAjaxPanel>
    </form>
</body>
</html>
