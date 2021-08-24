<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWFProcessTarget.ascx.cs" Inherits="UserControlWFProcessTarget" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DROPDOWNPOSITION="Static" ID="ddlProcessTarget" runat="server" DataTextField="FLDNAME" DataValueField="FLDTARGETID" EnableLoadOnDemand="true"
     OnItemDataBound="ddlProcessTarget_ItemDataBound" OnTextChanged="ddlProcessTarget_TextChanged"  EmptyMessage="Select Process Target" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>
