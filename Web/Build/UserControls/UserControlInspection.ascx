<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlInspection.ascx.cs"
    Inherits="UserControlInspection" %>
  
 <%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlInspection" runat="server" DataTextField="FLDINSPECTIONNAME" DataValueField="FLDINSPECTIONID" EnableLoadOnDemand="True"
    OnDataBound="ddlInspection_DataBound" OnTextChanged="ddlInspection_TextChanged" EmptyMessage="Type to select Inspection" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 