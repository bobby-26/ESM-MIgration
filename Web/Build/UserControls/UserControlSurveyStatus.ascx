<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSurveyStatus.ascx.cs" Inherits="UserControlSurveyStatus" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucSurveyStatus" runat="server" DataTextField="FLDSTATUS" DataValueField="FLDSTATUSID" EnableLoadOnDemand="True"
    OnDataBound="ucSurveyStatus_DataBound" OnTextChanged="ucSurveyStatus_TextChanged" EmptyMessage="Type to select Survey status" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>

