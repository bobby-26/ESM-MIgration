<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlConductEvaluationItem.ascx.cs" Inherits="UserControlConductEvaluationItem" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlConductEvaluation" runat="server" DataTextField="FLDCONDUCTQUESTION" DataValueField="FLDCONDUCTQUESTIONID" EnableLoadOnDemand="True"
    OnDataBound="ddlConductEvaluation_DataBound" OnSelectedIndexChanged="ddlConductEvaluation_TextChanged" EmptyMessage="Type to select Conduct Evaluation" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>