<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlRASeverity.ascx.cs" Inherits="UserControlRASeverity" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlSeverity" runat="server" DataTextField="FLDSEVERITY" DataValueField="FLDSEVERITYID" EnableLoadOnDemand="True"
    OnDataBound="ddlSeverity_DataBound" EmptyMessage="Type to select RA Severity" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
