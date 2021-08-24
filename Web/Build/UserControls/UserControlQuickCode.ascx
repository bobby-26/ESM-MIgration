<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlQuickCode.ascx.cs"
    Inherits="UserControlQuickCode" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlPriceClass" runat="server" DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKTYPECODE" EnableLoadOnDemand="True"
    OnDataBound="ddlPriceClass_DataBound" EmptyMessage="Type to select Course" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>

