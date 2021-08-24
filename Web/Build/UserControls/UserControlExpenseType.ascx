<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlExpenseType.ascx.cs"
    Inherits="UserControlExpenseType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlExpenseType" runat="server" DataTextField="FLDCATEGORY" DataValueField="FLDCATEGORYID" EnableLoadOnDemand="True"
    OnDataBound="ddlExpenseType_DataBound" OnSelectedIndexChanged="ddlExpenseType_TextChanged" EmptyMessage="Type to select Expense Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
