<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDMRCharter.ascx.cs" Inherits="UserControlDMRCharter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlDMRCharter" runat="server" DataTextField="FLDVOYAGENO" DataValueField="FLDCHARTERER" EnableLoadOnDemand="True"
    OnDataBound="ddlDMRCharter_DataBound" OnSelectedIndexChanged="ddlDMRCharter_TextChanged" EmptyMessage="Type to select DMR Charter" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>