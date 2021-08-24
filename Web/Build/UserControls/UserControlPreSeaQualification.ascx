<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPreSeaQualification.ascx.cs" Inherits="UserControlPreSeaQualification" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadComboBox DropDownPosition="Static" ID="ddlPreSeaQualification" runat="server" DataTextField="FLDQUALIFICATION" DataValueField="FLDQUALIFICATIONID" EnableLoadOnDemand="True"
    OnDataBound="ddlPreSeaQualification_DataBound" OnTextChanged ="ddlPreSeaQualification_OnTextChanged" EmptyMessage="Type to select Qualification" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 