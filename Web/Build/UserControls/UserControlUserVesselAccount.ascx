<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlUserVesselAccount.ascx.cs" Inherits="UserControls_UserControlUserVesselAccount" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlUserVesselAccount" runat="server" DataTextField="FLDDESCRIPTION" DataValueField="FLDACCOUNTID" EnableLoadOnDemand="True" Width="120px"
    OnDataBound="ddlUserVesselAccount_DataBound" OnTextChanged="ddlUserVesselAccount_TextChanged" EmptyMessage="Type to select Vessel Account" ToolTip="Type to select Vessel Account" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>