<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaSemWeeks.ascx.cs" Inherits="UserControlPreSeaSemWeeks" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlWeek" runat="server" DataTextField="FLDWEEKPERIOD" DataValueField="FLDWEEKID" EnableLoadOnDemand="True"
    OnDataBound="ddlWeek_DataBound" OnTextChanged="ddlWeek_TextChanged" EmptyMessage="Type to select SemWeeks" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 
