<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlTrainingMatrixStandard.ascx.cs" 
Inherits="UserControls_UserControlTrainingMatrixStandard" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlTrainingMatrixStandard" runat="server" DataTextField="FLDNAME" DataValueField="FLDADDRESSCODE" EnableLoadOnDemand="True"
   OnTextChanged="ddlTrainingMatrixStandard_TextChanged" OnDataBound="ddlTrainingMatrixStandard_DataBound" EmptyMessage="Type to select Training matrix standard" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
