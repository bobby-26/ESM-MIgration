<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlInspectionTopic.ascx.cs"
    Inherits="UserControlInspectionTopic" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlInspectionTopic" runat="server" DataTextField="FLDTOPICNAME" DataValueField="FLDTOPICID" EnableLoadOnDemand="True"
    OnDataBound="ddlInspectionTopic_DataBound" OnSelectedIndexChanged="ddlInspectionTopic_TextChanged" EmptyMessage="Type to select Inspection Topic" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>