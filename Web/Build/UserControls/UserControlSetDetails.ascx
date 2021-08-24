<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSetDetails.ascx.cs"
    Inherits="UserControlSetDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlSetDetails" runat="server" DataTextField="FLDSETDETAILNAME" DataValueField="FLDSETDETAILSID" EnableLoadOnDemand="True"
    OnDataBound="ddlSetDetails_DataBound" OnSelectedIndexChanged="ddlSetDetails_TextChanged" EmptyMessage="Type to select Set Details" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
