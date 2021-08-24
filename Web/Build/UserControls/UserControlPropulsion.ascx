<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPropulsion.ascx.cs" Inherits="UserControls_UserControlPropulsion" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucPropulsionName" runat="server" DataTextField="FLDPROPULSIONNAME" DataValueField="FLDPROPULSIONID" EnableLoadOnDemand="True"
    OnDataBound="ucPropulsionName_DataBound"  EmptyMessage="Type to select Propulsion Name" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
