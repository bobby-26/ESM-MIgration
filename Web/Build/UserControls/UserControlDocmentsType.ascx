<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDocmentsType.ascx.cs"
    Inherits="UserControlDocmentsType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlDocumentType" runat="server" DataTextField="FLDDOCUMENTTYPE" DataValueField="FLDDOCUMENTTYPEID" EnableLoadOnDemand="True"
    OnDataBound="ddlDocumentType_DataBound" EmptyMessage="Type to select Documents Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>