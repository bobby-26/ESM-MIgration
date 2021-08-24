<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlRateContractVendorProduct.ascx.cs"
    Inherits="UserControlRateContractVendorProduct" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlVendorProduct" runat="server" DataTextField="FLDPRODUCTNAME" DataValueField="FLDVENDORPRODUCTID" EnableLoadOnDemand="True"
    OnDataBound="ddlVendorProduct_DataBound" OnSelectedIndexChanged="ddlVendorProduct_TextChanged" EmptyMessage="Type to select Rate Contract Vendor Product" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
