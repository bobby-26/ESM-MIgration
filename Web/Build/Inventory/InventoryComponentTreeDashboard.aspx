<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryComponentTreeDashboard.aspx.cs" Inherits="Inventory_InventoryComponentTreeDashboard" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    
    <script type="text/javascript">
        function PaneResized() {            
            var sender = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            sender.set_height(browserHeight);
            $telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 72);
            $telerik.$("#ifMoreInfo").height(browserHeight - 10);
            frameGridResize();
        }
        window.onresize = window.onload = PaneResized;
        function pageLoad() {            
            PaneResized();
        }
        document.onkeydown = function (e) {
            var keyCode = (e) ? e.which : event.keyCode;
            if (keyCode == 13) {
                __doPostBack("<%=btnSearch.UniqueID %>", "");
            }
        }
        function frameGridResize() {
            var frm = document.getElementById('ifMoreInfo').contentWindow;
            if (frm.Resize != null) {
                setTimeout(function () {
                    frm.Resize();
                }, 200);                
            }
        }
    </script>
        </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPlannedMaintenanceComponent" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tvwComponent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="moreinfo"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuComponent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="moreinfo" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="MenuComponent" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnSearch">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="btnSearch"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="moreinfo"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>


        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%-- <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuComponent" runat="server" OnTabStripCommand="MenuComponent_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
            </div>--%>
        <telerik:RadSplitter ID="RadSplitter1" runat="server" Height="100%" Width="100%">
            <telerik:RadPane ID="navigationPane" runat="server" Width="300" OnClientResized="PaneResized">
                <eluc:TabStrip ID="Menusearch" runat="server" OnTabStripCommand="Menusearch_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                <eluc:TreeView ID="tvwComponent" runat="server" OnNodeClickEvent="tvwComponent_NodeClickEvent"  EmptyMessage="Type to search"/>
                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="hidden" />
             </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server">
                <div id="moreinfo" runat="server">
                    <iframe runat="server" id="ifMoreInfo" style="width: 100%" frameborder="0"></iframe>
                </div>
            </telerik:RadPane>
        </telerik:RadSplitter>

    </form>
</body>
</html>
