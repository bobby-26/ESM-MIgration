<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDepartment.ascx.cs" Inherits="UserControlDepartment" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlDepartment" runat="server" DataTextField="FLDDEPARTMENTNAME" DataValueField="FLDDEPARTMENTID" EnableLoadOnDemand="True"
    OnSelectedIndexChanged="ddlDepartment_TextChanged" OnDataBound="ddlDepartment_DataBound" EmptyMessage="Type to select Department" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
