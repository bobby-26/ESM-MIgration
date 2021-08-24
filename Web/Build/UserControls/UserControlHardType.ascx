<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlHardType.ascx.cs"
    Inherits="UserControlHardType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlHardType" runat="server" DataTextField="FLDHARDTYPENAME" DataValueField="FLDHARDTYPECODE" EnableLoadOnDemand="True"
   OnTextChanged="ddlHardType_TextChanged" OnDataBound="ddlHardType_DataBound" EmptyMessage="Type to select Hard Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 
