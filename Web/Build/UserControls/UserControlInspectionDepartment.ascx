<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlInspectionDepartment.ascx.cs" Inherits="UserControlInspectionDepartment" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlDepartment" runat="server" DataTextField="FLDDEPARTMENTNAME" DataValueField="FLDDEPARTMENTID" EnableLoadOnDemand="True"
    OnDataBound="ddlDepartment_DataBound" OnSelectedIndexChanged="ddlDepartment_TextChanged" EmptyMessage="Type to select Department" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>