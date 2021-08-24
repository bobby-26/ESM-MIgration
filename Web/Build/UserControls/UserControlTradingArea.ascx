<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlTradingArea.ascx.cs" Inherits="UserControls_UserControlTradingArea_" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="UserControlTradingArea" runat="server" DataTextField="FLDTRADINGAREANAME" DataValueField="FLDTRADINGAREAID" EnableLoadOnDemand="True"
    OnDataBound="ucTradingArea_DataBound"  EmptyMessage="Type to select Trading area" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
