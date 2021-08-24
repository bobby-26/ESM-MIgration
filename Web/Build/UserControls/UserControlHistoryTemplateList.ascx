<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlHistoryTemplateList.ascx.cs" 
Inherits="UserControlHistoryTemplateList" %>
 
 <%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlHistoryTemplate" runat="server" DataTextField="FLDFORMNAME" DataValueField="FLDFORMID" EnableLoadOnDemand="True" Width="180px"
    OnTextChanged="ddlHistoryTemplate_TextChanged" OnDataBound="ddlHistoryTemplate_DataBound" EmptyMessage="Type to select History Tepmlate" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
