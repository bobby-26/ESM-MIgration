<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceManuals.aspx.cs"
    Inherits="PlannedMaintenanceManuals" EnableEventValidation="false" %>
    
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ComponentTypeTreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PMS Manuals</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <style type="text/css">
        a.dbtooltip
        {
            outline: none;
            position: relative;
        }
        a.dbtooltip strong
        {
            line-height: 30px;
        }
        a.dbtooltip:hover
        {
            text-decoration: none;
        }
        a.dbtooltip div
        {
            display: none;
        }
        .hidden
        {
        	display: none;
        }
    </style>

    <script language="javascript" type="text/javascript">
    function getPos(screenX,screenY, param) {
        try {           
            var div = parent.parent.document.getElementById("divToolTip");
            if (div != null) {
                AjxGet(SitePath + "PlannedMaintenance/PlannedMaintenanceManualsToolTip.aspx?"+param, 'divMessage', false);               
                div.innerHTML = "<br/>" + document.getElementById("divMessage").innerHTML + "<br/>";
                div.style.display = "block";

                if (screenX + div.offsetWidth > window.screen.width - 100) {
                    div.style.left = screenX - 10 - div.offsetWidth + "px";
                }
                else {
                    div.style.left = screenX + 10 + "px";
                }

                if (screenY + div.offsetHeight > window.screen.height - 150) {
                    div.style.top = screenY - 80 - div.offsetHeight + "px";
                }
                else {
                    div.style.top = screenY - 80 + "px";
                }

                div.style.backgroundColor = "#fffAF0";
            }
        }
        catch (err) {
            var divErr = parent.parent.document.getElementById("divToolTip");
            if (divErr != null) {
                divErr.innerHTML = err.message;
                divErr.style.display = "none";
            }
        }
    }

    function mouseOut() {
        var div = parent.parent.document.getElementById("divToolTip");
        if (div != null) {
            div.style.display = "none";
            div.innerHTML = "";
        }
    }
     function OpenComponentTreeViewMapping(rpath) {        
        parent.Openpopup('chml', '', SitePath + 'PlannedMaintenance/PlannedMaintenanceManualsComponentTreeView.aspx?<%=Request.QueryString.ToString() %>&rpath='+rpath);
    }
     function resize() {
            var obj = document.getElementById("ifMoreInfo");
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight)-30 + "px";

        }       
        
        function listener(event){
            var data = JSON.parse(event.data);         
            var methodname  = data.method;           
            var message     = data.value;
            if(methodname=="Component")
            {
                OpenComponentTreeViewMapping(message);
            }
            else if(methodname=="mouseout")
            {
                mouseOut();
            } 
            else if(methodname=="mouseover")
            {                
                getPos(data.screenX,data.screenY,message);
            }            
	    }
	    if (window.addEventListener){
            addEventListener("message", listener, false)
        } else {
            attachEvent("onmessage", listener)    
	    }
    </script>

</telerik:RadCodeBlock></head>
<body onload="resize()" onresize="resize()">
    <form id="frmComponentJob" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlComponent">
        <ContentTemplate>
            <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative; right: 0px">
                        <eluc:Title runat="server" ID="Title1" Text="Manuals" ShowMenu="false"></eluc:Title>
                    </div>
                    <iframe id="ifMoreInfo" runat="server" style="min-height: 370px; width: 100%" frameborder="0"></iframe>                    
                    <div id="divMessage" class="hidden"></div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
