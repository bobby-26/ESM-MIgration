<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaBlock.ascx.cs" Inherits="UserControlPreSeaBlock" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ucBlock" runat="server" DataTextField="FLDBLOCKNAME" DataValueField="FLDBLOCKID" EnableLoadOnDemand="True"
    OnDataBound="ucBlock_DataBound" OnSelectedIndexChanged="ucBlock_TextChanged" EmptyMessage="Type to select Block" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 