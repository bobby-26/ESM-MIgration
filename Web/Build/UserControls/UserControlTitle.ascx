<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlTitle.ascx.cs"
    Inherits="UserControls_UserControlTitle" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<table>
    <tr valign="top">
        <td>
            <telerik:RadButton ID="chkShowMenu" runat="server" ToggleType="CheckBox" ButtonType="ToggleButton">
                <ToggleStates>
                    <telerik:RadButtonToggleState Text="Hide Menu" ImageUrl="<%$ PhoenixTheme:images/Left.png %>" Height="18" Width="18" />
                    <telerik:RadButtonToggleState Text="Show Menu" ImageUrl="<%$ PhoenixTheme:images/Right.png %>" Height="18" Width="18" />
                </ToggleStates>
            </telerik:RadButton>
        </td>
        <td>
            <asp:Label runat="server" ID="lblTitle" Width="100%" Text=""></asp:Label>            
        </td>
    </tr>
</table>
