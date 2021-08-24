<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAirfarePaymentVoucherSearch.aspx.cs" Inherits="AccountsAirfarePaymentVoucherSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAddressEntry">
        <ContentTemplate>
            <div class="navigation" id="navigation1" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Airfare Payment Voucher"></asp:Label>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuFilterMain" runat="server" OnTabStripCommand="MenuFilterMain_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table width="100%" cellpadding="1" cellspacing="1" style="z-index: 2;">
                        <tr>
                            <td>
                                <asp:Literal ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="input" Width="150px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVessel" runat="server" CssClass="input" Width="150px"></asp:TextBox>
                            </td>
                         </tr>
                         <tr>
                            <td>
                                <asp:Literal ID ="lblRangeFrom" runat="server" Text="Range From"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID ="txtRangeFrom" runat="server" CssClass="input" Width="150px" />
                            </td>
                            <td>
                                <asp:Literal ID ="lblRangeTo" runat="server" Text="Range To"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID ="txtRangeTo" runat="server" CssClass="input" Width="150px" />
                            </td>
                         </tr>
                         <tr>                                
                            <td>
                                <asp:Literal ID="lblFromDate" runat="server" Text="Departure Date From"></asp:Literal>
                            </td>
                            <td>
                                <eluc:UserControlDate ID="txtFromDate" runat="server" CssClass="input" />
                            </td>
                            <td>
                                <asp:Literal ID="lblToDate" runat="server" Text="Departure Date To"></asp:Literal>
                            </td>
                            <td>
                                <eluc:UserControlDate ID="txtToDate" runat="server" CssClass="input" />
                            </td>
                         </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
