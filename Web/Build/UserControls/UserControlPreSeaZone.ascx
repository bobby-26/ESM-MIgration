<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaZone.ascx.cs"
    Inherits="UserControlPreSeaZone" %>

    <%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlZone" runat="server" DataTextField="FLDPRESEAZONE" DataValueField="FLDPRESEAZONEID" EnableLoadOnDemand="True"
   OnDataBound="ddlZone_DataBound" OnTextChanged="ddlZone_TextChanged" EmptyMessage="Type to select Zone" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>

 
