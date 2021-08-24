<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSignOn.ascx.cs"
    Inherits="UserControlSignOn" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlSignOn" runat="server" DataTextField="FLDNAME" DataValueField="FLDEMPLOYEEID" EnableLoadOnDemand="True"
    OnDataBound="ddlSignOn_DataBound" EmptyMessage="Type to select Sign On" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>