<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlBriefingTopic.ascx.cs" Inherits="UserControlBriefingTopic" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlBriefingTopic" runat="server" DataTextField="FLDSUBJECT" DataValueField="FLDSUBJECTID" EnableLoadOnDemand="True"
    OnDataBound="ddlBriefingTopic_DataBound" EmptyMessage="Type to select Briefing Topic" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>