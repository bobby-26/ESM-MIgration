<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWFProcessActivity.ascx.cs" Inherits="UserControlWFProcessActivity" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DROPDOWNPOSITION="Static" ID="ddlProcessActivity" runat="server" DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" EnableLoadOnDemand="true"
    OnItemDataBound="ddlProcessActivity_ItemDataBound" OnTextChanged="ddlProcessActivity_TextChanged" EmptyMessage="Select Process Activity" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>

