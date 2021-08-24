<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSize.ascx.cs" 
Inherits="UserControls_UserControlSize" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlSize" runat="server" DataTextField="FLDSIZENAME" DataValueField="FLDSIZEID" EnableLoadOnDemand="True"
    OnDataBound="ddlSize_DataBound" EmptyMessage="Type to select Size" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 