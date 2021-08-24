<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlOwnersVessel.ascx.cs" Inherits="UserControlOwnersVessel" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlVessel" runat="server" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" EnableLoadOnDemand="True"
    OnDataBound="ddlVessel_DataBound" OnSelectedIndexChanged="ddlVessel_TextChanged" EmptyMessage="Type to select Owner Vessel" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>