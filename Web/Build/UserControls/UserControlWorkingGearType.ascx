<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWorkingGearType.ascx.cs"
    Inherits="UserControlWorkingGearType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlGearType" runat="server" DataTextField="FLDWORKINGGEARTYPENAME" DataValueField="FLDWORKINGGEARTYPEID" EnableLoadOnDemand="True"
    OnDataBound="ddlGearType_TextChanged" OnSelectedIndexChanged="ddlGearType_TextChanged" EmptyMessage="Type to select Working Gear Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
