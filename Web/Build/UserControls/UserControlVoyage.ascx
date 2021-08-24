<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVoyage.ascx.cs"
    Inherits="UserControlVoyage" %>
    

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlVoyage" runat="server" DataTextField="FLDVOYAGENO" DataValueField="FLDVOYAGEID" EnableLoadOnDemand="True"
     OnDataBound="ddlVoyage_DataBound" OnTextChanged="ddlVoyage_TextChanged" EmptyMessage="Type to select Voyage" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
