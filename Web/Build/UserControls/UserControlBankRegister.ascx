<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlBankRegister.ascx.cs"
    Inherits="UserControlBankRegister" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox ID="ddlBank" runat="server" DataTextField="FLDNAME" DataValueField="FLDBANKID" OnDataBound="ddlBank_DataBound"
 OnTextChanged="ddlBank_TextChanged" EmptyMessage="Type to select Bank" EnableLoadOnDemand="True" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>
