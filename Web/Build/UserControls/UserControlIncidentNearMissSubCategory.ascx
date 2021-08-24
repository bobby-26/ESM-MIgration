<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlIncidentNearMissSubCategory.ascx.cs" Inherits="UserControlIncidentNearMissSubCategory" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlIncidentNearMissSubCategory" runat="server" DataTextField="FLDNAME" DataValueField="FLDINCIDENTNEARMISSSUBCATEGORYID" EnableLoadOnDemand="True"
    OnDataBound="ddlIncidentNearMissSubCategory_DataBound" OnTextChanged="ddlIncidentNearMissSubCategory_TextChanged" EmptyMessage="Type to select Incident Subcategory" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 