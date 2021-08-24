<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlMoney.ascx.cs"
    Inherits="UserControlMoney" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:TextBox ID="txtMoney" runat="server" MaxLength="19" Width="60px" CssClass="input"></asp:TextBox>
<ajaxToolkit:MaskedEditExtender runat="server" ID="txtMoneyExtender" TargetControlID="txtMoney"
    Mask="9,999,999.99" MessageValidatorTip="true" MaskType="Number" InputDirection="RightToLeft"
    AcceptNegative="Left" DisplayMoney="Left" ErrorTooltipEnabled="True" />
