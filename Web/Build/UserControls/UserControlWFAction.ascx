<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWFAction.ascx.cs" Inherits="UserControlWFAction" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DROPDOWNPOSITION="Static" ID="ddlAction" runat="server" DataTextField="FLDNAME" DataValueField="FLDACTIONID" EnableLoadOnDemand="true"

 OnItemDataBound="ddlAction_ItemDataBound" OnTextChanged="ddlAction_TextChanged"   EmptyMessage="Select Action" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>
