<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlAddressCharterer.ascx.cs" Inherits="UserControlAddressCharterer" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlAddressType" runat="server" DataTextField="FLDNAME" DataValueField="FLDADDRESSCODE" EnableLoadOnDemand="True" CssClass="input"
    OnDataBound="ddlAddressType_DataBound" OnTextChanged="ddlAddressType_TextChanged" EmptyMessage="Type to select Charterer" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
