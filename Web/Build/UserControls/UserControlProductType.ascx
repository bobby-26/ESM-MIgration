<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlProductType.ascx.cs" Inherits="UserControlProductType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlProductType" runat="server" DataTextField="FLDPRODUCTTYPENAME" DataValueField="FLDPRODUCTTYPEID" EnableLoadOnDemand="True"
    OnDataBound="ddlProductType_DataBound" OnTextChanged="ddlProductType_TextChanged" EmptyMessage="Type to select Product Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
 