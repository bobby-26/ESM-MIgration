<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlInspectionSchedule.ascx.cs" Inherits="UserControlInspectionSchedule" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucInspectionSchedule" runat="server" DataTextField="FLDINSPECTIONSCHEDULENAME" DataValueField="FLDINSPECTIONSCHEDULEID" EnableLoadOnDemand="True"
    OnDataBound="ucInspectionSchedule_DataBound" OnSelectedIndexChanged="ucInspectionSchedule_TextChanged" EmptyMessage="Type to select Inspection Schedule" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>