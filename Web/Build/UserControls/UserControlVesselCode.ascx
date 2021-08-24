<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVesselCode.ascx.cs"
    Inherits="UserControlVesselCode" %>
 
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlVesselCode" runat="server" DataTextField="FLDVESSELCODE" DataValueField="FLDVESSELCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlVesselCode_DataBound"  OnTextChanged="ddlVesselCode_TextChanged" EmptyMessage="Type to select Vessel code" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>

