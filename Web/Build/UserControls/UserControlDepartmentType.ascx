<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDepartmentType.ascx.cs"
    Inherits="UserControlDepartmentType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlDepartmentType" runat="server" DataTextField="FLDDEPARTMENTTYPENAME" DataValueField="FLDDEPARTMENTTYPEID" EnableLoadOnDemand="True"
    OnDataBound="ddlDepartmentType_DataBound" EmptyMessage="Type to select Department Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
