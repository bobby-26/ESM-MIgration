<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCity.ascx.cs" Inherits="UserControlCity" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlCity" runat="server" DataTextField="FLDCITYNAME" DataValueField="FLDCITYID" EnableLoadOnDemand="True"
    OnDataBound="ddlCity_DataBound" OnTextChanged="ddlCity_TextChanged" EmptyMessage="Type to select City" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>