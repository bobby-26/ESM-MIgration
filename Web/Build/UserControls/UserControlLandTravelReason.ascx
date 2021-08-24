<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlLandTravelReason.ascx.cs" Inherits="UserControlLandTravelReason" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddllandtravelreason" runat="server" DataTextField="FLDREASON" DataValueField="FLDLANDTRAVELREASONID" EnableLoadOnDemand="True"
    OnDataBound="ddllandtravelreason_DataBound" OnSelectedIndexChanged="ddlLandTravelReason_TextChanged" EmptyMessage="Type to select Land Travel Reason" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>