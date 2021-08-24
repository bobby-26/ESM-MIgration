<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMappedDepartment.ascx.cs" Inherits="UserControlMappedDepartment" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlDepartment" runat="server" DataTextField="FLDDEPARTMENTNAME" DataValueField="FLDDEPARTMENTID" EnableLoadOnDemand="True"
    OnDataBound="ddlDepartment_DataBound" EmptyMessage="Type to select Mapped Department" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>