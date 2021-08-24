<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlRateContractVendorPort.ascx.cs"
    Inherits="UserControlRateContractVendorPort" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlVendorPort" runat="server" DataTextField="FLDSEAPORTNAME" DataValueField="FLDSEAPORTID" EnableLoadOnDemand="True"
    OnDataBound="ddlVendorPort_DataBound" OnSelectedIndexChanged="ddlVendorPort_TextChanged" EmptyMessage="Type to select Rate Contract Vendor Port" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
