<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlLongitude.ascx.cs" Inherits="UserControlLongitude" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<eluc:Error runat="server" Visible="false" ID="ucError" />

<table id="TBLC" cellpadding="0px" cellspacing="0px">
    <tr>
        <td align="right">
            <eluc:MaskNumber ID="txtDegree" Width="40px" runat="server" MaxLength="3" IsInteger="true" IsPositive="true" CssClass="input" />
        </td>
        <td align="right">
            <eluc:MaskNumber ID="txtMinute" runat="server" MaxLength="2" Width="35px" IsInteger="true" IsPositive="true"  CssClass="input" />
        </td>
        <td align="right">
            <eluc:MaskNumber ID="txtSecond" runat="server" MaxLength="2" Width="35px" IsInteger="true" Text="00" IsPositive="true" CssClass="input" />
        </td>
         <td align="right">
            <telerik:RadDropDownList ID="ddlDirection" CssClass="input" Width="45px" runat="server">
                <Items>
                    <telerik:DropDownListItem Text="E" Value="E"></telerik:DropDownListItem>
                    <telerik:DropDownListItem Text="W" Value="W"></telerik:DropDownListItem>
                </Items>
            </telerik:RadDropDownList>
        </td>
    </tr>
    
</table>
