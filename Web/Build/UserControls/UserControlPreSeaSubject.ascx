<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaSubject.ascx.cs" Inherits="UserControlPreSeaSubject" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlSubject" runat="server" DataTextField="FLDSUBJECTNAME" DataValueField="FLDSUBJECTID" EnableLoadOnDemand="True"
    OnDataBound="ddlSubject_DataBound" OnTextChanged="ddlSubject_TextChanged" EmptyMessage="Type to select Subject" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 