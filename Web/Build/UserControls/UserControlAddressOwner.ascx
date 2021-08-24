<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlAddressOwner.ascx.cs"
    Inherits="UserControlAddressOwner" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlAddressType" runat="server" DataTextField="FLDNAME" DataValueField="FLDADDRESSCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlAddressType_DataBound" OnTextChanged="ddlAddressType_TextChanged" EmptyMessage="Type to select Owner" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>