<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlOwnerSubReportType.ascx.cs"
    Inherits="UserControlOwnerSubReportType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlSubReportType" runat="server" DataTextField="FLDSUBREPORTTYPE" DataValueField="FLDSUBREPORTCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlSubReportType_DataBound" OnSelectedIndexChanged="ddlSubReportType_TextChanged" EmptyMessage="Type to select Owner Sub Report Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
