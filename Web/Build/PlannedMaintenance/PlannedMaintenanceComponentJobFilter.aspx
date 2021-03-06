<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentJobFilter.aspx.cs"
    Inherits="PlannedMaintenanceComponentJobFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MakerList" Src="~/UserControls/UserControlMaker.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title></title>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComponentFilter" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="subHeader" style="position: relative">
        <div id="div2" style="vertical-align: top">
            <eluc:Title runat="server" ID="Title1" Text="Component Filter" ShowMenu="True"></eluc:Title>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuComponentFilter" runat="server" OnTabStripCommand="MenuComponentFilter_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlDiscussion">
        <ContentTemplate>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblFunctionNumber" runat="server" Text="Function Number"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFunctionNumber" runat="server" CssClass="input" MaxLength="9"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                       <asp:Literal ID="lblFunctionDescription" runat="server" Text="Function Description"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFunctionDescription" runat="server" CssClass="input" Width="180px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                       <asp:Literal ID="lblPerformingcomponentNumber" runat="server" Text="Performing component Number"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtComponentNumber" runat="server" CssClass="input" MaxLength="9"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                         <asp:Literal ID="lblPerformingcomponentName" runat="server" Text="Performing component Name"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtComponentName" runat="server" CssClass="input" Width="180px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
