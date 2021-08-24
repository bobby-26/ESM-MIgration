<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlBudgetBillingItem.ascx.cs" Inherits="UserControlBudgetBillingItem" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlBudgetBillingItem" runat="server" DataTextField="FLDBILLINGITEMDESCRIPTION" DataValueField="FLDBUDGETBILLINGID" EnableLoadOnDemand="True"
    OnDataBound="ddlBudgetBillingItem_DataBound" OnSelectedIndexChanged="ddlBudgetBillingItem_TextChanged" EmptyMessage="Type to select Billing Item" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>

