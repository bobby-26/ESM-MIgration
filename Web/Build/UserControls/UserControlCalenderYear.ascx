<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCalenderYear.ascx.cs" Inherits="UserControls_UserControlCalenderYear" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlCalenderYear" runat="server" DataTextField="FLDYEAR" DataValueField="FLDYEAR" EnableLoadOnDemand="True"
    OnDataBound="ddlCalenderYear_DataBound" OnSelectedIndexChanged="ddlCalenderYear_TextChanged" EmptyMessage="Type to select Year" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>