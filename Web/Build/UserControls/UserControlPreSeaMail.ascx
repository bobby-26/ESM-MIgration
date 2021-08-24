<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaMail.ascx.cs"
    Inherits="UserControlPreSeaMail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlMailTemplate" runat="server" DataTextField="FLDEMAILTEMPLATENAME" DataValueField="FLDEMAILTEMPLATECODE" EnableLoadOnDemand="True"
    OnDataBound="ddlMailTemplate_DataBound" OnTextChanged="ddlMailTemplate_TextChanged" EmptyMessage="Type to select Mail" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
