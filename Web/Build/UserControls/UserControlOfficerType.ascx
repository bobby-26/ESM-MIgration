<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlOfficerType.ascx.cs"
    Inherits="UserControlOfficerType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlOfficerType" runat="server" DataTextField="FLDOFFICERTYPENAME" DataValueField="FLDOFFICERTYPE" EnableLoadOnDemand="True"
    OnDataBound="ddlOfficerType_DataBound" OnSelectedIndexChanged="ddlOfficerType_TextChanged" EmptyMessage="Type to select Officer Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>