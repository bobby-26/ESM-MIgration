<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlBasicSubCause.ascx.cs"
 Inherits="UserControlBasicSubCause" %>
 
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox ID="ddlBasicSubCause" runat="server" DataTextField="FLDSUBCAUSENAME" DataValueField="FLDSUBCAUSEID" 
    OnTextChanged="ddlBasicSubCause_TextChanged" OnDataBound="ddlBasicSubCause_DataBound" EmptyMessage="Type to select Cause" EnableLoadOnDemand="True" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>