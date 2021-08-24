<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMovement.ascx.cs" Inherits="UserControlMovement" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlMovement" runat="server" DataTextField="FLDNAME" DataValueField="FLDMOVEMENTID" EnableLoadOnDemand="True"
    OnDataBound="ddlMovement_DataBound" OnTextChanged="ddlMovement_TextChanged" EmptyMessage="Type or select Movement" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>
