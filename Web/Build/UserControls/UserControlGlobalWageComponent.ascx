<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlGlobalWageComponent.ascx.cs" Inherits="UserControlGlobalWageComponent" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlGWC" runat="server" DataTextField="FLDNAME" DataValueField="FLDWAGECOMPONENTID" EnableLoadOnDemand="True"
    OnDataBound="ddlGWC_DataBound" OnTextChanged="ddlGWC_TextChanged" EmptyMessage="Type or select Component" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>