<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockSuptRemarks.aspx.cs"
    Inherits="DryDockSuptRemarks" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDetail" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader" style="position: relative">
                <div id="divHeading" style="vertical-align: top">
                    <eluc:Title runat="server" ID="Title1" Text="Job Description Changes" ShowMenu="false"></eluc:Title>
                </div>
            </div>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuSuptRemark" runat="server" OnTabStripCommand="MenuSuptRemark_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            </div>
            <div style="font-weight: 600; font-size: 12px;" runat="server">

                <eluc:TabStrip ID="MenuJobDetail" runat="server" OnTabStripCommand="MenuJobDetail_TabStripCommand"></eluc:TabStrip>

            </div>

            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input_mandatory" TextMode="MultiLine" Height="350px" Width="90%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastModifiedDateGMT" runat="server" Text="Last Modified Date (GMT)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastModifiedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastModifiedBy" runat="server" Text="Last Modified By"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastModifiedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />
        </div>
    </form>
</body>
</html>
