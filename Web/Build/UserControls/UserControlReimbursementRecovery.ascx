<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlReimbursementRecovery.ascx.cs"
    Inherits="UserControlReimbursementRecovery" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlReimbursementRecovery" runat="server" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlReimbursementRecovery_DataBound" OnSelectedIndexChanged="ddlReimbursementRecovery_TextChanged" EmptyMessage="Type to select Reimbursement Recovery" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
