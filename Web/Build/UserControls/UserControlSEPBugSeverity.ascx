<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSepBugSeverity.ascx.cs" Inherits="UserControlSepBugSeverity" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlBugSeverity" runat="server" DataTextField="FLDNAME" DataValueField="FLDID" EnableLoadOnDemand="True"
    OnDataBound="ddlBugSeverity_DataBound" OnSelectedIndexChanged="ddlBugSeverity_TextChanged" EmptyMessage="Type to select SEP Bug Severity" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>