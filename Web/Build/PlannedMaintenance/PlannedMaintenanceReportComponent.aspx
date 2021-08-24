<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceReportComponent.aspx.cs"
    Inherits="PlannedMaintenanceReportComponent" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComponentCodeFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
      
        <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
            <eluc:TabStrip ID="MenuReportComponent" runat="server" OnTabStripCommand="MenuReportComponent_TabStripCommand"></eluc:TabStrip>
        </telerik:RadCodeBlock>
        
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblComponentNumber" runat="server" Text="Component Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentNumber" runat="server" CssClass="input" MaxLength="20"
                            Width="80px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblComponentName" runat="server" Text="Component Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentName" runat="server" ReadOnly="false" CssClass="input"
                            MaxLength="400" Width="80px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLocation" runat="server" CssClass="input" MaxLength="50" Width="264px"></telerik:RadTextBox>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblMaker" runat="server" Text="Maker"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtMakerCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="60px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtMakerName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="180px"></telerik:RadTextBox>
                            <img runat="server" id="imgShowMaker" style="cursor: pointer; vertical-align: top" src="<%$ PhoenixTheme:images/picklist.png %>" />
                            <telerik:RadTextBox ID="txtMakerId" runat="server" CssClass="input" Width="1px"></telerik:RadTextBox>
                        </span>&nbsp;
                        <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdMakerClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <span id="spnPickListVendor">
                            <telerik:RadTextBox ID="txtVendorNumber" runat="server" MaxLength="20" Width="60px" CssClass="input readonlytextbox"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" MaxLength="20" Width="180px" CssClass="input readonlytextbox"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox>
                        </span>&nbsp;
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdVendorClear_Click" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentType" runat="server" CssClass="input" MaxLength="50"
                            Width="264px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblClassCode" runat="server" Text="Class Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtClassCode" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
