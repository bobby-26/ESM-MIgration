<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDocuments.ascx.cs"
    Inherits="UserControlDocuments" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlDocuments" runat="server" DataTextField="FLDDOCUMENTNAME" DataValueField="FLDDOCUMENTID" EnableLoadOnDemand="True"
    OnDataBound="ddlDocuments_DataBound" OnSelectedIndexChanged="ddlDocuments_TextChanged" EmptyMessage="Type to select Documents" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
