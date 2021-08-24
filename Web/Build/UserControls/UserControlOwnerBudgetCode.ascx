<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlOwnerBudgetCode.ascx.cs" Inherits="UserControlOwnerBudgetCode" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlBudgetGroup" runat="server" DataTextField="FLDOWNERBUDGETGROUP" DataValueField="FLDOWNERBUDGETID" EnableLoadOnDemand="True"
    OnDataBound="ddlBudgetGroup_DataBound" EmptyMessage="Type to select Owner Budget Code" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>