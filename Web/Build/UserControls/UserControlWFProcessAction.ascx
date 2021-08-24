<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWFProcessAction.ascx.cs" Inherits="UserControlWFProcessAction" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DROPDOWNPOSITION="Static" ID="ddlProcessAction" runat="server" DataTextField="FLDNAME" DataValueField="FLDACTIONID" EnableLoadOnDemand="true"

 OnItemDataBound="ddlProcessAction_ItemDataBound" OnTextChanged="ddlProcessAction_TextChanged"    EmptyMessage="Select Process Action" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>

