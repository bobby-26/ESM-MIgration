<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSepModuleList.ascx.cs" Inherits="UserControlSepModuleList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlModuleList" runat="server" DataTextField="FLDMODULENAME" DataValueField="FLDMODULEID" EnableLoadOnDemand="True"
    OnDataBound="ddlModuleList_DataBound" OnSelectedIndexChanged="ddlModuleList_TextChanged" EmptyMessage="Type to select SEP Module List" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>