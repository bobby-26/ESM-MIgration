<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPhoneCardStatus.ascx.cs" Inherits="UserControls_UserControlPhoneCardStatus" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlPhoneCard" runat="server" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlPhoneCard_DataBound" OnSelectedIndexChanged="ddlPhoneCard_TextChanged" EmptyMessage="Type to select Phone Card Status" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>