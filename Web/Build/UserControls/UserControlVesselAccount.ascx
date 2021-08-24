<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVesselAccount.ascx.cs"
    Inherits="UserControlVesselAccount" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlVesselAccount" runat="server" DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID" EnableLoadOnDemand="True"
    OnDataBound="ddlVesselAccount_DataBound" OnTextChanged="ddlVesselAccount_TextChanged" EmptyMessage="Type to select Vessel account" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>

