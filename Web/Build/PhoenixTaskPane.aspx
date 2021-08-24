<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PhoenixTaskPane.aspx.cs" Inherits="PhoenixTaskPane" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    <script type="text/javascript">
        function resizeCustom()
        {
            resizeWindow();
            expandModule('divMenu');
        }
        
        window.onresize = function() 
        {
            resizeWindow();
            expandModule('divMenu');
        }

        function opendropmenu() {
            document.getElementById('divApplications').style.overflow = "visible";
            document.getElementById('divApplications').style.height = "auto";
            document.getElementById('lnkMenus').style.visibility = "hidden";
            document.getElementById('lnkMenus').style.height = "0px";
        }
        function closedropmenu() {
            document.getElementById('divApplications').style.overflow = "hidden";
            document.getElementById('divApplications').style.height = "25px";
            document.getElementById('lnkMenus').style.visibility = "visible";
            document.getElementById('lnkMenus').style.height = "25px";
        } 
    </script>
</head>
<body onload="resizeWindow()">
    <form id="form1" runat="server">
    <div class="navigation" id="navigation" style="position:absolute; top:0px; margin-left: 0px; vertical-align: bottom;height:705px; display:inline">
        <div class="subHeader">
            <div id="divHeading" runat="server">
                Menu
            </div>
            <a id="lnkOptions" visible="false"  onserverclick="lnkOptions_serverclick"></a>
        </div>
        <div class="navPanel" id="divMenu" style="overflow-x:hidden; overflow-y:auto;
            vertical-align: top; position: relative; height:470px; margin-bottom: 10px" runat="server">
            <asp:TreeView ID="tvwMenu" runat="server" NodeIndent="5" ShowLines="true"
                Width="20%" ExpandDepth="1" SkipLinkText="Menu" DataSourceID="XmlMenu" OnSelectedNodeChanged="tvwMenu_SelectedNodeChanged" 
                OnTreeNodeDataBound="tvwMenu_DataBinding"
                >          
                <SelectedNodeStyle CssClass="navPanelMenu" />      
                <ParentNodeStyle Font-Names="Tahoma" Font-Size="8.5pt" Font-Bold="true" ForeColor="DarkBlue" />
                <RootNodeStyle Font-Names="Tahoma" Font-Bold="True" Font-Size="8.5pt" HorizontalPadding="0" />
                <DataBindings>
                    <asp:TreeNodeBinding DataMember="DIRECTORY" TextField="NAME" ValueField="CODE" />
                    <asp:TreeNodeBinding DataMember="FILE" TextField="NAME" ValueField="CODE" NavigateUrlField="URL" SelectAction="Select"/>
                </DataBindings>
            </asp:TreeView>      
            <asp:XmlDataSource ID="XmlMenu" runat="server" DataFile="FileStructureXML.xml" XPath="/RECORDSET/Options"> 
            </asp:XmlDataSource>                   
        </div>        
        <div class="navAppSelect" id="divApplicationSelect" style="position: absolute; vertical-align: bottom; float:left; bottom: 5px;" runat="server">
<%--            <div class="navAppSeparator" onclick="showHideDiv('divApplications')">                
            </div>--%>
            <div id="divApplications" onmouseout="closedropmenu()" onmouseover="opendropmenu()" style="height:25px;">                             
                <a id="lnkMenus" style="visibility:visible;height:25px; text-align:center">Phoenix Menus</a>
                <%--<a runat="server" id="lnkDashboard" onclick="showVessel();" onserverclick="lnk_serverclick" visible="false">Dashboard</a>--%>
                <a runat="server" id="lnkRegisters" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="R">Registers</a>
                <a runat="server" id="lnkInventory" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="I">Inventory</a> 
                <a runat="server" id="lnkPurchase" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="P">Purchase</a>
                <a runat="server" id="lnkPlannedMaintenance" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="M">Planned Maintenance</a>
                <a runat="server" id="lnkCertificatesandSurveys" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="CS">Certificates and Surveys</a>
                <a runat="server" id="lnkDocking" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="D">Docking</a>
                <a runat="server" id="lnkQuality" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="Q">Quality</a>
                <a runat="server" id="lnkAccounts" onclick="showCompany();"  onserverclick="lnk_serverclick" accesskey="A">Accounts</a>
                <a runat="server" id="lnkCrew" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="C">Crew</a>
                <a runat="server" id="lnkReports" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="R">Reports</a>
                <a runat="server" id="lnkBudget" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="B">Budget</a>
                <a runat="server" id="lnkVesselAccounting" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="V">Vessel Accounting</a>                 
                <a runat="server" id="lnkOwners" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="O">Owners</a>                 
                <a runat="server" id="lnkDocumentManagement" onclick="showCompany();" onserverclick="lnk_serverclick" accesskey="M">Document Management</a>
                <a runat="server" id="lnkVesselPosition" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="W">Vessel Position</a>  
                <a runat="server" id="lnkDMR" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="W">DMR</a>   
                <a runat="server" id="lnkPreSea" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="S">Pre Sea</a>                
                <a runat="server" id="lnkOffshore" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="H">Offshore</a>
                <a runat="server" id="lnkAdministration" onclick="showVessel();" onserverclick="lnk_serverclick" accesskey="AD">Administration</a>
            </div>
        </div>
    </div>
    </form>
    <script>
       resizeCustom();
    </script>
</body>
</html>
