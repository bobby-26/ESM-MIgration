<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlRAMiscellaneous.ascx.cs" Inherits="UserControlRAMiscellaneous" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlMiscellaneous" runat="server" DataTextField="FLDNAME" DataValueField="FLDMISCELLANEOUSID" EnableLoadOnDemand="True"
   OnDataBound="ddlMiscellaneous_DataBound"   EmptyMessage="Type to select Miscellaneous" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>




