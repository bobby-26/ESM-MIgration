<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlContractCrew.ascx.cs" Inherits="UserControlContractCrew" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlCrewComponents" runat="server" DataTextField="FLDCOMPONENTNAME" DataValueField="FLDCOMPONENTID" EnableLoadOnDemand="True"
    OnDataBound="ddlCrewComponents_DataBound" OnSelectedIndexChanged="ddlCrewComponents_TextChanged" EmptyMessage="Type to select Contract Crew" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
