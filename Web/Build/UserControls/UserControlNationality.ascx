<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlNationality.ascx.cs"
    Inherits="UserControlNationality" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlNationality" runat="server" DataTextField="FLDNATIONALITY" DataValueField="FLDCOUNTRYCODE" EnableLoadOnDemand="True"
    OnDataBound="ddlNationality_DataBound" EmptyMessage="Type to select Nationality" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
