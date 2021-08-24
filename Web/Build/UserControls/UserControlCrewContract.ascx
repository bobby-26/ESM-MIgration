<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCrewContract.ascx.cs" Inherits="UserControlCrewContract" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlContract" runat="server" DataTextField="FLDCONTRACTDETAIL" DataValueField="FLDCONTRACTID" EnableLoadOnDemand="True"
    OnDataBound="ddlContract_DataBound" OnTextChanged="ddlContract_TextChanged" EmptyMessage="Type to select Contract" Filter="Contains" MarkFirstMatch="true">

</telerik:RadComboBox>