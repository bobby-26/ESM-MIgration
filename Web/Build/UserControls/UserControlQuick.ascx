<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlQuick.ascx.cs"
    Inherits="UserControlQuick" %>
 
 <%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlQuick" runat="server" DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" EnableLoadOnDemand="True"
    OnTextChanged="ddlQuick_TextChanged" OnDataBound="ddlQuick_DataBound"  EmptyMessage="Type to select" Filter="Contains" ShowDropDownOnTextboxClick="true" ShowToggleImage="true" MarkFirstMatch="true" ExpandDirection="Down">
</telerik:RadComboBox><asp:LinkButton ID="lnkQuickEdit" runat="server" Text="Q" Font-Size="10px" ForeColor="Red"></asp:LinkButton>
   