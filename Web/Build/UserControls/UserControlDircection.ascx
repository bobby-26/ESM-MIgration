<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDircection.ascx.cs"
    Inherits="UserControlDirection" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlDirection" runat="server" DataTextField="FLDDIRECTIONNAME" DataValueField="FLDDIRECTIONCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlDirection_DataBound" OnSelectedIndexChanged="ddlDirection_TextChanged" EmptyMessage="Type to select Direction" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
