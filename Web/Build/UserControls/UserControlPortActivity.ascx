<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPortActivity.ascx.cs" Inherits="UserControlPortActivity" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucportactivity" runat="server" DataTextField="FLDPORTACTIVITYNAME" DataValueField="FLDPORTACTIVITYID" EnableLoadOnDemand="True"
    OnDataBound="ucportactivity_DataBound" EmptyMessage="Type to select Zone" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>