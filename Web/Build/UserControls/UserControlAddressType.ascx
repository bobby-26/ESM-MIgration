<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlAddressType.ascx.cs"
    Inherits="UserControlAddressType" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlAddressType" runat="server" DataTextField="FLDNAME" DataValueField="FLDADDRESSCODE" EnableLoadOnDemand="True" Width="120px"
    OnDataBound="ddlAddressType_DataBound" OnTextChanged="ddlAddressType_TextChanged" EmptyMessage="Type to select Addresstype" ToolTip="Type to select Addresstype" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>