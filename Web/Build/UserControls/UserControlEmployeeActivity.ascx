<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlEmployeeActivity.ascx.cs" Inherits="UserControlEmployeeActivity" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlEmpActivity" runat="server" DataTextField="FLDACTIVITYNAME" DataValueField="FLDACTIVITYID" EnableLoadOnDemand="True"
    OnDataBound="ddlEmpActivity_DataBound" OnSelectedIndexChanged="ddlEmpActivity_TextChanged" EmptyMessage="Type to select Employee Activity" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>