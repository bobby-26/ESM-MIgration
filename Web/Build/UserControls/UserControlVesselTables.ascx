<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVesselTables.ascx.cs"
    Inherits="UserControlVesselTables" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlTables" runat="server" DataTextField="FLDTABLENAME" DataValueField="FLDTABLENUMBER" EnableLoadOnDemand="True"
    OnDataBound="ddlTables_DataBound" OnSelectedIndexChanged="ddlTables_TextChanged" EmptyMessage="Type to select Vessel Tables" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
