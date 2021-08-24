<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVPRSEventType.ascx.cs" Inherits="UserControlVPRSEventType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlEventType" runat="server" DataTextField="FLDEVENTTYPE" DataValueField="FLDEVENTTYPEID" EnableLoadOnDemand="True"
    OnDataBound="ddlEventType_DataBound" OnSelectedIndexChanged="ddlEventType_TextChanged" EmptyMessage="Type to select VPRS Event Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>