<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlUserGroup.ascx.cs"
    Inherits="UserControlUserGroup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlUserGroup" runat="server" DataTextField="FLDGROUPNAME" DataValueField="FLDGROUPCODE" EnableLoadOnDemand="True"
    OnTextChanged="ddlUserGroup_TextChanged" OnDataBound="ddlUserGroup_DataBound" EmptyMessage="Type to select User group" Filter="Contains" MarkFirstMatch="true" >
</telerik:RadComboBox> 
