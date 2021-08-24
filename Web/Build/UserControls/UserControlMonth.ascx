<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMonth.ascx.cs"
    Inherits="UserControlMonth" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox ID="ddlMonth" runat="server" DataTextField="FLDMONTHTEXT" DataValueField="FLDMONTHVALUE" EnableLoadOnDemand="True" Width="180px"
    OnDataBound="ddlMonth_DataBound" OnTextChanged="ddlMonth_TextChanged" EmptyMessage="Type to select Month" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>

