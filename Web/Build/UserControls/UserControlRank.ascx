<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlRank.ascx.cs"
    Inherits="UserControlRank" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlRank" runat="server" DataTextField="FLDRANKNAME" DataValueField="FLDRANKID" EnableLoadOnDemand="True"
    OnDataBound="ddlRank_DataBound" OnTextChanged="ddlRank_TextChanged" EmptyMessage="Type to select Rank" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>
