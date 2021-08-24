<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlJobParameter.ascx.cs"
    Inherits="UserControlJobParameter" %>
 
 <%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlJobParameter" runat="server" DataTextField="FLDPARAMETERNAME" DataValueField="FLDPARAMETERID" EnableLoadOnDemand="True"
    OnDataBound="ddlJobParameter_DataBound" EmptyMessage="Type to select Parameter" Filter="Contains" MarkFirstMatch="true">   
</telerik:RadComboBox>
   
