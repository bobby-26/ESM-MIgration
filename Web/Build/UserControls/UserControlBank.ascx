<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlBank.ascx.cs" Inherits="UserControlBank" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlBank" runat="server" DataTextField="FLDBANKNAME" DataValueField="FLDBANKID" EnableLoadOnDemand="True"
    OnTextChanged="ddlBank_TextChanged" EmptyMessage="Type to search bank" OnDataBound="ddlBank_DataBound" Filter="Contains" MarkFirstMatch="true" CssClass="Input">

</telerik:RadComboBox>