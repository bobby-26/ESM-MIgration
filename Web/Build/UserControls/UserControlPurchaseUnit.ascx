<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPurchaseUnit.ascx.cs" Inherits="UserControlPurchaseUnit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlUnit" runat="server" DataTextField="FLDUNITNAME" DataValueField="FLDUNITID" EnableLoadOnDemand="True"
    OnDataBound="ddlUnit_DataBound" EmptyMessage="Type to select Unit" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>

