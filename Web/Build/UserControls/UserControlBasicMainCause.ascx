<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlBasicMainCause.ascx.cs" 
Inherits="UserControlBasicMainCause" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox ID="ddlBasicMainCause" runat="server" DataTextField="FLDMAINCAUSENAME" DataValueField="FLDMAINCAUSEID" 
    OnTextChanged="ddlBasicMainCause_TextChanged" OnDataBound="ddlBasicMainCause_DataBound" EmptyMessage="Type to select Cause" EnableLoadOnDemand="True" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>