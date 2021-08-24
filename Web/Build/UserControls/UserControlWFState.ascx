<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWFState.ascx.cs" Inherits="UserControlWFState" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DROPDOWNPOSITION="Static" ID="ddlState" runat="server" DataTextField="FLDNAME" DataValueField="FLDSTATEID" EnableLoadOnDemand="true"

 OnItemDataBound="ddlState_DataBound" OnTextChanged="ddlState_TextChanged"   EmptyMessage="Select State" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>