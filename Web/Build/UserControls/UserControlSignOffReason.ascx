<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSignOffReason.ascx.cs" Inherits="UserControlSignOffReason" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlSignOffReason" runat="server" DataTextField="FLDREASON" DataValueField="FLDREASONID" EnableLoadOnDemand="True"
    OnDataBound="ddlSignOffReason_DataBound" OnSelectedIndexChanged="ddlSignOffReason_TextChanged" EmptyMessage="Type to select Sign Off Reason" Filter="Contains" MarkFirstMatch="true"> 
</telerik:RadComboBox>