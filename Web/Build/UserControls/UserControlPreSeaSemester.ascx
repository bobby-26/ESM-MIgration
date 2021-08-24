<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaSemester.ascx.cs" Inherits="UserControlPreSeaSemester" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucSemester" runat="server" DataTextField="FLDSEMESTERNAME" DataValueField="FLDBATCHSEMID" EnableLoadOnDemand="True"
   OnDataBound="ucSemester_DataBound" OnTextChanged="ucSemester_TextChanged" EmptyMessage="Type to select Semester" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 
