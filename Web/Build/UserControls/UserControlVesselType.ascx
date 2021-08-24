<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVesselType.ascx.cs"
    Inherits="UserControlVesselType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlVesselType" runat="server" DataTextField="FLDTYPEDESCRIPTION" DataValueField="FLDVESSELTYPEID" EnableLoadOnDemand="True"
    OnDataBound="ddlVesselType_DataBound" OnSelectedIndexChanged="ddlVesselType_TextChanged" EmptyMessage="Type to select Vessel Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
