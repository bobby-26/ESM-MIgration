<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceGlobalComponentMigrate.aspx.cs" Inherits="PlannedMaintenanceGlobalComponentMigrate" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Migrate to Vessel</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized() {
            var sender = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            sender.set_height(browserHeight - 40);
            $telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 55);
        }
    </script>
</head>
<body onresize="PaneResized();" onload="PaneResized();">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tvwComponent">
                    <UpdatedControls>
                         <telerik:AjaxUpdatedControl ControlID="tvwComponent"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuComponent">
                    <UpdatedControls>
                         <telerik:AjaxUpdatedControl ControlID="tvwComponent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuComponent" LoadingPanelID="RadAjaxLoadingPanel"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucStatus"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" /> 
        <div style="font-weight:600;font-size:12px" runat="server">
                <eluc:TabStrip ID="MenuComponent" runat="server" OnTabStripCommand="MenuComponent_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
            </div>
        
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="200px" Width="100%">
            <telerik:RadPane ID="navigationPane" runat="server" Width="400">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" runat="server" Text="NO Of Units" ID="lblUnits"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number runat="server" ID="txtUnits" />
                        </td>
                    </tr>
                </table>
                <%--<telerik:RadTreeView ID="tvwComponent" runat="server" OnNodeClickEvent="tvwComponent_NodeClickEvent" OnNodeDataBoundEvent="tvwComponent_NodeDataBoundEvent" SearchEmptyMessage="Type to search component" />--%>
                <div class="rdTreeFilter" runat="server" id="divTreeFilter">
                    <telerik:RadTextBox ClientEvents-OnLoad="telerik.clientTreeSearch" ID="treeViewSearch" runat="server" Width="100%" EmptyMessage="Type to search Component" />
                </div>
                <div class="rdTreeScroll">
                    <telerik:RadTreeView RenderMode="Lightweight" ID="tvwComponent" runat="server" OnNodeDataBound="tvwComponent_NodeDataBoundEvent" CheckBoxes="true" CheckChildNodes="true"
                        OnNodeClick="tvwComponent_NodeClickEvent" AllowNodeEditing="true" OnNodeEdit="tvwComponent_NodeEdit" OnNodeCreated="tvwComponent_NodeCreated">
                        <NodeTemplate>
                            <telerik:RadLabel runat="server" RenderMode="Lightweight" ID="lblDisplayName" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                            <asp:LinkButton runat="server" AlternateText="ADD"
                                CommandName="ADD" ID="cmdAdd" OnClick="cmdAdd_Click"
                                ToolTip="Add" Width="20PX" Height="20PX">
                                            <span class="icon"><i class="fas fa-plus"></i></span>
                            </asp:LinkButton>
                        </NodeTemplate>
                        <ExpandAnimation Type="None" />
                        <CollapseAnimation Type="None" />
                        <DataBindings>
                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                        </DataBindings>
                    </telerik:RadTreeView>
                </div>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
            </telerik:RadSplitBar>
            <telerik:RadPane ID="contentPane" runat="server">
                <table style="width: 50%">
                    <tr>
                        <td style="width: 20%">
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td style="width: 80%">
                            <eluc:Vessel ID="ucVessel" runat="server" Width="100%" />
                        </td>
                    </tr>
                </table>
            </telerik:RadPane>
        </telerik:RadSplitter>
    </form>
</body>
</html>
