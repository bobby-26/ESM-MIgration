<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewDMSDocumentSectionView.aspx.cs" Inherits="Crew_CrewDMSDocumentSectionView" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: text-top; height: auto; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="subHeader" style="position: relative">
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="ttlContent" Text="View" ShowMenu="false"></eluc:Title>
                     <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
            </div>
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <eluc:TabStrip ID="MenuClose" runat="server" OnTabStripCommand="MenuClose_TabStripCommand"></eluc:TabStrip>
            </div>
        </div>
    </form>
    <br />
    <br />
    <br />
    <div id="divForm" runat="server" style="padding-left: 4%; padding-right: 3%; display: inline-block;">
        <%-- <span id="span1" runat="server" title="View" style="display: inline-block;">
        </span>--%>
    </div>
    <br />
    <br />
</body>
</html>
