<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlHard.ascx.cs"
    Inherits="UserControlHard" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlHard" runat="server" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" EnableLoadOnDemand="True"
     OnDataBound="ddlHard_DataBound" OnTextChanged="ddlHard_TextChanged" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
