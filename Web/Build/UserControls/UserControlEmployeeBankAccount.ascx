<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlEmployeeBankAccount.ascx.cs"
    Inherits="UserControlEmployeeBankAccount" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlBankAccount" runat="server" DataTextField="FLDACCOUNTNUMBER" DataValueField="FLDBANKACCOUNTID" EnableLoadOnDemand="True"
    OnDataBound="ddlBankAccount_DataBound" OnSelectedIndexChanged="ddlBankAccount_TextChanged" EmptyMessage="Type to select Employee Bank Account" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>