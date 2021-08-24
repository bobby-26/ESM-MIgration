<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlBankAccount.ascx.cs"
    Inherits="UserControlBankAccount" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox ID="ddlBankAccount" runat="server" DataTextField="FLDBANKACCOUNTNUMBER" DataValueField="FLDSUBACCOUNTID" EnableLoadOnDemand="True" 
    OnDataBound="ddlBankAccount_DataBound" OnTextChanged="ddlBankAccount_TextChanged" EmptyMessage="Type to select BankAccount" Filter="Contains" MarkFirstMatch="true" style="width:240px;">

</telerik:RadComboBox>

