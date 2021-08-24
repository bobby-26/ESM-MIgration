<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlQualification.ascx.cs" Inherits="UserControls_UserControlQualification" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlQualification" runat="server" DataTextField="FLDQUALIFICATION" DataValueField="FLDQUALIFICATIONID" EnableLoadOnDemand="True"
    OnDataBound="ddlQualification_DataBound"  EmptyMessage="Type to select Qualification" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>

