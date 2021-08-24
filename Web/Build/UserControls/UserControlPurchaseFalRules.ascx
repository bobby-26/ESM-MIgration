<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPurchaseFalRules.ascx.cs" Inherits="UserControlPurchaseFalRules" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlFalRules" runat="server" DataTextField="FLDNAME" DataValueField="FLDAPPROVALRULEID" EnableLoadOnDemand="True" 
    OnDataBound="ddlFalRules_DataBound"   EmptyMessage="Select Process Fal Rules" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>
