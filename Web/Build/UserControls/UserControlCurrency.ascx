<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCurrency.ascx.cs"
    Inherits="UserControlCurrency" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadDropDownList DropDownPosition="Static" ID="ddlCurrency" runat="server" DataTextField="FLDCURRENCYCODE" DataValueField="FLDCURRENCYID"  OnDataBound="ddlCurrency_DataBound" OnSelectedIndexChanged="ddlCurrency_TextChanged">
</telerik:RadDropDownList>
