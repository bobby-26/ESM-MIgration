<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCourseCode.ascx.cs" Inherits="UserControlCourseCode" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlCourse" runat="server" DataTextField="FLDABBREVIATION" DataValueField="FLDDOCUMENTID" EnableLoadOnDemand="True"
    OnDataBound="ddlCourse_DataBound" OnSelectedIndexChanged="ddlCourse_TextChanged" EmptyMessage="Type to select Course Code" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
