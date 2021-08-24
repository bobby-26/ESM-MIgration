<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSeaport.ascx.cs"
    Inherits="UserControlSeaport" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
    

<telerik:RadComboBox ID="ucSeaport" runat="server" DataTextField="FLDSEAPORTNAME" DataValueField="FLDSEAPORTID" EnableLoadOnDemand="True"
OnDataBound="ucSeaport_DataBound" OnTextChanged="ucSeaport_TextChanged" EmptyMessage="Type to select Seaport" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>