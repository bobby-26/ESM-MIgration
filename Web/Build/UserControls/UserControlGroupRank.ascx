<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlGroupRank.ascx.cs"
    Inherits="UserControlGroupRank" %>

    <%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlRank" runat="server" DataTextField="FLDGROUPRANK" DataValueField="FLDGROUPRANKID" EnableLoadOnDemand="True"
    OnDataBound="ddlRank_DataBound"  OnTextChanged="ddlRank_TextChanged" EmptyMessage="Type to select Rank" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 
