<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlInstituteBatchList.ascx.cs" Inherits="UserControls_UserControlInstituteBatchList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlBatchList" runat="server" DataTextField="FLDBATCHNO" DataValueField="FLDCREWINSTITUTEBATCHID" EnableLoadOnDemand="True"
    OnDataBound="ddlBatchList_DataBound" OnSelectedIndexChanged="ddlBatchList_TextChanged" EmptyMessage="Type to select Institute Batch List" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>