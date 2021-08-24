<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMaker.ascx.cs"
    Inherits="UserControlMaker" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlAddress" runat="server" DataTextField="FLDNAME" DataValueField="FLDADDRESSCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlAddress_DataBound" EmptyMessage="Type to select Maker" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
