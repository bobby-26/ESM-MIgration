<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlUserType.ascx.cs" Inherits="UserControlUserType" %>
 
 <%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlUserType" runat="server" DataTextField="FLDDESCRIPTION" DataValueField="FLDUSERTYPE" EnableLoadOnDemand="True"
    OnDataBound="ddlUserType_DataBound"  EmptyMessage="Type to select User type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
   