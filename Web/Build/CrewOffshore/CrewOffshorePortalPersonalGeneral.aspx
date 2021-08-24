<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshorePortalPersonalGeneral.aspx.cs" Inherits="CrewOffshore_CrewOffshorePortalPersonalGeneral" %>

<!DOCTYPE html>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Personal General</title>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript">
            function resize() {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 60 + "px";
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body onload="resize();" onresize="resize();">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <%-- <div class="subHeader" style="position: relative">
            <div id="divHeading">
                <eluc:Title runat="server" ID="Title1" Text="Crew Management" ShowMenu="<%# Title1.ShowMenu %>">
                </eluc:Title>
            </div>
        </div>--%>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
            </div>
           <%-- <div style="font-weight: 600; font-size: 12px;" runat="server">

                <eluc:TabStrip ID="CrewMenuGeneral" runat="server" OnTabStripCommand="CrewMenuGeneral_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>

            </div>--%>
            <iframe runat="server" id="ifMoreInfo" frameborder="0" style="width: 100%"></iframe>
        </div>
    </form>
</body>
</html>

