<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsLeaveAllotmentGeneral.aspx.cs"
    Inherits="AccountsLeaveAllotmentGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Leave Allotment General</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function resize() {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 30 + "px";
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body onload="resize();" onresize="resize();">
    <telerik:RadFormDecorator ID="frmRegistersRankk" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_OnClick" />
            <asp:TextBox ID="txtHidden" runat="server" />

            <eluc:TabStrip ID="LeaveAllotment" runat="server" OnTabStripCommand="LeaveAllotment_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <iframe runat="server" id="ifMoreInfo" frameborder="0" style="width: 100%;height:93%"></iframe>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
