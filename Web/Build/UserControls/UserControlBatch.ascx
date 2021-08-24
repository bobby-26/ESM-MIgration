<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlBatch.ascx.cs" Inherits="UserControlBatch" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlBatch" runat="server" DataTextField="FLDBATCH" DataValueField="FLDBATCHID" EnableLoadOnDemand="True"
    OnDataBound="ddlBatch_DataBound" OnSelectedIndexChanged="ddlBatch_TextChanged" EmptyMessage="Type to select Batch" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>