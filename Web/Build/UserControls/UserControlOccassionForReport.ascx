<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlOccassionForReport.ascx.cs" Inherits="UserControlOccassionForReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlOccassionForReport" runat="server" DataTextField="FLDOCCASION" DataValueField="FLDOCCASIONID" EnableLoadOnDemand="True"
    OnDataBound="ddlOccassionForReport_DataBound" EmptyMessage="Type to select Occassion for Report" Filter="Contains" MarkFirstMatch="true" AutoPostBack="false">
</telerik:RadComboBox>