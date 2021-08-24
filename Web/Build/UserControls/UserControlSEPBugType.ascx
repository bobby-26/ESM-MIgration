<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSepBugType.ascx.cs" Inherits="UserControlSepBugType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlBugType" runat="server" DataTextField="FLDNAME" DataValueField="FLDID" EnableLoadOnDemand="True"
    OnDataBound="ddlBugType_DataBound" OnSelectedIndexChanged="ddlBugType_TextChanged" EmptyMessage="Type to select SEP Bug Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>