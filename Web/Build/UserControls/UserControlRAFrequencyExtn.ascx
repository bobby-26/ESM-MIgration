<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlRAFrequencyExtn.ascx.cs" Inherits="UserControlRAFrequencyExtn" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlFrequency" runat="server" DataTextField="FLDNAME" DataValueField="FLDFREQUENCYID" EnableLoadOnDemand="True"
    OnDataBound="ddlFrequency_DataBound"  EmptyMessage="Type to select Frequency" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
