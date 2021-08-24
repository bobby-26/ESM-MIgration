<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlClinic.ascx.cs" Inherits="UserControlClinic" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucClinic" runat="server" DataTextField="FLDCLINICNAME" DataValueField="FLDCLINICCODE" EnableLoadOnDemand="True"
    OnDataBound="ucClinic_DataBound" OnSelectedIndexChanged="ucClinic_TextChanged" EmptyMessage="Type to select Clinic" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>