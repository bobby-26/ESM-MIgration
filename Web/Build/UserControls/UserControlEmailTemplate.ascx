<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlEmailTemplate.ascx.cs"
    Inherits="UserControlEmailTemplate" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlEmailTemplate" runat="server" DataTextField="FLDTEMPLATENAME" DataValueField="FLDTEMPLATEID" EnableLoadOnDemand="True"
    OnDataBound="ddlEmailTemplate_DataBound" OnSelectedIndexChanged="ddlEmailTemplate_TextChanged" EmptyMessage="Type to select Email Template" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
