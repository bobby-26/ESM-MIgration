<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlOwnerReportType.ascx.cs"
    Inherits="UserControlOwnerReportType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlReportType" runat="server" DataTextField="FLDREPORTTYPE" DataValueField="FLDREPORTCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlReportType_DataBound" OnSelectedIndexChanged="ddlReportType_TextChanged" EmptyMessage="Type to select Owner Report Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
