<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCalenderMonths.ascx.cs" Inherits="UserControls_UserControlCalenderMonths" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlCalenderMonth" runat="server" DataTextField="FLDMONTHNAME" DataValueField="FLDMONTH" EnableLoadOnDemand="True"
    OnDataBound="ddlCalenderMonth_DataBound" OnSelectedIndexChanged="ddlCalenderMonth_TextChanged" EmptyMessage="Type to select Month" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>