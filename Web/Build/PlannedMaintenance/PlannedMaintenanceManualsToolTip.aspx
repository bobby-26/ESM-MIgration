<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceManualsToolTip.aspx.cs"
    Inherits="PlannedMaintenanceManualsToolTip" %>

<asp:listview id="lstMappedComponent" runat="server">
    <LayoutTemplate>
        <ol>
            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
        </ol>
    </LayoutTemplate>
    <ItemTemplate>
        <li><b></b>
            <%# Eval("FLDCOMPONENTNAME") %></b></li>
    </ItemTemplate>
</asp:listview>
