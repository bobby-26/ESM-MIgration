<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlOilType.ascx.cs"
    Inherits="UserControlOilType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlOilType" runat="server" DataTextField="FLDOILTYPENAME" DataValueField="FLDOILTYPECODE" EnableLoadOnDemand="True"
    OnDataBound="ddlOilType_DataBound" OnSelectedIndexChanged="ddlOilType_TextChanged" EmptyMessage="Type to select Oil Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
