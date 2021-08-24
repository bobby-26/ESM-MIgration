<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlOtherCompany.ascx.cs" Inherits="UserControlOtherCompany" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlOtherCompany" runat="server" DataTextField="FLDCOMPANYNAME" DataValueField="FLDCOMPANYID" EnableLoadOnDemand="True"
    OnDataBound="ddlOtherCompany_DataBound" OnSelectedIndexChanged="ddlOtherCompany_TextChanged" EmptyMessage="Type to select Other Company" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
