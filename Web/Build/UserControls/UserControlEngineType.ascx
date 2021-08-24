<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlEngineType.ascx.cs"
    Inherits="UserControlEngineType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucEngineName" runat="server" DataTextField="FLDENGINENAME" DataValueField="FLDENGINEID" EnableLoadOnDemand="True"
    OnDataBound="ucEngineName_DataBound" EmptyMessage="Type to select Engine Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
