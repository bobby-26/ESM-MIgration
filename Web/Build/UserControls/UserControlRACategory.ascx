<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlRACategory.ascx.cs" Inherits="UserControlRACategory" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlCategory" runat="server" DataTextField="FLDNAME" DataValueField="FLDCATEGORYID" EnableLoadOnDemand="True"
    OnDataBound="ddlCategory_DataBound"  OnTextChanged="ddlCategory_TextChanged" EmptyMessage="Type to select Process" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>



