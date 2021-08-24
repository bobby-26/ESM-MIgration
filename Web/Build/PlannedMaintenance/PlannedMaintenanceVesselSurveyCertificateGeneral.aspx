<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVesselSurveyCertificateGeneral.aspx.cs"
    Inherits="PlannedMaintenance_PlannedMaintenanceVesselSurveyCertificateGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Certificate Details</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="dvscriptsk">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </div>
    <script type="text/javascript">
        function resize() {
            var obj = document.getElementById("ifMoreInfo");
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 30 + "px";

        }
        </script> 
</telerik:RadCodeBlock></head>
<body onload="resize()" onresize="resize()">
    <form id="frmCertificate" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlSurvey">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader">
                    <asp:Literal runat="server" ID="Title1" Text="Certificate Details" ></asp:Literal>
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                </div>
                <div class="navSelect" style="width: auto; float: right; margin-top: -26px">
                    <eluc:TabStrip ID="MenuSurveyCertificateHeader" runat="server" OnTabStripCommand="MenuSurveyCertificateHeader_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div style="position: relative; overflow: hidden; clear: right; border-width:0px;">
                    <iframe runat="server" id="ifMoreInfo" style="height: 620px; width: 100%; border:0">
                    </iframe>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
