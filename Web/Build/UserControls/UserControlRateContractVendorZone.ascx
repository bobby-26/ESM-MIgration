<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlRateContractVendorZone.ascx.cs"
    Inherits="UserControlRateContractVendorZone" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlVendorZone" runat="server" DataTextField="FLDZONEDESCRIPTION" DataValueField="FLDVENDORZONEID" EnableLoadOnDemand="True"
    OnDataBound="ddlVendorZone_DataBound" OnSelectedIndexChanged="ddlVendorZone_TextChanged" EmptyMessage="Type to select Rate Contract Vendor Zone" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
