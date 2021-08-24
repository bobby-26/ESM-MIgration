<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDPClass.ascx.cs" Inherits="UserControls_UserControlDPClass" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucDPClassName" runat="server" DataTextField="FLDDPCLASSNAME" DataValueField="FLDDPCLASSID" EnableLoadOnDemand="True"
    OnDataBound="ucDPClassName_DataBound" EmptyMessage="Type to select DP Class" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>