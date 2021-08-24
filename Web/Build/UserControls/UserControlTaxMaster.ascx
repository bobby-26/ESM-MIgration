<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlTaxMaster.ascx.cs"
    Inherits="UserControlTaxMaster" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlTaxMaster" runat="server" DataTextField="FLDDESCRIPTION" DataValueField="FLDTAXCODE" EnableLoadOnDemand="True"
     OnDataBound="ddlTaxMaster_DataBound"  EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>

