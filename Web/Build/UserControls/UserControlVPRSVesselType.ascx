<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVPRSVesselType.ascx.cs" Inherits="UserControlVPRSVesselType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlVesselType" runat="server" DataTextField="FLDSHORTNAME" DataValueField="FLDVESSELTYPEID" EnableLoadOnDemand="True"
    OnDataBound="ddlVesselType_DataBound" OnSelectedIndexChanged="ddlVesselType_TextChanged" EmptyMessage="Type to select VPRS Vessel Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
