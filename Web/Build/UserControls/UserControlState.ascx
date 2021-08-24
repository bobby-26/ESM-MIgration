<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlState.ascx.cs" Inherits="UserControlState" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlState" runat="server" DataTextField="FLDSTATENAME" DataValueField="FLDSTATECODE" EnableLoadOnDemand="True"
    OnDataBound="ddlState_DataBound" OnTextChanged="ddlState_TextChanged" EmptyMessage="Type to select State" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>