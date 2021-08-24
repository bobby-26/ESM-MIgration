<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantActivitylGeneral.aspx.cs"
    Inherits="CrewNewApplicantActivitylGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Personal General</title>
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
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
        
            
                    <eluc:TabStrip ID="CrewActivity" Title="Crew Activities" runat="server" OnTabStripCommand="CrewActivity_TabStripCommand" TabStrip="true"></eluc:TabStrip>
              
           
            <div style="position: relative">
                <iframe runat="server" id="ifMoreInfo" frameborder="0" scrolling="no" style="width: 100%"></iframe>
            </div>
        </div>
    </form>
</body>
</html>
