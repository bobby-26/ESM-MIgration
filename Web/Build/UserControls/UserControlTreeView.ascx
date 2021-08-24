<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlTreeView.ascx.cs" Inherits="UserControlTreeView" %>
<asp:TreeView ID="tvwTree" runat="server" NodeIndent="10" ShowLines="true"  
    Width="20%" ExpandDepth="FullyExpand" ShowExpandCollapse="true" SkipLinkText="Planned Maintenance" OnSelectedNodeChanged="tvwTree_SelectedNodeChanged" OnUnload="tvwTree_Unload">
    <NodeStyle CssClass="tvwTest_tvwTree_1" />
    <RootNodeStyle Font-Names="Tahoma" Font-Bold="True" Font-Size="8.5pt" HorizontalPadding="0" />    
    <SelectedNodeStyle BackColor="#FFE88C" />
    <Nodes>
        <asp:TreeNode Value="Root" Text="">
        </asp:TreeNode>
    </Nodes>
</asp:TreeView>
