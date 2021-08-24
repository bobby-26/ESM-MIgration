<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDesignation.ascx.cs" Inherits="UserControlDesignation" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucDesignation" runat="server" DataTextField="FLDDESIGNATIONNAME" DataValueField="FLDDESIGNATIONID" EnableLoadOnDemand="True"
    OnDataBound="ucDesignation_DataBound" EmptyMessage="Type to select Designation" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>