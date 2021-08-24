<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlHistoryTemplate.ascx.cs"
    Inherits="UserControlHistoryTemplate" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlHistoryTemplate" runat="server" DataTextField="FLDTEMPLATENAME" DataValueField="FLDTEMPLATEID" EnableLoadOnDemand="True"
   OnTextChanged="ddlHistoryTemplate_TextChanged" OnDataBound="ddlHistoryTemplate_DataBound" EmptyMessage="Type to select Template" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 
