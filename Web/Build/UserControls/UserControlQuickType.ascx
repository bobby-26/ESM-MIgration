<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlQuickType.ascx.cs"
    Inherits="UserControlQuickType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlQuickType" runat="server" DataTextField="FLDQUICKTYPENAME" DataValueField="FLDQUICKTYPECODE" EnableLoadOnDemand="True"
    OnDataBound="ddlQuickType_DataBound" OnTextChanged="ddlQuickType_TextChanged" EmptyMessage="Type to select Course" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
    