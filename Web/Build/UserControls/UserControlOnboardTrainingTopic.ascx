<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlOnboardTrainingTopic.ascx.cs" Inherits="UserControlOnboardTrainingTopic" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlOnboardTrainingTopic" runat="server" DataTextField="FLDSUBJECT" DataValueField="FLDSUBJECTID" EnableLoadOnDemand="True"
    OnDataBound="ddlOnboardTrainingTopic_DataBound" EmptyMessage="Type to select Onboard Training Topic" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>