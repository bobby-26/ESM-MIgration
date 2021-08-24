<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWFProcess.ascx.cs" Inherits="UserControlWFProcess" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlProcess" runat="server" DataTextField="FLDNAME" DataValueField="FLDPROCESSID" EnableLoadOnDemand="True" 
    OnDataBound="ddlProcess_DataBound"   EmptyMessage="Select Process" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>

