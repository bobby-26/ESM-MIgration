<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWFGroup.ascx.cs" Inherits="UserControlWFGroup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlGroup" runat="server" DataTextField="FLDNAME" DataValueField="FLDGROUPID" EnableLoadOnDemand="True" 
     OnDataBound="ddlGroup_DataBound"    EmptyMessage="Select Group" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>
