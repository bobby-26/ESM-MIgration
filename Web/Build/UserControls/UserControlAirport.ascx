<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlAirport.ascx.cs" Inherits="UserControlAirport" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlAirport" runat="server" DataTextField="FLDAIRPORTNAME" DataValueField="FLDAIRPORTID" EnableLoadOnDemand="True"
    OnDataBound="ddlAirport_DataBound"  EmptyMessage="Type to select Airport" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>