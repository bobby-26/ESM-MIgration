<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWFProcessGroup.ascx.cs" Inherits="UserControlWFProcessGroup" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DROPDOWNPOSITION="Static" ID="ddlProcessGroup" runat="server" DataTextField="FLDNAME" DataValueField="FLDGROUPID" EnableLoadOnDemand="true"
     OnItemDataBound="ddlProcessGroup_ItemDataBound" OnTextChanged="ddlProcessGroup_TextChanged"  EmptyMessage="Select Process Group" Filter="Contains" MarkFirstMatch="true" >

</telerik:RadComboBox>
