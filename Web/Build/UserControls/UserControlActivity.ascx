<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlActivity.ascx.cs"
    Inherits="UserControlActivity" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlActivity" runat="server" DataTextField="FLDACTIVITYNAME" DataValueField="FLDACTIVITYID" EnableLoadOnDemand="True"
    OnDataBound="ddlActivity_DataBound" EmptyMessage="Type to select Activity" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>