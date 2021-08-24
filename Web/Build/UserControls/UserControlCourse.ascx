<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCourse.ascx.cs" Inherits="UserControlCourse" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlCourse" runat="server" DataTextField="FLDCOURSE" DataValueField="FLDDOCUMENTID" EnableLoadOnDemand="True"
    OnDataBound="ddlCourse_DataBound" OnTextChanged="ddlCourse_TextChanged" EmptyMessage="Type to select Course" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 