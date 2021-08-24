<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlProjectCode.ascx.cs" Inherits="UserControls_UserControlProjectCode" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlProjectCode" runat="server" DataTextField="FLDPROJECTCODE" DataValueField="FLDID" EnableLoadOnDemand="True"
    OnTextChanged="ddlProjectCode_TextChanged" OnDataBound="ddlProjectCode_DataBound"  EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>