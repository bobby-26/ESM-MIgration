<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlZone.ascx.cs"
    Inherits="UserControlZone" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlZone" runat="server" DataTextField="FLDZONE" DataValueField="FLDZONEID" EnableLoadOnDemand="True"
    OnDataBound="ddlZone_DataBound" OnSelectedIndexChanged="ddlZone_TextChanged" EmptyMessage="Type to select Zone" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>