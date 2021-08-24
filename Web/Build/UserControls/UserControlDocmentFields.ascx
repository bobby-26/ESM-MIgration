<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDocmentFields.ascx.cs"
    Inherits="UserControlDocmentFields" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlDocumentFields" runat="server" DataTextField="FLDFIELDNAME" DataValueField="FLDFIELDID" EnableLoadOnDemand="True"
    OnDataBound="ddlDocumentFields_DataBound" EmptyMessage="Type to select Document Fields" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
