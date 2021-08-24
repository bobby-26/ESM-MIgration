<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlInspectionNonConformanceSubCategory.ascx.cs"
    Inherits="UserControlInspectionNonConformanceSubCategory" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlNonConformanceSubCategory" runat="server" DataTextField="FLDNCSUBCATEGORYDESCRIPTION" DataValueField="FLDNCSUBCATEGORYID" EnableLoadOnDemand="True"
    OnDataBound="ddlNonConformanceSubCategory_DataBound" OnSelectedIndexChanged="ddlNonConformanceSubCategory_TextChanged" EmptyMessage="Type to select Non Comformance Sub Category" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
