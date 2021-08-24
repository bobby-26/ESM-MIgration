<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ERMERMdisplayVoucherFilter.aspx.cs" Inherits="ERMERMdisplayVoucherFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Voucher"></asp:Label>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAddressEntry">
        <ContentTemplate>
            <div id="divFind" style="position: relative; z-index: 2">
                <table id="tblExchangeRate">
                    <tr>
                        <td width="10%">
                            <asp:Literal ID="lblERMVoucherNumber" runat="server" Text="ERM Voucher Number"></asp:Literal>
                        </td>
                        <td width="40%">
                            <asp:TextBox ID="txtErmvoucherno" runat="server" MaxLength="100" CssClass="input"
                                Width="150px"></asp:TextBox>
                        </td>


                         <td width="10%">
                            <asp:Literal ID="lblESMVoucherNumber" runat="server" Text="Voucher Number"></asp:Literal>
                        </td>
                        <td width="40%">
                            <asp:TextBox ID="txtEsmvoucherno" runat="server" MaxLength="100" CssClass="input"
                                Width="150px"></asp:TextBox>
                        </td>
                       
                    </tr>
                    <tr>
                        <td width="20%">
                            <asp:Literal ID="lblVoucherFromDate" runat="server" Text="Voucher From Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:UserControlDate ID="txtVoucherFromdateSearch" runat="server" CssClass="input" />
                        </td>
                           <td >
                            <asp:Literal ID="lblVoucherToDate" runat="server" Text="Voucher To Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:UserControlDate ID="txtVoucherTodateSearch" runat="server" CssClass="input" />
                        </td>
                   </tr>
                         <tr>
                            <td >
                            <asp:Literal ID="lblLongDescription" runat="server" Text="Long Description"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLongDescription" runat="server" TextMode="MultiLine" Rows="2"
                                CssClass="input"></asp:TextBox>
                        </td>
                       <td>
                            <asp:Literal ID="lblReferenceNumber" runat="server" Text="Reference Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReferenceNumberSearch" runat="server" MaxLength="100" CssClass="input"
                                Width="150px"></asp:TextBox>
                        </td>
                        
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
