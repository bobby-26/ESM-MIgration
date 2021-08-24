<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCtmfollowupMail.aspx.cs" Inherits="AccountsCtmfollowupMail" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mail Compose</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
<%--   <div id="headerdiv" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </div>--%>
</telerik:RadCodeBlock></head>
<body>
       <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
 <%-- <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false" >
    </ajaxToolkit:ToolkitScriptManager>--%>
   <%-- <div class="subHeader">
        <div id="divHeading" class="divFloatLeft">
            <eluc:Title runat="server" ID="ucTitle" Text="Message" ShowMenu="false"></eluc:Title>
        </div>--%>
     <%--   <div style="position: absolute; right: 0px">--%>
            <eluc:TabStrip ID="MenuMailRead" runat="server" OnTabStripCommand="MenuMailRead_TabStripCommand">
            </eluc:TabStrip>
      <%--  </div>--%>
        <eluc:Status runat="server" ID="ucStatus" />
  <%--  </div>--%>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <table width="100%" border="1">
        <tr>
            <td width="40%">
                <table width="100%">
                    <tr>
                        <td>
                            To
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTo" runat="server" CssClass="input" Width="90%" ></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Cc
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCc" runat="server" CssClass="input" Width="90%" ></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Subject
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSubject" runat="server" CssClass="input" Width="90%" ></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="40%">
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td colspan="2" width="100%">
                <cc1:Editor ID="edtBody" runat="server" Width="100%" Height="400px" CssClass="readonlytextbox" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
