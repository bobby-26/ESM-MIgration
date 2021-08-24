<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaMainSubject.ascx.cs" Inherits="UserControlPreSeaMainSubject" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlMainSubject" runat="server" DataTextField="FLDMAINSUBJECTNAME" DataValueField="FLDMAINSUBJECTID" EnableLoadOnDemand="True"
   OnDataBound="ddlMainSubject_DataBound" OnTextChanged="ddlMainSubject_TextChanged"  EmptyMessage="Type to select Main Subject" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 
