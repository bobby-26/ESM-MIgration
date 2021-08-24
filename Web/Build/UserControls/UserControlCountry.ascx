<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCountry.ascx.cs" Inherits="UserControlCountry" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlCountry" runat="server" DataTextField="FLDCOUNTRYNAME" DataValueField="FLDCOUNTRYCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlCountry_DataBound" OnTextChanged="ddlCountry_TextChanged" EmptyMessage="Type to select Country" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>