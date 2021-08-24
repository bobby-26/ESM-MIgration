<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMoreLinks.ascx.cs" Inherits="UserControlMoreLinks" %>
<div id="divMoreLinks" class="cntxMorelinksSelect" style="height:auto; width:auto;text-align:right; " onclick="document.getElementById('divMoreLinks').style.display='none';">
<asp:ImageButton runat="server" ID="imgmoreclose" ImageUrl="<%$ PhoenixTheme:images/xft_close_icon.gif %>" OnClick="imgmoreclose_Click" ToolTip="Close" />
<asp:DataList runat="server" ID="dlstMenu" RepeatDirection="Vertical" GridLines="Both"
    Width="100%" CellPadding="0" CellSpacing="0" OnItemDataBound="dlstMenu_ItemDataBound" OnItemCommand="dlstMenu_ItemCommand">
    <ItemStyle CssClass="notselected" Height="0px" Width="0px" HorizontalAlign="Left"/>
    <SelectedItemStyle CssClass="notselected" />
    <AlternatingItemStyle CssClass="notselected" />
    <ItemTemplate>
        <asp:LinkButton runat="server" ID="btnMenu" Visible="false" CssClass="navMorelinks"></asp:LinkButton>
    </ItemTemplate>
</asp:DataList>
</div>