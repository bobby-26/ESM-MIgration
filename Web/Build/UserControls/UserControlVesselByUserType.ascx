﻿<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVesselByUserType.ascx.cs"
    Inherits="UserControlVesselByUserType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlVessel" runat="server" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" EnableLoadOnDemand="True"
    OnDataBound="ddlVessel_DataBound" OnTextChanged="ddlVessel_TextChanged" EmptyMessage="Type to select Vessel" Filter="Contains" MarkFirstMatch="true" >
</telerik:RadComboBox>

