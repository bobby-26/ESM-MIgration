<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlContractCBA.ascx.cs"
    Inherits="UserControlContractCBA" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlContract" runat="server" DataTextField="FLDCOMPONENTNAME" DataValueField="FLDCOMPONENTID" EnableLoadOnDemand="True"
    OnDataBound="ddlContract_DataBound" OnSelectedIndexChanged="ddlContract_TextChanged" EmptyMessage="Type to select Contract CBA" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
