<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlIncidentNearMissCategory.ascx.cs" Inherits="UserControlIncidentNearMissCategory" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlIncidentNearMissCategory" runat="server" DataTextField="FLDNAME" DataValueField="FLDINCIDENTNEARMISSCATEGORYID" EnableLoadOnDemand="True"
   OnDataBound="ddlIncidentNearMissCategory_DataBound" OnTextChanged="ddlIncidentNearMissCategory_TextChanged" EmptyMessage="Type to select Incident Category" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>