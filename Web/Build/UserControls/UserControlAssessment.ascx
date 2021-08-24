<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlAssessment.ascx.cs" Inherits="UserControlAssessment" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlAssessment" runat="server" DataTextField="FLDNAME" DataValueField="FLDASSESSMENTID" EnableLoadOnDemand="True"
    OnDataBound="ddlAssessment_DataBound" EmptyMessage="Type to select Assessment" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>


