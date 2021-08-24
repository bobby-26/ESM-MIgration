<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAdminAmcDoneDate.aspx.cs" Inherits="Registers_RegistersAdminAmcDoneDate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Done Date</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 7%;
        }
    </style>
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />        

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmDirectorComment" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title ID="ucTitle" runat="server" ShowMenu="false" Text="" />
        </div>
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <br />
    <table width="50%">
        <tr>
            <td class="auto-style1">
                Asset
            </td>
            <td>
                <asp:TextBox ID="txtAssetName" runat="server" CssClass="readonlytextbox" Enabled="false" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">
                Done Date
            </td>
            <td>
                <eluc:Date ID="ucDoneDate" runat="server" CssClass="input_mandatory" />
            </td>
        </tr>
        <tr>
            <td width="50%" colspan="2">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small" Width="50%"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuAssetMenu" runat="server" OnTabStripCommand="MenuAssetMenu_TabStripCommand">
        </eluc:TabStrip>
    </div>
    </form>
</body>
</html>
