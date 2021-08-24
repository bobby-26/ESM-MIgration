<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaExamVenue.ascx.cs" Inherits="UserControlPreSeaExamVenue" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlPreSeaExamVenue" runat="server" DataTextField="FLDEXAMVENUENAME" DataValueField="FLDEXAMVENUEID"  EnableLoadOnDemand="True"
    OnDataBound="ddlPreSeaExamVenue_DataBound" OnTextChanged="ddlPreSeaExamVenue_TextChanged" EmptyMessage="Type to select Exam Venu" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>