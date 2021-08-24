<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSEPTeamMembers.ascx.cs"
    Inherits="UserControlSEPTeamMembers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlTeamMembers" runat="server" DataTextField="FLDNAME" DataValueField="FLDDEVELOPERID" EnableLoadOnDemand="True"
    OnDataBound="ddlTeamMembers_DataBound" OnSelectedIndexChanged="ddlTeamMembers_TextChanged" EmptyMessage="Type to select SEP Team Members" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
