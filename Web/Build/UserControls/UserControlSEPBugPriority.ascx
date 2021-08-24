<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSepBugPriority.ascx.cs" Inherits="UserControlSepBugPriority" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlBugPriority" runat="server" DataTextField="FLDNAME" DataValueField="FLDID" EnableLoadOnDemand="True"
    OnDataBound="ddlBugPriority_DataBound" OnSelectedIndexChanged="ddlBugPriority_TextChanged" EmptyMessage="Type to SEP Bug Priority" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>