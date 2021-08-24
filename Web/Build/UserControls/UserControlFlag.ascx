<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlFlag.ascx.cs" Inherits="UserControlFlag" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlFlag" runat="server" DataTextField="FLDFLAGNAME" DataValueField="FLDCOUNTRYCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlFlag_DataBound"  OnTextChanged="ddlFlag_TextChanged" EmptyMessage="Type to select Flag" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 
