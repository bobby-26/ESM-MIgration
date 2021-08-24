<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPool.ascx.cs"
    Inherits="UserControlPool" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlPool" runat="server" DataTextField="FLDPOOLNAME" DataValueField="FLDPOOLID" EnableLoadOnDemand="True"
    OnDataBound="ddlPool_DataBound" EmptyMessage="Type to select Pool" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>