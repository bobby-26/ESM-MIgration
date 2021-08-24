<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSignOnReason.ascx.cs" Inherits="UserControlSignOnReason" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlSignOnReason" runat="server" DataTextField="FLDREASON" DataValueField="FLDREASONID" EnableLoadOnDemand="True"
   OnDataBound="ddlSignOnReason_DataBound" OnTextChanged="ddlSignOnReason_TextChanged" EmptyMessage="Type to select Signon Reason" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>   


