<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlCommonAddress.ascx.cs"
    Inherits="UserControlCommonAddress" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCountry" Src="UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<table cellpadding="1" cellspacing="1" width="100%">
    <tr>
        <td>
            <telerik:RadLabel ID="RadLbl1" runat="server" Text="Address1"></telerik:RadLabel>

        </td>
        <td colspan="3">
            <telerik:RadTextBox ID="txtAddressLine1" runat="server" Width="360px" CssClass="input_mandatory"
                MaxLength="200">
            </telerik:RadTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Address2"></telerik:RadLabel>
        </td>
        <td colspan="3">
            <telerik:RadTextBox ID="txtAddressLine2" runat="server" Width="360px" MaxLength="200"></telerik:RadTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <telerik:RadLabel ID="RadLabel3" runat="server" Text="Address3"></telerik:RadLabel>
        </td>
        <td colspan="3">
            <telerik:RadTextBox ID="txtAddressLine3" runat="server" Width="360px" MaxLength="200"></telerik:RadTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <telerik:RadLabel ID="RadLabel4" runat="server" Text="Address4"></telerik:RadLabel>
        </td>
        <td colspan="3">
            <telerik:RadTextBox ID="txtAddressLine4" runat="server" Width="360px" MaxLength="200"></telerik:RadTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <telerik:RadLabel ID="RadLabel5" runat="server" Text="Country"></telerik:RadLabel>
        </td>
        <td>
            <eluc:UserControlCountry ID="ddlCountry" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                CssClass="input_mandatory" OnTextChangedEvent="ddlCountry_TextChanged" />
        </td>
    </tr>
    <tr>
        <td>
            <telerik:RadLabel ID="RadLabel6" runat="server" Text="State"></telerik:RadLabel>
        </td>
        <td>
            <eluc:State ID="ddlState" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ddlState_TextChanged" />
        </td>
    </tr>
    <tr>
        <td>
            <telerik:RadLabel ID="RadLabel7" runat="server" Text="City"></telerik:RadLabel>
        </td>
        <td>
            <eluc:City ID="ddlCity" runat="server" AppendDataBoundItems="true" />
        </td>
    </tr>
    <tr>
        <td>
            <telerik:RadLabel ID="RadLabel8" runat="server" Text="Postal Code"></telerik:RadLabel>
        </td>
        <td colspan="3">
            <telerik:RadTextBox ID="txtPinCode" runat="server" MaxLength="10" Style="text-align: right;"></telerik:RadTextBox>
        </td>
    </tr>
</table>
