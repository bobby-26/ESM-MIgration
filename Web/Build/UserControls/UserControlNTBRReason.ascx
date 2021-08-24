<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlNTBRReason.ascx.cs"
    Inherits="UserControlNTBRReason" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="uclNTBRReason" runat="server" DataTextField="FLDREASON" DataValueField="FLDREASONID" EnableLoadOnDemand="True"
    OnDataBound="uclNTBRReason_DataBound" EmptyMessage="Type to select NTBR Reason" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>