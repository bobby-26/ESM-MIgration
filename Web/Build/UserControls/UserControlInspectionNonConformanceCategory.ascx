<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlInspectionNonConformanceCategory.ascx.cs"
    Inherits="UserControlInspectionNonConformanceCategory" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlNonConformanceCategory" runat="server" DataTextField="FLDNCCATEGORYDESCRIPTION" DataValueField="FLDNCCATEGORYID" EnableLoadOnDemand="True"
    OnDataBound="ddlNonConformanceCategory_DataBound" OnSelectedIndexChanged="ddlNonConformanceCategory_TextChanged" EmptyMessage="Type to select Non Conformance Category" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>