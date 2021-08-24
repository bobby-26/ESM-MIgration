<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlFaculty.ascx.cs" Inherits="UserControlFaculty" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlFaculty" runat="server" DataTextField="FLDFACULTYNAME" DataValueField="FLDFACULTYID" EnableLoadOnDemand="True"
    OnDataBound="ddlFaculty_DataBound" OnSelectedIndexChanged="ddlFaculty_TextChanged" EmptyMessage="Type to select Faculty" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
