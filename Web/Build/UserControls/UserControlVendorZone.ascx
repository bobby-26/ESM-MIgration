<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVendorZone.ascx.cs" Inherits="UserControlVendorZone" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlVendorZone" runat="server" DataTextField="FLDZONECODE" DataValueField="FLDVENDORZONEID" EnableLoadOnDemand="True"
    OnDataBound="ddlVendorZone_DataBound" OnTextChanged="ddlVendorZone_TextChanged" EmptyMessage="Type to select Vendor zone" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
