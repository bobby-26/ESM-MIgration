<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWorkingGearSize.ascx.cs" 
Inherits="UserControls_UserControlWorkingGearSize" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlWorkingGearSize" runat="server" DataTextField="FLDSIZENAME" DataValueField="FLDSIZEID" EnableLoadOnDemand="True"
    OnDataBound="ddlWorkingGearSize_DataBound" OnSelectedIndexChanged="ddlWorkingGearSize_TextChanged" EmptyMessage="Type to select Working Gear Size" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>