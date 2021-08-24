<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlInspectionChecklist.ascx.cs"
    Inherits="UserControlInspectionChecklist" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlInspectionChecklist" runat="server" DataTextField="FLDREFERENCENUMBER" DataValueField="FLDINSPECTIONCHECKLISTID" EnableLoadOnDemand="True"
    OnDataBound="ddlInspectionChecklist_DataBound" OnSelectedIndexChanged="ddlInspectionChecklist_TextChanged" EmptyMessage="Type to select Inspection Checklist" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>