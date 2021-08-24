<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWFProcessState.ascx.cs" Inherits="UserControlWFProcessState" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox  DROPDOWNPOSITION="Static" ID="ddlProcessState" runat="server" DataTextField="FLDNAME" DataValueField="FLDSTATEID" EnableLoadOnDemand="true"
     OnItemDataBound="ddlProcessState_ItemDataBound" OnTextChanged="ddlProcessState_TextChanged"  EmptyMessage="Select Process State" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>
