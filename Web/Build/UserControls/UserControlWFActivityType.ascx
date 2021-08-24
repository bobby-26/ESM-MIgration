<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWFActivityType.ascx.cs" Inherits="UserControlWFActivityType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>



<telerik:RadComboBox DropDownPosition="Static" ID="ddlActivitytype" runat="server" DataTextField="FLDNAME" DataValueField="FLDACTIVITYTYPEID" EnableLoadOnDemand="True"
    OnDataBound="ddlActivitytype_DataBound" EmptyMessage="Select Activity Type" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>


