<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlImmediateMainCause.ascx.cs" 
Inherits="UserControlImmediateMainCause" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlImmediateMainCause" runat="server" DataTextField="FLDMAINCAUSENAME" DataValueField="FLDMAINCAUSEID" EnableLoadOnDemand="True"
    OnTextChanged="ddlImmediateMainCause_TextChanged" OnDataBound="ddlImmediateMainCause_DataBound" EmptyMessage="Type to select Immediate main Course" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 
