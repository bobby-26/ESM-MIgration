<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCrewCostEvaluationSectionType.ascx.cs" Inherits="UserControlCrewCostEvaluationSectionType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlSectionType" runat="server" DataTextField="FLDSECTIONTYPENAME" DataValueField="FLDSECTIONTYPEID" EnableLoadOnDemand="True"
    OnDataBound="ddlSectionType_DataBound" OnSelectedIndexChanged="ddlSectionType_TextChanged" EmptyMessage="Type to select Cost Evaluation Section Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>