<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaInfrastructureType.ascx.cs"
    Inherits="UserControlPreSeaInfrastructureType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlPreSeaInfrastructureType" runat="server" DataTextField="FLDINFRASTRUCTURENAME" DataValueField="FLDINFRASTRUCTUREID" EnableLoadOnDemand="True"
    OnDataBound="ddlPreSeaInfrastructureType_DataBound"  OnTextChanged="ddlPreSeaInfrastructureType_TextChanged" EmptyMessage="Type to select Infrastructure type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
