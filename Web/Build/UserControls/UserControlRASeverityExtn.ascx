<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlRASeverityExtn.ascx.cs" Inherits="UserControlRASeverityExtn" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlSeverity" runat="server" DataTextField="FLDSEVERITY" DataValueField="FLDSEVERITYID" EnableLoadOnDemand="True"
    OnDataBound="ddlSeverity_DataBound" EmptyMessage="Type to select RA Severity" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
