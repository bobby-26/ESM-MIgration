<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlUserName.ascx.cs"
    Inherits="UserControlUserName" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlUserName" runat="server" DataTextField="FLDUSERNAME" DataValueField="FLDUSERCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlUserName_DataBound" OnTextChanged="ddlUserName_TextChanged" EmptyMessage="Type to select User name" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>

