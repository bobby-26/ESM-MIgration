<%@ Control Language="C#" AutoEventWireup="false" CodeFile="UserControlTitleTelerik.ascx.cs"
    Inherits="UserControlTitleTelerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<table>
    <tr valign="top">
        <td>            
            <telerik:RadButton RenderMode="Lightweight" ID="chkShowMenu" runat="server" ButtonType="ToggleButton"
                ToggleType="CheckBox" AutoPostBack="false" Height="18px" Width="18px" OnClientClicked="telerik.OnClientClicked" CssClass="titleImg">
                <ToggleStates>
                    <telerik:RadButtonToggleState ImageUrl="<%$ PhoenixTheme:images/Right.png %>"></telerik:RadButtonToggleState>
                    <telerik:RadButtonToggleState ImageUrl="<%$ PhoenixTheme:images/Left.png %>"></telerik:RadButtonToggleState>
                </ToggleStates>
            </telerik:RadButton>
        </td>
        <td>
            <asp:Label runat="server" ID="lblTitle" Width="100%" Text=""></asp:Label>            
        </td>
    </tr>
</table>
<style>
    .titleImg {
        background-repeat:no-repeat; background-position:center;
    }
</style>


