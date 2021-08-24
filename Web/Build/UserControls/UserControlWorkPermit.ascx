<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWorkPermit.ascx.cs"
    Inherits="UserControlWorkPermit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlWorkPermit" runat="server" DataTextField="FLDCAPTION" DataValueField="FLDFORMID" EnableLoadOnDemand="True"
    OnDataBound="ddlWorkPermit_DataBound" OnSelectedIndexChanged="ddlWorkPermit_TextChanged" EmptyMessage="Type to select Work Permit" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
