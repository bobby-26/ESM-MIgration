<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaBatchSubject.ascx.cs" Inherits="UserControlPreSeaBatchSubject" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlSubject" runat="server" DataTextField="FLDSUBJECTNAME" DataValueField="FLDSUBJECTID" EnableLoadOnDemand="True"
    OnDataBound="ddlSubject_DataBound" OnSelectedIndexChanged="ddlSubject_TextChanged" EmptyMessage="Type to select PreSea Batch Subject" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>