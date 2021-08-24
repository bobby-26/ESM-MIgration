<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlTravelReason.ascx.cs" Inherits="UserControlTravelReason" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddltravelreason" runat="server" DataTextField="FLDREASON" DataValueField="FLDTRAVELREASONID" EnableLoadOnDemand="True"
    OnDataBound="ddltravelreason_DataBound" OnTextChanged="ddlTravelReason_TextChanged" EmptyMessage="Type to select Travel reason" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>


