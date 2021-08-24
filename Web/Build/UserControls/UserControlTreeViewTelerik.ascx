<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlTreeViewTelerik.ascx.cs" Inherits="UserControlTreeViewTelerik" %>

    <div class="rdTreeFilter" runat="server" id="divTreeFilter">
        <telerik:RadTextBox ClientEvents-OnLoad="telerik.clientTreeSearch" ID="treeViewSearch" runat="server" Width="100%" EmptyMessage="Type to search location"/>
    </div>    
    <div class="rdTreeScroll" style="height:92%; overflow:scroll">
        <telerik:RadTreeView RenderMode="Lightweight" ID="treeViewLocation" runat="server" OnNodeDataBound="treeViewLocation_NodeDataBound"
            OnNodeClick="treeViewLocation_NodeClick">
            <ExpandAnimation Type="None" />
            <CollapseAnimation Type="None" />
            <DataBindings>
                <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
            </DataBindings>   
        </telerik:RadTreeView>
    </div>
