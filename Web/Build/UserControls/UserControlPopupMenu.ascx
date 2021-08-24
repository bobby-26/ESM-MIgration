<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPopupMenu.ascx.cs" Inherits="UserControlPopupMenu" %>
<div id="divContextMenu" class="cntxMenuSelect" style="display:none;z-index:99;position:absolute; border:20px; border-color:Red; width:180px" onclick="document.getElementById('divContextMenu').style.display='none';">
<asp:DataList runat="server" ID="dlstMenu" RepeatDirection="Vertical" GridLines="Both"
    Width="100%" CellPadding="0" CellSpacing="1" OnItemDataBound="dlstMenu_ItemDataBound" OnItemCommand="dlstMenu_ItemCommand">
    <ItemStyle CssClass="notselected" Height="8px" Width="70px" HorizontalAlign="Left"/>
    <SelectedItemStyle CssClass="notselected" />
    <AlternatingItemStyle CssClass="notselected" />
    <ItemTemplate>
        <asp:LinkButton runat="server" ID="btnMenu" Visible="false"></asp:LinkButton>
    </ItemTemplate>
</asp:DataList>
</div>