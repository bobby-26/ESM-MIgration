<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlRAHazardType.ascx.cs" Inherits="UserControlRAHazardType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlHazardType" runat="server" DataTextField="FLDNAME" DataValueField="FLDHAZARDID" EnableLoadOnDemand="True"
    OnDataBound="ddlHazardType_DataBound"  OnTextChanged="ddlHazardType_TextChanged" EmptyMessage="Type to select Hazard Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>



