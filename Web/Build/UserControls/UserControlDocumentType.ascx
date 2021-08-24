<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDocumentType.ascx.cs"
    Inherits="UserControlDocumentType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlDocumentType" runat="server" DataTextField="FLDDOCUMENTNAME" DataValueField="FLDDOCUMENTID" EnableLoadOnDemand="True"
    OnDataBound="ddlDocumentType_DataBound" OnSelectedIndexChanged="ddlDocumentType_TextChanged" EmptyMessage="Type to select Document Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>