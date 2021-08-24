<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlEmployee.ascx.cs"
    Inherits="UserControlEmployee" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlEmployee" runat="server" DataTextField="FLDEMPLOYEENAME" DataValueField="FLDEmployeeID" EnableLoadOnDemand="True"
    OnDataBound="ddlEmployee_DataBound" OnSelectedIndexChanged="ddlEmployee_TextChanged" EmptyMessage="Type to select Employee" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>