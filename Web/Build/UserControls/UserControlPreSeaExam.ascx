<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaExam.ascx.cs" Inherits="UserControlPreSeaExam" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadComboBox DropDownPosition="Static" ID="ucExam" runat="server" DataTextField="FLDEXAMNAME" DataValueField="FLDEXAMID" EnableLoadOnDemand="True"
    OnDataBound="ucExam_DataBound" OnTextChanged="ucExam_TextChanged" EmptyMessage="Type to select Exam" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>