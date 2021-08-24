<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSeaCondtion.ascx.cs" Inherits="UserControlSeaCondtion" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlSeaCondition" runat="server" DataTextField="FLDSEACONDITIONNAME" DataValueField="FLDSEACONDITIONCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlSeaCondition_DataBound" OnSelectedIndexChanged="ddlSeaCondition_TextChanged" EmptyMessage="Type to select Sea Condition" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>