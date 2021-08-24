<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlEarMarkCompany.ascx.cs" Inherits="UserControlEarMarkCompany" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlCompany" runat="server" DataTextField="FLDSHORTCODE" DataValueField="FLDCOMPANYID" EnableLoadOnDemand="True"
    OnDataBound="ddlCompany_DataBound" OnSelectedIndexChanged="ddlCompany_TextChanged" EmptyMessage="Type to select EarMark Company" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
