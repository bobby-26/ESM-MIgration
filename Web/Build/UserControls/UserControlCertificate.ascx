<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCertificate.ascx.cs"
    Inherits="UserControlCertificate" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlCertificate" runat="server" DataTextField="FLDCERTIFICATENAME" DataValueField="FLDCERTIFICATEID" EnableLoadOnDemand="True"
    OnDataBound="ddlCertificate_DataBound" OnSelectedIndexChanged="ddlCertificate_TextChanged" EmptyMessage="Type to select Certificate" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
