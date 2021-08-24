<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlAuditTrail.ascx.cs" Inherits="UserControls_UserControlAudit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<telerik:RadButton RenderMode="Lightweight" ID="cmdAuditTrail" runat="server" Text="Image Button" CssClass="classImage"
    HoveredCssClass="classHoveredImage" PressedCssClass="classPressedImage">
    <Image EnableImageButton="true"  ImageUrl="<%$ PhoenixTheme:images/audit_start.png %>" />
</telerik:RadButton>
