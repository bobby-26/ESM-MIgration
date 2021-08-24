<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSubDepartment.ascx.cs" Inherits="UserControlSubDepartment" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlSubDepartment" runat="server" DataTextField="FLDSUBDEPARTMENTNAME" DataValueField="FLDMAPPINGID" EnableLoadOnDemand="True"
   OnDataBound="ddlSubDepartment_DataBound" EmptyMessage="Type to select Sub Department" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>

