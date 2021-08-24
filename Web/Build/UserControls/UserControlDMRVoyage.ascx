<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDMRVoyage.ascx.cs" Inherits="UserControls_UserControlDMRVoyage" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlDMRCharter" runat="server" DataTextField="FLDVOYAGENO" DataValueField="FLDCHARTERER" EnableLoadOnDemand="True"
    OnDataBound="ddlDMRCharter_DataBound" OnSelectedIndexChanged="ddlDMRCharter_TextChanged" EmptyMessage="Type to select DMR Voyage" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>