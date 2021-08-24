<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlInstitution.ascx.cs"
    Inherits="UserControlInstitution" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlInstitution" runat="server" DataTextField="FLDNAME" DataValueField="FLDINSTITUTIONID" EnableLoadOnDemand="True"
    OnDataBound="ddlInstitution_DataBound" EmptyMessage="Type to select Institution" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>