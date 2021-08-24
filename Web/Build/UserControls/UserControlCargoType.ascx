<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCargoType.ascx.cs"
    Inherits="UserControlCargoType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlCargoType" runat="server" DataTextField="FLDCARGOTYPENAME" DataValueField="FLDCARGOTYPECODE" EnableLoadOnDemand="True"
    OnDataBound="ddlCargoType_DataBound" OnSelectedIndexChanged="ddlCargoType_TextChanged" EmptyMessage="Type to select Cargo Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
