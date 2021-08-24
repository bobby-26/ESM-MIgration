<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWFActivity.ascx.cs" Inherits="UserControlWFActivity" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DROPDOWNPOSITION="Static" ID="ddlActivity" runat="server" DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" EnableLoadOnDemand="true"

  OnItemDataBound="ddlActivity_ItemDataBound" OnTextChanged="ddlActivity_TextChanged"   EmptyMessage="Select Activity" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>
