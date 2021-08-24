<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlYear.ascx.cs"
    Inherits="UserControlYear" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox ID="ddlyear" runat="server" DataTextField="FLDYEARTEXT" DataValueField="FLDYEARVALUE" EnableLoadOnDemand="True" 
    OnDataBound="ddlyear_DataBound" OnTextChanged="ddlyear_TextChanged" EmptyMessage="Type to select year" Filter="Contains" MarkFirstMatch="true" >
</telerik:RadComboBox>

