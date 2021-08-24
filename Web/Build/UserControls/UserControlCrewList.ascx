<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCrewList.ascx.cs"
    Inherits="UserControlCrewList" %>

<telerik:RadComboBox ID="ddlCrewList" runat="server" DataTextField="FLDEMPLOYEENAME" DataValueField="FLDSIGNONOFFID" EnableLoadOnDemand="True" AutoPostBack="true"
    OnDataBound="ddlCrewList_DataBound" OnSelectedIndexChanged="ddlCrewList_TextChanged" OnItemsRequested="ddlCrewList_ItemsRequested"
    EmptyMessage="Type to select crew" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
