<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlNumeric.ascx.cs" Inherits="UserControlNumeric" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:TextBox ID="txtNumber" CssClass="input" runat="server" ReadOnly="false"></asp:TextBox>
<ajaxToolkit:MaskedEditExtender  runat="server" ID="txtNumberExtender"
    TargetControlID="txtNumber"    
    Mask="9,999,999.99"  
    MessageValidatorTip="true"    
    MaskType="Number"    
    InputDirection="RightToLeft"    
    AcceptNegative="Left"    
    DisplayMoney="None" 
    ErrorTooltipEnabled="True"/> 