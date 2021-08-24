<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlWFTarget.ascx.cs" Inherits="UserControlWFTarget" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadComboBox DropDownPosition="Static" ID="ddlTarget" runat="server" DataTextField="FLDNAME" DataValueField="FLDTARGETID" EnableLoadOnDemand="True"
   OnDataBound="ddlTarget_DataBound" EmptyMessage="Select Target" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>