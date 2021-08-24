<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCertificateCategory.ascx.cs" Inherits="UserControlCertificateCategory" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlCertificateCategory" runat="server" DataTextField="FLDCATEGORYNAME" DataValueField="FLDCATEGORYID" EnableLoadOnDemand="True"
    OnDataBound="ddlCertificateCategory_DataBound" OnSelectedIndexChanged="ddlCertificateCategory_TextChanged" EmptyMessage="Type to select Certificate Category" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
