<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlFleet.ascx.cs" Inherits="UserControlFleet" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucFleet" runat="server" DataTextField="FLDFLEETDESCRIPTION" DataValueField="FLDFLEETID" EnableLoadOnDemand="True"
     OnDataBound="ucFleet_DataBound" OnTextChanged="ucFleet_TextChanged" EmptyMessage="Type to select Fleet" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 