<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMaskedTextBox.ascx.cs" Inherits="UserControlMaskedTextBox" %>

  <telerik:RadMaskedTextBox RenderMode="Lightweight" runat="server" Width="60px"  ID="txtNumber" OnTextChanged="txtNumber_TextChanged" Mask="##########">
                    </telerik:RadMaskedTextBox>