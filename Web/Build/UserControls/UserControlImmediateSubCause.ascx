<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlImmediateSubCause.ascx.cs" 
Inherits="UserControlImmediateSubCause" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlImmediateSubCause" runat="server" DataTextField="FLDSUBCAUSENAME" DataValueField="FLDSUBCAUSEID" EnableLoadOnDemand="True"
    OnTextChanged="ddlImmediateSubCause_TextChanged" OnDataBound="ddlImmediateSubCause_DataBound" EmptyMessage="Type to select Immediate sub Course" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>