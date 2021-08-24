<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFMSMailFilter.aspx.cs"
    Inherits="DocumentManagementFMSMailFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Filter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFileNumber" runat="server" Text="File Number">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtFileNumber" runat="server" MaxLength="200" Width="100%" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel ID="ddlVessel" runat="server" VesselsOnly="true" AppendDataBoundItems="true"
                        Width="100%" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtFromDate" runat="server" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtToDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSender" runat="server" Text="Sender">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSender" runat="server" MaxLength="200" Width="100%" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblRecipient" runat="server" Text="Recipient">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtRecipient" runat="server" MaxLength="200" Width="100%" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSubject" runat="server" MaxLength="200" Width="100%" />
                </td>
                <td colspan="2" />
            </tr>
        </table>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
