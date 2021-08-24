<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSEPMailUsername.ascx.cs" Inherits="UserControlSEPMailUsername" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlModuleList" runat="server" DataTextField="FLDMAILUSERNAME" DataValueField="FLDMAILUSERNAME" EnableLoadOnDemand="True"
    OnDataBound="ddlModuleList_DataBound" OnSelectedIndexChanged="ddlModuleList_TextChanged" EmptyMessage="Type to select SEP Mail Username" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>