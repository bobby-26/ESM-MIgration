<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlTimeSlots.ascx.cs" Inherits="UserControlTimeSlots" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlTimeSlot" runat="server"  DataTextField="FLDTIMESLOTS" DataValueField="FLDTIME" EnableLoadOnDemand="True"
   OnDataBound="ddlTimeSlot_DataBound" OnTextChanged="ddlTimeSlot_TextChanged" EmptyMessage="Type to select Course" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
