<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementDocumentPublishedDate.aspx.cs" Inherits="DocumentManagement_DocumentManagementDocumentPublishedDate" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Document Published Date</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPublishedDate" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <br />
    <table cellpadding="1" cellspacing="1" width="25%">
        <tr>
            <td colspan="4">
                <b><asp:Literal ID="lblPublished" runat="server" Text="Are you sure you want to 'Publish' this document? "></asp:Literal></b>
                <br />
                <b>Please enter the date</b>
                <br />
            </td>
        </tr>
        <tr>
             <td colspan="2">
                <br />
                 <asp:Literal ID="lblPublishedDate" runat="server" Text="Published Date"></asp:Literal>
             </td>
             <td colspan="2">
                <br />
                 <eluc:Date ID="ucPublishedDate" runat="server" CssClass="input_mandatory" />
             </td>
        </tr>            
        <tr>
            <td colspan="4">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small" Width="150%"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuPublished" runat="server" OnTabStripCommand="MenuPublished_TabStripCommand">
        </eluc:TabStrip>
    </div>
    </form>
</body>
</html>
