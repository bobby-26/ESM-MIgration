<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDocumentVaccination.ascx.cs" Inherits="UserControlDocumentVaccination" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlDocumentVaccination" runat="server" DataTextField="FLDDOCUMENTNAME" DataValueField="FLDDOCUMENTID" EnableLoadOnDemand="True"
    OnDataBound="ddlDocumentVaccination_DataBound" OnSelectedIndexChanged="ddlDocumentVaccination_TextChanged" EmptyMessage="Type to select Document Vaccination" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
