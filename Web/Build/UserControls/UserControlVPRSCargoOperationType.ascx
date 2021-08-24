<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlVPRSCargoOperationType.ascx.cs" Inherits="UserControlVPRSCargoOperationType" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlCargoOperationType" runat="server" DataTextField="FLDSHORTNAME" DataValueField="FLDOPERATIONTYPEID" EnableLoadOnDemand="True"
    OnDataBound="ddlCargoOperationType_DataBound" OnSelectedIndexChanged="ddlCargoOperationType_TextChanged" EmptyMessage="Type to select VPRS Cargo Operation Type" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
