<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMaritalStatus.ascx.cs"
    Inherits="UserControlMaritalStatus" %>
 
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlMaritalStatus" runat="server" DataTextField="FLDDESCRIPTION" DataValueField="FLDMARITALSTATUSCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlMaritalStatus_DataBound" EmptyMessage="Type to select Marital Status" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>