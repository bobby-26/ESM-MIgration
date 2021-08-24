<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPhoneCard.ascx.cs" Inherits="UserControls_UserControlPhoneCard" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlPhoneCard" runat="server" DataTextField="FLDNAME" DataValueField="FLDSTOREITEMID" EnableLoadOnDemand="True"
    OnDataBound="ddlPhoneCard_DataBound" OnSelectedIndexChanged="ddlPhoneCard_TextChanged" EmptyMessage="Type to select Phone Card" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>