<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAdditionalCommitmentsFilter.aspx.cs"
    Inherits="Accounts_AccountsAdditionalCommitmentsFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselAccount" Src="~/UserControls/UserControlVesselAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
       <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
 
           
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand">
        </eluc:TabStrip>
 
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
                <table width="100%">
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td  width="15%">
                            <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input" VesselsOnly="true" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                        </td>
                        <td width="35%" colspan="2">
                            <span id="spnPickListMaker">
                                <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input" ReadOnly="false"
                                    Width="60px"></telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input" ReadOnly="false"
                                    Width="180px"></telerik:RadTextBox>
                                <img id="ImgSupplierPickList" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                    style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                                <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="lblPOno" runat="server" Text="PO Number"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <telerik:RadTextBox ID="txtPONumber" runat="server" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="lblpurchseorderfromdate" runat="server" Text="PO From date"></telerik:RadLabel>
                        </td>
                        <td >
                            <eluc:Date ID="txtPOFrom" runat="server" CssClass="input" DatePicker="true" />
                        </td>
                        <td >
                            <telerik:RadLabel ID="lblpurchseordertodate" runat="server" Text="PO To date"></telerik:RadLabel>
                        </td>
                        <td >
                            <eluc:Date ID="txtPOTo" runat="server" CssClass="input" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="lblReversedFromDate" runat="server" Text="Reversed From Date "></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtRvFrom" runat="server" CssClass="input" DatePicker="true" />
                        </td>
                        <td >
                            <telerik:RadLabel ID="lblReversedToDate" runat="server" Text="Reversed To Date "></telerik:RadLabel>
                        </td>
                        <td >
                            <eluc:Date ID="txtRvTo" runat="server" CssClass="input" DatePicker="true" />
                        </td>
                    </tr>
                </table>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
