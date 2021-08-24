<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlBudgetCode.ascx.cs" Inherits="UserControlBudgetCode" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox ID="ddlBudgetCode" runat="server" OnDataBound="ddlBudgetCode_DataBound" OnTextChanged="ddlBudgetCode_TextChanged" 
    DataTextField="FLDSUBACCOUNT" DataValueField="FLDBUDGETID" CssClass="input" Width="100px" EmptyMessage="Type to select BudgetCode" EnableLoadOnDemand="True" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>