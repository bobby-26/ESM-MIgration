<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlRateContractVendor.ascx.cs"
    Inherits="UserControlRateContractVendor" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlVendor" runat="server" DataTextField="FLDNAME" DataValueField="FLDVENDORID" EnableLoadOnDemand="True"
    OnDataBound="ddlVendor_DataBound" OnSelectedIndexChanged="ddlVendor_TextChanged" EmptyMessage="Type to select Rate Contract Vendor" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
