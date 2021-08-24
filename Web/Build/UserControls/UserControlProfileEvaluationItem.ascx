<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlProfileEvaluationItem.ascx.cs" Inherits="UserControlProfileEvaluationItem" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlProfileEvaluation" runat="server" DataTextField="FLDPROFILEQUESTION" DataValueField="FLDPROFILEQUESTIONID" EnableLoadOnDemand="True"
   OnDataBound="ddlProfileEvaluation_DataBound" OnTextChanged="ddlProfileEvaluation_TextChanged" EmptyMessage="Type to select Profile Evaluation Item" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
