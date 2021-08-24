<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlAgentBankAccount.ascx.cs" Inherits="UserControlAgentBankAccount" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlAgentBankAccount" runat="server" DataTextField="FLDACCOUNTNUMBER" DataValueField="FLDBANKID" EnableLoadOnDemand="True" OnItemDataBound="ddlAgentBankAccount_ItemDataBound"
    OnDataBound="ddlAgentBankAccount_DataBound" OnSelectedIndexChanged="ddlAgentBankAccount_TextChanged" AddressCode="3982" EmptyMessage="Type to select Agent Bank Account" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
