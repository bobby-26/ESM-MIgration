<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCrewCostEvaluationSection.ascx.cs" Inherits="UserControlCrewCostEvaluationSection" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlSection" runat="server" DataTextField="FLDSECTIONENAME" DataValueField="FLDSECTIONID" EnableLoadOnDemand="True"
    OnDataBound="ddlSection_DataBound" OnSelectedIndexChanged="ddlSection_TextChanged" EmptyMessage="Type to select Cost Evaluation Section" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>