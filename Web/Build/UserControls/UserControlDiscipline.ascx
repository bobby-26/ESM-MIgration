<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlDiscipline.ascx.cs"
    Inherits="UserControlDiscipline" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlDiscipline" runat="server" DataTextField="FLDDISCIPLINENAME" DataValueField="FLDDISCIPLINEID" EnableLoadOnDemand="True"
    OnDataBound="ddlDiscipline_DataBound" OnSelectedIndexChanged="ddlDiscipline_TextChanged" EmptyMessage="Type to select Discipline" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>