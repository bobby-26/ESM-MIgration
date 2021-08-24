<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaCourse.ascx.cs" Inherits="UserControlPreSeaCourse" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlPreSeaCourse" runat="server" DataTextField="FLDPRESEACOURSENAME" DataValueField="FLDPRESEACOURSEID" EnableLoadOnDemand="True"
    OnDataBound="ddlPreSeaCourse_DataBound" OnTextChanged="ddlPreSeaCourse_TextChanged" EmptyMessage="Type to select Course" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>