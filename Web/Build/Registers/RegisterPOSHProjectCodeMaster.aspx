<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterPOSHProjectCodeMaster.aspx.cs" Inherits="Registers_RegisterPOSHProjectCodeMaster" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Project Code Master</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    <script type="text/javascript">
        function resizeFrame(obj) {
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 50 + "px";
        }      
    </script>

</telerik:RadCodeBlock></head>
<body onresize="resizeFrame(document.getElementById('ifMoreInfo'));" onload="resizeFrame(document.getElementById('ifMoreInfo'));">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server"  >
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel runat="server" ID="pnlProjectMaster" Height="100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
<%--                    <eluc:Title runat="server" ID="Title1" Text=" Chart of Accounts"></eluc:Title>--%>
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    <eluc:TabStrip ID="MenuProjectMaster" runat="server" OnTabStripCommand="ProjectMaster_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 100%;width: 100%"></iframe>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
