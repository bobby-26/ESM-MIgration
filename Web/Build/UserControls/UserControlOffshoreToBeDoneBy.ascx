<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlOffshoreToBeDoneBy.ascx.cs" Inherits="UserControlOffshoreToBeDoneBy" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlToBeDoneBy" runat="server" DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlToBeDoneBy_DataBound" OnSelectedIndexChanged="ddlToBeDoneBy_TextChanged" EmptyMessage="Type to select Done By" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>