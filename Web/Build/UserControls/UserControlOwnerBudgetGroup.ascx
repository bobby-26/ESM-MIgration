<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlOwnerBudgetGroup.ascx.cs" Inherits="UserControlOwnerBudgetGroup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlBudgetGroup" runat="server" DataTextField="FLDOWNERBUDGETGROUP" DataValueField="FLDOWNERBUDGETGROUPID" EnableLoadOnDemand="True"
    OnDataBound="ddlBudgetGroup_DataBound" EmptyMessage="Type to select Owner Budget Group" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>