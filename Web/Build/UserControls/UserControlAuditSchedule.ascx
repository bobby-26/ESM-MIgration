<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlAuditSchedule.ascx.cs" Inherits="UserControlAuditSchedule" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox ID="ucAuditSchedule" runat="server" DataTextField="FLDAUDITSCHEDULENAME" EnableLoadOnDemand="True" Filter="Contains" MarkFirstMatch="true"
    DataValueField="FLDREVIEWSCHEDULEID" OnTextChanged="ucAuditSchedule_TextChanged" OnDataBound="ucAuditSchedule_DataBound" EmptyMessage="Type to select Schedule">

</telerik:RadComboBox>
