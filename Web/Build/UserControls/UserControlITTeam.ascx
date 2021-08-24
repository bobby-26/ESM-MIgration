<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlITTeam.ascx.cs" Inherits="UserControlITTeam" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlITTeam" runat="server" DataTextField="FLDNAME" DataValueField="FLDITTEAMID" EnableLoadOnDemand="True"
    OnDataBound="ddlITTeam_DataBound" EmptyMessage="Type to select IT Team" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>