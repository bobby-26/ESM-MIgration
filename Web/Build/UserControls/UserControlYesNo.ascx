<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlYesNo.ascx.cs"
    Inherits="UserControlYesNo" %>
<%--<asp:RadioButtonList ID="rblYesNo" runat="server" RepeatDirection="Horizontal">
    <asp:ListItem Value="YES">Yes</asp:ListItem>
    <asp:ListItem Value="NO">No</asp:ListItem>
</asp:RadioButtonList>--%>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadRadioButtonList runat="server" ID="rblYesNo" Direction="Horizontal">
    <Items>
        <telerik:ButtonListItem Text="Yes" Value="YES"/>
        <telerik:ButtonListItem Text="No" Value="NO"/> 
    </Items> 
</telerik:RadRadioButtonList>
