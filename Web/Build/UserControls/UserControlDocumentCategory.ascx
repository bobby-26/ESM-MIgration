<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDocumentCategory.ascx.cs" Inherits="UserControlDocumentCategory" %> 

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox  ID="ddlDocumentCategory" runat="server" DataTextField="FLDCATEGORYNAME" DataValueField="FLDDOCUMENTCATEGORYID" EnableLoadOnDemand="True"
    OnDataBound="ddlDocumentCategory_DataBound" OnSelectedIndexChanged="ddlDocumentCategory_TextChanged" EmptyMessage="Type to select Document Category" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>