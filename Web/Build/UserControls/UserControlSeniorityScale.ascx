<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSeniorityScale.ascx.cs"
    Inherits="UserControlSeniorityScale" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlSeniorityScale" runat="server" DataTextField="FLDSCALEDESC" DataValueField="FLDSENIORITYSCALEID" EnableLoadOnDemand="True"
    OnDataBound="ddlSeniorityScale_DataBound" OnSelectedIndexChanged="ddlSeniorityScale_TextChanged" EmptyMessage="Type to select Seniority Scale" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
