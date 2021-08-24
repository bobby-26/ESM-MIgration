<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlContractCompany.ascx.cs" Inherits="UserControlContractCompany" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlContractCompany" runat="server" DataTextField="FLDCOMPANYNAME" DataValueField="FLDCOMPANYID" EnableLoadOnDemand="True"
    OnDataBound="ddlContractCompany_DataBound" OnSelectedIndexChanged="ddlContractCompany_TextChanged" EmptyMessage="Type to select Contract Company" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>