<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDropDownComponentTree.ascx.cs" Inherits="UserControlDropDownComponentTree" %>
<telerik:RadComboBox RenderMode="Lightweight" ID="AsycTree" runat="server" Width="250px" ShowToggleImage="True"
    Style="vertical-align: middle;" OnClientDropDownOpened="telerik.OnClientDropDownOpenedHandler"
    EmptyMessage="Choose a component" ExpandAnimation-Type="None" CollapseAnimation-Type="None" OnLoad="AsycTree_Load">
    <ItemTemplate>
        <telerik:RadAjaxPanel runat="server" ID="TreeAjaxPanel">
            <div>
                <div class="rdTreeFilter">
                    <telerik:RadTextBox ID="txtComponentSearch" runat="server" OnTextChanged="txtComponentSearch_TextChanged" MaxLength="100" AutoPostBack="true" Width="100%"></telerik:RadTextBox>
                </div>
                <div class="rdTreeScroll">
                    <telerik:RadTreeView RenderMode="Lightweight" runat="server" ID="DropDownTreeView" OnClientNodeClicking="telerik.nodeClicking"
                        Width="100%" Height="300" OnNodeExpand="DropDownTreeView_NodeExpand" OnNodeDataBound="DropDownTreeView_NodeDataBound">
                    </telerik:RadTreeView>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </ItemTemplate>
    <Items>
        <telerik:RadComboBoxItem Text="" CssClass="ajxTree"></telerik:RadComboBoxItem>
    </Items>
</telerik:RadComboBox>
