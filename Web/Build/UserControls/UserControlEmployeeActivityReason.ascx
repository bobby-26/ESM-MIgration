<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlEmployeeActivityReason.ascx.cs" Inherits="UserControlEmployeeActivityReason" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlEmpActivityReason" runat="server" DataTextField="FLDREASONNAME" DataValueField="FLDREASONID" EnableLoadOnDemand="True"
    OnDataBound="ddlEmpActivityReason_DataBound" OnSelectedIndexChanged="ddlEmpActivityReason_TextChanged" EmptyMessage="Type to select Employee Activity Reason" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>