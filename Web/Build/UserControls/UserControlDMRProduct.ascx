<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDMRProduct.ascx.cs" Inherits="UserControlDMRProduct" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlProduct" runat="server" DataTextField="FLDOILTYPENAME" DataValueField="FLDOILTYPECODE" EnableLoadOnDemand="True"
    OnDataBound="ddlProduct_DataBound" OnSelectedIndexChanged="ddlProduct_TextChanged" EmptyMessage="Type to select DMR Product" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>