<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPurchaseRules.ascx.cs" Inherits="UserControlPurchaseRules" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<telerik:RadComboBox DropDownPosition="Static" ID="ddlPurchaseRules" runat="server" DataTextField="FLDRULENAME" DataValueField="FLDRULEID" EnableLoadOnDemand="True" 
 OnItemDataBound="ddlPurchaseRules_ItemDataBound" EmptyMessage="Select Purchase Rules" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>

