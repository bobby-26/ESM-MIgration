<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlFacultyCode.ascx.cs" Inherits="UserControlFacultyCode" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadComboBox DropDownPosition="Static" ID="ddlFacultyCode" runat="server" DataTextField="FLDFACULTYCODE" DataValueField="FLDFACULTYID" EnableLoadOnDemand="True"
    OnDataBound="ddlFacultyCode_DataBound" OnTextChanged="ddlFacultyCode_TextChanged" EmptyMessage="Type to select Faculty Code" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 
