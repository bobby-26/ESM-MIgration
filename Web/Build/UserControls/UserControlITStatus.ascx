<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlITStatus.ascx.cs" Inherits="UserControlITStatus" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlITStatus" runat="server" DataTextField="FLDSTATUSNAME" DataValueField="FLDSTATUSID" EnableLoadOnDemand="True"
    OnDataBound="ddlITStatus_DataBound" OnSelectedIndexChanged="ddlITStatus_TextChanged" EmptyMessage="Type to select IT Status" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>