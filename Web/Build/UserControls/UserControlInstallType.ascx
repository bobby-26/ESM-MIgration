<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlInstallType.ascx.cs" Inherits="UserControls_UserControlInstallType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucInstallType" runat="server" DataTextField="FLDTYPEOFINSTALLATIONNAME" DataValueField="FLDTYPEOFINSTALLATIONID" EnableLoadOnDemand="True"
    OnDataBound="ucInstallType_DataBound" EmptyMessage="Type to select Install Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>