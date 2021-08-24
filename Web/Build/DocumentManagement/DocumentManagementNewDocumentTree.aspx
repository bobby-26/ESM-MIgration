<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementNewDocumentTree.aspx.cs" Inherits="DocumentManagementNewDocumentTree" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Document</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    
    <script type="text/javascript">

        function PaneResized() {
            var sender = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            sender.set_height(browserHeight - 15);
            $telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 60);
            $telerik.$("#ifMoreInfo").height($telerik.$("#navigationPane").height() - 60);
        }
        window.onresize = window.onload = PaneResized;
        function pageLoad() {
            PaneResized();
        }

       <%--function KeywordSearch() {
            var searchvalue = document.getElementById('<%=treeViewSearch.ClientID%>').value;
            document.getElementById('ifMoreInfo').src = '../DocumentManagement/DocumentManagementSearchResultsNew.aspx?keyword=' + searchvalue;
        }--%>

        (function (global, undefined) {
            
            var telerik = global.telerik = {};
            var treeSearchTimer = null;
            telerik.OnClientClicked = function (sender, eventArgs) {
                ResizeMenu(null);
            };
            telerik.clientTreeSearchthis = function (sender) {
                $telerik.$(".riTextBox", sender.get_element().parentNode).bind("keydown", telerik.valueChanging);
                
            };
            telerik.valueChanging = function (sender, eventArgs) {
                if (treeSearchTimer) {
                    clearTimeout(treeSearchTimer);
                }

                treeSearchTimer = setTimeout(function () {
                    var tree = $find(sender.target.parentElement.parentElement.nextElementSibling.querySelectorAll(".RadTreeView")[0].id);
                    var textbox = $find(sender.target.id);
                    var searchString = textbox.get_element().value;

                    for (var i = 0; i < tree.get_nodes().get_count() ; i++) {
                        telerik.findNodes(tree.get_nodes().getNode(i), searchString);
                    }
                    
                }, 200);
            };
            telerik.findNodes = function (node, searchString) {
                //node.set_expanded(true);


                var hasFoundChildren = false;
                for (var i = 0; i < node.get_nodes().get_count() ; i++) {
                    hasFoundChildren = telerik.findNodes(node.get_nodes().getNode(i), searchString) || hasFoundChildren;
                }

                if (hasFoundChildren || node.get_text().toLowerCase().indexOf(searchString.toLowerCase()) != -1) {
                    node.set_visible(true);
                    if (searchString != undefined && searchString != null && searchString.length > 2)
                        node.set_expanded(true);
                    return true;
                }
                else {
                    node.set_visible(false);
                    return false;
                }
            };
            telerik.OnClientDropDownOpenedHandler = function (sender, eventArgs) {
                var tree = sender.get_items().getItem(0).findControl("DropDownTreeView");
                var selectedNode = tree.get_selectedNode();
                tree._element.style.height = (sender._animatedElement.offsetHeight - 31) + "px";
                if (selectedNode) {
                    selectedNode.scrollIntoView();
                }
            };
            telerik.nodeClicking = function (sender, args) {

                var id = sender._clientStateFieldID;
                var position = id.indexOf("_", id.indexOf("_") + 1);
                var comboBox = $find(id.substring(0, position));
                var node = args.get_node();

                comboBox.set_text(node.get_text());

                comboBox.trackChanges();
                comboBox.get_items().getItem(0).set_text(node.get_text());
                comboBox.get_items().getItem(0).set_value(node.get_value());
                comboBox.commitChanges();
                comboBox.hideDropDown();

            };
        })(window);
        
    </script>
        <style>
            body {
                overflow-y: hidden;
            }
            .outerdiv
            {
                width:100% !important;
                margin: 0 auto !important;
                text-align: left !important;
            }

            .innerdiv
            {
                margin: 2px !important;
                box-sizing: border-box !important;
                display: inline-block !important;
            }
        </style>
        </telerik:RadCodeBlock>
</head>
<body >
    <form id="form1" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
         <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"  >
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tvwComponent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuDiscussion"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" UpdatePanelHeight="28%"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>

                 <telerik:AjaxSetting AjaxControlID="cmdButton">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>   
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
        <div style="height: 100%; margin-left: auto; margin-right: auto; vertical-align: middle;">
         <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <asp:Button runat="server" ID="cmdButton" OnClick="cmdButton_Click" CssClass="hidden" />
         <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuDiscussion" runat="server" Title="Document Management" OnTabStripCommand="MenuDiscussion_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" BorderColor="White" Height="94%" Width="99.5%">
                <telerik:RadPane ID="navigationPane" runat="server" Width="450">
                    <div>
                        <div class="rdTreeFilter outerdiv" runat="server" id="divTreeFilter">
                            <telerik:RadTextBox ClientEvents-OnLoad="telerik.clientTreeSearchthis" ID="treeViewSearch" runat="server" Width="85%" EmptyMessage="Type to search" />
                            <div class="innerdiv">
                                <h12 style="cursor: pointer; width: 10%" onclick="javascript:document.getElementById('cmdButton').click()">
                                    <img runat="server" src="<%$ PhoenixTheme:images/search.png %>" alt="Advanced Search" height="16" width="16" title="Advanced Search" />
                                </h12>
                                <i runat="server" id="BtnTreeSearchInfo" title="Help" class="fas fa-info-circle pull-right"></i>
                            </div>

                        </div>
                    <div class="rdTreeScroll">
                        <telerik:RadTreeView RenderMode="Lightweight" ID="tvwComponent" runat="server" 
                            OnNodeClick="tvwComponent_NodeClickEvent" AllowNodeEditing="false">
                            <ExpandAnimation Type="None" />
                            <CollapseAnimation Type="None" />
                            <DataBindings>
                                <telerik:RadTreeNodeBinding></telerik:RadTreeNodeBinding>
                            </DataBindings>
                        </telerik:RadTreeView>
                    </div>
                    </div>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitBar1" runat="server" CollapseMode="Forward" Height="99%">
            </telerik:RadSplitBar>         
            <telerik:RadPane ID="contentPane" runat="server">
                    <iframe runat="server" id="ifMoreInfo" style="min-height:570px;position:relative;" visible="true" width="99.5%"></iframe>
            </telerik:RadPane>
            </telerik:RadSplitter>
    </div>
    </form>
</body>
</html>
