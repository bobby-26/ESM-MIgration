<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlITCategory.ascx.cs" Inherits="UserControlITCategory" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlCategory" runat="server" DataTextField="FLDNAME" DataValueField="FLDCATEGORYID" EnableLoadOnDemand="True"
    OnDataBound="ddlCategory_DataBound" EmptyMessage="Type to select IT Category" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>