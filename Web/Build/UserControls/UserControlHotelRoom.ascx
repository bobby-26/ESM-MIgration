<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlHotelRoom.ascx.cs" Inherits="UserControlHotelRoom" %> 

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlHotelRoom" runat="server" DataTextField="FLDROOMTYPENAME" DataValueField="FLDROOMTYPEID" EnableLoadOnDemand="True"
    OnDataBound="ddlHotelRoom_DataBound" OnTextChanged="ddlHotelRoom_TextChanged" EmptyMessage="Type to select Hotel room" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
