<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlEngineModel.ascx.cs"
    Inherits="UserControlEngineModel" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucModelName" runat="server" DataTextField="FLDMODELNAME" DataValueField="FLDENGINEID" EnableLoadOnDemand="True"
    OnDataBound="ucModelName_DataBound" EmptyMessage="Type to select Engine Model" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>