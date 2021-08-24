<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlBudget.ascx.cs" Inherits="UserControlBudget" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox ID="ddlBudgetCode" runat="server" DataTextField="FLDBUDGET" DataValueField="FLDBUDGETID" CssClass="input" Width="100px" 
     OnDataBound="ddlBudgetCode_DataBound" OnTextChanged="ddlBudgetCode_TextChanged" EmptyMessage="Type to select Budget" EnableLoadOnDemand="True" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
