<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVesselMappingCurrency.ascx.cs"
    Inherits="UserControlVesselMappingCurrency" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<telerik:RadComboBox DropDownPosition="Static" ID="ddlCurrency" runat="server" DataTextField="FLDCURRENCYCODE" DataValueField="FLDCURRENCYID" EnableLoadOnDemand="True"
    OnDataBound="ddlCurrency_DataBound" OnTextChanged="ddlCurrency_TextChanged" EmptyMessage="Type to select Currency" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
