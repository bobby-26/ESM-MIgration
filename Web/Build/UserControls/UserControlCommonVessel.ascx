<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCommonVessel.ascx.cs" Inherits="UserControlCommonVessel" %>
    
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox ID="ddlVessel" runat="server" OnTextChanged="ddlVessel_TextChanged" Width="120" EnableLoadOnDemand="True"
     DataTextField="FLDVESSELNAME"   DataValueField="FLDVESSELID" OnDataBound="ddlVessel_DataBound" EmptyMessage="Type to select vessel" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>
