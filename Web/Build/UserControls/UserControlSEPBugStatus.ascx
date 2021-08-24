<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSepBugStatus.ascx.cs" Inherits="UserControlSepBugStatus" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlBugStatus" runat="server" DataTextField="FLDNAME" DataValueField="FLDID" EnableLoadOnDemand="True"
    OnDataBound="ddlBugStatus_DataBound" OnSelectedIndexChanged="ddlBugStatus_TextChanged" EmptyMessage="Type to select SEP Bug Status" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
