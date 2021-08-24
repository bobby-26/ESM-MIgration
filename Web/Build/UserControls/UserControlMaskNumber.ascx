<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMaskNumber.ascx.cs"
    Inherits="UserControlMaskNumber" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%--<asp:TextBox ID="txtNumber" CssClass="txtNumber small" OnTextChanged="txtNumber_TextChanged"
    runat="server" MaxLength="19" Width="60px"></asp:TextBox>--%>

  <telerik:RadTextBox RenderMode="Lightweight" runat="server" Width="60px"  ID="txtNumber" OnTextChanged="txtNumber_TextChanged" MaxLength="19">
                    </telerik:RadTextBox>
