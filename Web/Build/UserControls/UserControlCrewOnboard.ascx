<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCrewOnboard.ascx.cs"
    Inherits="UserControlCrewOnboard" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlOnboard" runat="server" DataTextField="FLDEMPLOYEENAME" DataValueField="FLDEMPLOYEEID" EnableLoadOnDemand="True"
    OnDataBound="ddlOnboard_DataBound" OnSelectedIndexChanged="ddlOnboard_TextChanged" EmptyMessage="Type to select CrewOnboard" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
