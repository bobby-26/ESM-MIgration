<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersFlagDocumentsRequired.aspx.cs" Inherits="RegistersFlagDocumentsRequired" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="~/UserControls/UserControlDocuments.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Documents Required</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">    
        <div id="certificateslink" runat="server">
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager> 
    
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;  width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false">
        </eluc:Error>
        
        <div class="subHeader" style="position: relative">
            <div id="divHeading">
                <eluc:Title runat="server" id="ucTitle" Text="Flag Documents Required"></eluc:Title>
            </div>
        </div>
        
        <div style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuFlag" runat="server" OnTabStripCommand="MenuFlag_TabStripCommand" TabStrip="true">
            </eluc:TabStrip><asp:Button runat="server" ID="cmdHiddenSubmit" Visible="false" OnClick="cmdHiddenSubmit_Click" />
        </div>
        
        <iframe runat="server" id="ifMoreInfo"  scrolling="yes" style="min-height: 500px; height: 800px; width:100%">
        </iframe>
    </div>
    </form>
</body>
</html>
