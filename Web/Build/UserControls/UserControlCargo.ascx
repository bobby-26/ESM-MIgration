<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCargo.ascx.cs"
    Inherits="UserControlCargo" %>
    
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlCargo" runat="server" DataTextField="FLDCARGONAME" DataValueField="FLDCARGOCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlCargo_DataBound" OnSelectedIndexChanged="ddlCargo_TextChanged" EmptyMessage="Type to select Cargo" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
