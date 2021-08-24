<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMultiColumnVoyagePort.ascx.cs" Inherits="UserControlMultiColumnVoyagePort" %>

<telerik:RadComboBox DropDownPosition="Static" ID="RadMCPort" runat="server" DataTextField="FLDSEAPORTNAME" DataValueField="FLDPORTCALLID" EnableLoadOnDemand="True"
    OnDataBound="RadMCPort_DataBound" OnSelectedIndexChanged="RadMCPort_TextChanged" EmptyMessage="Type to select Port" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>