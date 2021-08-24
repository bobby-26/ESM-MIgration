<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaFees.ascx.cs" Inherits="UserControlPreSeaFees" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlPreSeaFees" runat="server" DataTextField="FLDFEESNAME" DataValueField="FLDFEESID" EnableLoadOnDemand="True"
    OnDataBound="ddlPreSeaFees_DataBound" OnTextChanged="ddlPreSeaFees_TextChanged" EmptyMessage="Type to select Fees" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>