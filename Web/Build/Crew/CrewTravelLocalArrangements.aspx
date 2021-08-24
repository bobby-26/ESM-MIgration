<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelLocalArrangements.aspx.cs"
    Inherits="CrewTravelLocalArrangements" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<%@ register tagprefix="eluc" tagname="TabStrip" src="~/UserControls/UserControlTabs.ascx" %>
<head id="Head1" runat="server">
    <title>Arrangements</title>
      <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmArrangements" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <eluc:Title runat="server" ID="Title1" Text="Local Arrangements" ShowMenu="false">
                </eluc:Title>
            </div>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuTravel" runat="server" OnTabStripCommand="MenuTravel_TabStripCommand">
                </eluc:TabStrip>
            </div>
        </div>
        <asp:Panel ID="pnlArrangements" runat="server" GroupingText="Arrangements Required" Width="500">
            <asp:CheckBoxList ID="cblArrangements" runat="server" DataTextField="FLDHARDNAME" CellPadding="10"
                DataValueField="FLDHARDCODE" RepeatDirection="Horizontal">
            </asp:CheckBoxList>
        </asp:Panel>
        <eluc:Status ID="ucStatus" runat="server" />
    </div>
    </form>
</body>
</html>
