<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWFStateType.ascx.cs" Inherits="UserControlWFStateType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadComboBox DropDownPosition="Static" ID="ddlStateType" runat="server" DataTextField="FLDNAME" DataValueField="FLDSTATETYPEID" EnableLoadOnDemand="True"
    OnDataBound="ddlStateType_DataBound" EmptyMessage="Select StateType" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>




