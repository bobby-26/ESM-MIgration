<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceMoreInfo.aspx.cs" Inherits="AccountsInvoiceMoreInfo" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
<form id="frmLedgerGeneral" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
     <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
            
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             
           <eluc:TabStrip ID="Menutab" runat="server" TabStrip="false" >
                            </eluc:TabStrip>
                
                
                    <table cellpadding="2" cellspacing="1" style="width: 100%" border="1">
                        <tr>
                            <td style="width: 25%">
                                <b>
                                    <telerik:RadLabel ID="lblPOListCap" runat="server" Text="PO List"></telerik:RadLabel></b>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPOList" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>
                                    <telerik:RadLabel ID="lblMismatchCap" runat="server" Text="Supplier Mismatch"></telerik:RadLabel></b>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblMismatch" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>
                                    <telerik:RadLabel ID="lblvesselListCap" runat="server" Text="Vessel List"></telerik:RadLabel></b>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblvesselList" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>
                                    <telerik:RadLabel ID="lblCurrencyMismatchCap" runat="server" Text="Currency Mismatch"></telerik:RadLabel></b>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCurrencyMismatch" runat="server"></telerik:RadLabel></b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>
                                    <telerik:RadLabel ID="lblPurchaserNameCap" runat="server" Text="Purchaser Name"></telerik:RadLabel></b>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPurchaserName" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>
                                    <telerik:RadLabel ID="lblPurchaserUserNameCap" runat="server" Text="Purchaser User Name"></telerik:RadLabel></b>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPurchaserUserName" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>
                                    <telerik:RadLabel ID="lblSuperintendentNameCap" runat="server" Text="Superintendent Name"></telerik:RadLabel></b>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblSuperintendentName" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>
                                    <telerik:RadLabel ID="lblSuperintendentUserNameCap" runat="server" Text="Superintendent User Name"></telerik:RadLabel></b>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblSuperintendentUserName" runat="server" Text="Superintendent User Name"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>
                                    <telerik:RadLabel ID="lblPOCountCap" runat="server" Text="PO Count"></telerik:RadLabel></b>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPOCount" runat="server" Text="PO Count"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
              
         </telerik:RadAjaxPanel>
    </form>
</body>
</html>
