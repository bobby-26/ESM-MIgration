<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWorkingGearItemType.ascx.cs"
    Inherits="UserControlWorkingGearItemType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlGearItemType" runat="server" DataTextField="FLDWORKINGGEARITEMTYPENAME" DataValueField="FLDWORKINGGEARITEMTYPEID" EnableLoadOnDemand="True"
    OnDataBound="ddlGearItemType_DataBound" OnSelectedIndexChanged="ddlGearItemType_TextChanged" EmptyMessage="Type to select Working Gear Item Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
