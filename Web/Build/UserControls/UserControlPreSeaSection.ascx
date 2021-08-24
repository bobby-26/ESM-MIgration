<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaSection.ascx.cs" Inherits="UserControlPreSeaSection" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucSection" runat="server" DataTextField="FLDBATCHSECTIONNAME" DataValueField="FLDSECTIONID" EnableLoadOnDemand="True"
    OnDataBound="ucSection_DataBound" OnTextChanged="ucSection_TextChanged" EmptyMessage="Type to select Section" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
