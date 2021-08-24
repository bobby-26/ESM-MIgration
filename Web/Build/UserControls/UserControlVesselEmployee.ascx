<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVesselEmployee.ascx.cs"
    Inherits="UserControlVesselEmployee" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlEmployee" runat="server" DataTextField="FLDEMPLOYEENAME" DataValueField="FLDEMPLOYEEID" EnableLoadOnDemand="True"
    OnDataBound="ddlEmployee_DataBound" OnSelectedIndexChanged="ddlEmployee_TextChanged" EmptyMessage="Type to select Vessel Employee" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
