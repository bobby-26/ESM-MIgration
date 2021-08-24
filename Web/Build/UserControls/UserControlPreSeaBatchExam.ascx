<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaBatchExam.ascx.cs" Inherits="UserControlPreSeaBatchExam" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlBatchExam" runat="server" DataTextField="FLDEXAMNAME" DataValueField="FLDEXAMSCHEDULEID" EnableLoadOnDemand="True"
    OnDataBound="ddlBatchExam_DataBound" OnSelectedIndexChanged="ddlBatchExam_TextChanged" EmptyMessage="Type to select PreSea Batch Exam" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
