<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWFActionType.ascx.cs" Inherits="UserControlWFActionType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadComboBox DropDownPosition="Static" ID="ddlActiontype" runat="server" DataTextField="FLDNAME" DataValueField="FLDACTIONTYPEID" EnableLoadOnDemand="True"
    OnDataBound="ddlActiontype_DataBound" EmptyMessage="Select ActionType " Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>

