<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlTabs.ascx.cs" Inherits="UserControlTabs" %>
<div class="navAppSelect" style="position:relative">
<asp:DataList runat="server" ID="dlstTabs" RepeatDirection="Horizontal" GridLines="Both"
    CellPadding="0" CellSpacing="3" OnItemDataBound="dlstTabs_ItemDataBound" OnItemCommand="dlstTabs_ItemCommand">
    <ItemStyle CssClass="notselected" HorizontalAlign="Center"/>
    <SelectedItemStyle CssClass="notselected" />
    <AlternatingItemStyle CssClass="notselected" />
    <ItemTemplate>
        <asp:LinkButton runat="server" ID="btnMenu" Visible="false"></asp:LinkButton>
        <asp:Label runat="server" ID="lblMenu" Visible="false" CssClass="navPanellabel"></asp:Label>
    </ItemTemplate>
</asp:DataList>
</div>

