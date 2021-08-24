<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreseaCurrentBatch.ascx.cs" Inherits="UserControlPreseaCurrentBatch" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlBatch" runat="server" DataTextField="FLDBATCH" DataValueField="FLDBATCHID" EnableLoadOnDemand="True"
    OnDataBound="ddlBatch_DataBound"  OnTextChanged="ddlBatch_TextChanged" EmptyMessage="Type to select Batch" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>