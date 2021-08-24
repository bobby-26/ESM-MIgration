<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaCourseSemester.ascx.cs" Inherits="UserControls_UserControlPreSeaCourseSemester" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucSemester" runat="server" DataTextField="FLDSEMESTER" DataValueField="FLDSEMESTER" EnableLoadOnDemand="True"
    OnDataBound="ucSemester_DataBound" OnTextChanged="ucSemester_TextChanged" EmptyMessage="Type to select Semester" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>