<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVesselSignonList.ascx.cs"
    Inherits="UserControlVesselSignonList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlEmployee" runat="server" DataTextField="FLDEMPLOYEENAME" DataValueField="FLDSIGNONOFFID" EnableLoadOnDemand="True"
    OnDataBound="ddlEmployee_DataBound" OnSelectedIndexChanged="ddlEmployee_TextChanged" EmptyMessage="Type to select Vessel Employee" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
