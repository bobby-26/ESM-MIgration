<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaNewApplicantPersonalGeneral.aspx.cs" Inherits="PreSeaNewApplicantPersonalGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PreSea New Applicant Personal General</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
         </telerik:RadCodeBlock>
    <script type="text/javascript">
        function resize() {
            var obj = document.getElementById("ifMoreInfo");
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight)-60 + "px";
        }
    </script>
       
</head>
<body onload="resize();" onresize="resize();">
     <form id="frmPreSeaPersonalGeneral" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader">
            <div class="divFloatLeft">
                <eluc:Title runat="server" ID="Title1" Text="Applicant" ShowMenu="<%# Title1.ShowMenu %>">
                </eluc:Title>
            </div>
            <div class="divFloat">
                <eluc:TabStrip ID="PreSeaMenu" runat="server" OnTabStripCommand="PreSeaMenu_TabStripCommand" TabStrip="true">
                </eluc:TabStrip>
            </div>
        </div>
        <div class="subHeader">
            <div class="divFloat" style="clear:right">
                <eluc:TabStrip ID="PreSeaMenuGeneral" runat="server" OnTabStripCommand="PreSeaMenuGeneral_TabStripCommand" TabStrip="true">
                </eluc:TabStrip>
            </div>
        </div>
        <div style="position: relative">
            <iframe runat="server" id="ifMoreInfo" frameborder="0" style="width: 100%">
            </iframe>
        </div>
    </div>
    </form>
</body>
</html>
