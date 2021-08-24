<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlAppraisalOccasion.ascx.cs" Inherits="UserControlAppraisalOccasion" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlOccasion" runat="server" DataTextField="FLDOCCASION" DataValueField="FLDAPPRAISALOCCASIONID" EnableLoadOnDemand="True"
    OnDataBound="ddlOccasion_DataBound" OnSelectedIndexChanged="ddlOccasion_TextChanged" EmptyMessage="Type to select Occasion" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>