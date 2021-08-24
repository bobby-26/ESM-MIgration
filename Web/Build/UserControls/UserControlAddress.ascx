<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlAddress.ascx.cs"
    Inherits="UserControlAddress" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
        <%--<link href="../css/Theme1/Phoenix.Telerik.css" rel="stylesheet" />--%>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="../UserControls/UserControlMobileNumber.ascx" %>
<eluc:Error runat="server" Visible="false" ID="ucError" />
<table align="left" width="100%">
    <tr valign="top">
        <td>
            Name:
        </td>
        <td>
            <telerik:RadTextBox runat="server" ID="txtCode" CssClass="riTextBox riRead input" ReadOnly="true"></telerik:RadTextBox>
            <telerik:RadTextBox runat="server" ID="txtName" CssClass="riTextBox riEnabled input_mandatory" Width="180px"></telerik:RadTextBox>
            <%--<asp:TextBox runat="server" ID="txtCode" CssClass="input" ReadOnly="true"></asp:TextBox>--%>
            <%--<asp:TextBox runat="server" ID="txtName" CssClass="input_mandatory" Width="180px"></asp:TextBox>--%>
        </td>
        <td>
            Status:
        </td>
        <td>
            <eluc:Hard ID="ucStatus" runat="server" CssClass="input" AppendDataBoundItems="true" />
        </td>
    </tr>
    <tr valign="top">
        <td>
            Attention:
        </td>
        <td>
            <telerik:RadTextBox runat="server" ID="txtAttention" CssClass="input"></telerik:RadTextBox>
            <%--<asp:TextBox ID="txtAttention" runat="server" CssClass="input"></asp:TextBox>--%>
        </td>
        <td>
            Phone 1:
        </td>
        <td>
            <eluc:PhoneNumber ID="txtPhone1" runat="server" CssClass="input_mandatory" />
            &nbsp; AOH 1:
            <telerik:RadMaskedTextBox runat="server" ID="txtaohTelephoneno" CssClass="input" Mask="###########" ></telerik:RadMaskedTextBox>
            <%--<asp:TextBox ID="txtaohTelephoneno" runat="server" CssClass="input"></asp:TextBox>
            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtaohTelephoneno"
                OnInvalidCssClass="MaskedEditError" Mask="99999999999" MaskType="Number" InputDirection="LeftToRight"
                AutoComplete="false">
            </ajaxToolkit:MaskedEditExtender>--%>
        </td>
    </tr>
    <tr>
        <td>
            Address 1:
        </td>
        <td>
            <telerik:RadTextBox ID="txtAddress1" runat="server" CssClass="input" Width="90%"></telerik:RadTextBox>
            <%--<asp:TextBox ID="txtAddress1" runat="server" CssClass="input" Width="90%"></asp:TextBox>--%>
        </td>
        <td>
            Phone 2:
        </td>
        <td>
            <eluc:PhoneNumber ID="txtPhone2" runat="server" CssClass="input" />
            &nbsp; AOH 2:
            <telerik:RadMaskedTextBox runat="server" ID="txtaohMobileno" CssClass="input" Mask="###########" ></telerik:RadMaskedTextBox>
            <%--<asp:TextBox ID="txtaohMobileno" runat="server" CssClass="input"></asp:TextBox>
            <ajaxToolkit:MaskedEditExtender ID="maskedittxtaohMobileno" runat="server" TargetControlID="txtaohMobileno"
                OnInvalidCssClass="MaskedEditError" Mask="99999999999" MaskType="Number" InputDirection="LeftToRight"
                AutoComplete="false">
            </ajaxToolkit:MaskedEditExtender>--%>
        </td>
    </tr>
    <tr>
        <td width="15%">
            Address 2:
        </td>
        <td width="35%">
            <%--<asp:TextBox ID="txtAddress2" runat="server" CssClass="input" Width="90%"></asp:TextBox>--%>
            <telerik:RadTextBox ID="txtAddress2" runat="server" CssClass="input" Width="90%"></telerik:RadTextBox>

        </td>
        <td>
            Fax 1:
        </td>
        <td>
            <eluc:PhoneNumber ID="txtFax1" runat="server" CssClass="input" />
            <%--<asp:TextBox ID="txtFax1" runat="server" CssClass="input"></asp:TextBox>--%>
            <%--            <ajaxToolkit:MaskedEditExtender ID="maskedittxtFax1" runat="server" TargetControlID="txtFax1"
                OnInvalidCssClass="MaskedEditError" Mask="99999999999" MaskType="Number" InputDirection="LeftToRight"
                AutoComplete="false">
            </ajaxToolkit:MaskedEditExtender>--%>
        </td>
    </tr>
    <tr>
        <td>
            Address 3:
        </td>
        <td>
            <%--<asp:TextBox ID="txtAddress3" runat="server" CssClass="input" Width="90%"></asp:TextBox>--%>
            <telerik:RadTextBox ID="txtAddress3" runat="server" CssClass="input" Width="90%"></telerik:RadTextBox>

        </td>
        <td>
            Fax 2:
        </td>
        <td>
            <eluc:PhoneNumber ID="txtFax2" runat="server" CssClass="input" />
            <%--<asp:TextBox ID="txtFax2" runat="server" CssClass="input"></asp:TextBox>--%>
            <%--            <ajaxToolkit:MaskedEditExtender ID="maskedittxtFax2" runat="server" TargetControlID="txtFax2"
                OnInvalidCssClass="MaskedEditError" Mask="99999999999" MaskType="Number" InputDirection="LeftToRight"
                AutoComplete="false">
            </ajaxToolkit:MaskedEditExtender>--%>
        </td>
    </tr>
    <tr>
        <td>
            Address 4:
        </td>
        <td>
            <%--<asp:TextBox ID="txtAddress4" runat="server" CssClass="input" Width="90%"></asp:TextBox>--%>
            <telerik:RadTextBox ID="txtAddress4" runat="server" CssClass="input" Width="90%"></telerik:RadTextBox>

        </td>
        <td>
            Email 1:
        </td>
        <td>
            <%--<asp:TextBox ID="txtEmail1" runat="server" CssClass="input_mandatory" Width="80%"></asp:TextBox>--%>
            <telerik:RadTextBox ID="txtEmail1" runat="server" CssClass="input_mandatory" Width="80%"></telerik:RadTextBox>

        </td>
    </tr>
    <tr>
        <td>
            Postal Code/ Country
        </td>
        <td>
            <telerik:RadTextBox ID="txtPostalCode" runat="server" CssClass="input" Width="120px"></telerik:RadTextBox>
            <%--<asp:TextBox ID="txtPostalCode" runat="server" CssClass="input" Width="120px"></asp:TextBox>--%>
            &nbsp;/
            <eluc:Country runat="server" ID="ucCountry" AutoPostBack="true" AppendDataBoundItems="true"
                CssClass="RadComboBox RadComboBox_Metro input" OnTextChangedEvent="ucCountry_TextChanged" />
            <telerik:RadLabel runat="server" ID="lblISDCode"></telerik:RadLabel>
            <%--<asp:Label runat="server" ID="lblISDCode"></asp:Label>--%>
        </td>
        <td>
            Email 2:
        </td>
        <td>
            <telerik:RadTextBox ID="txtEmail2" runat="server" CssClass="input" Width="80%"></telerik:RadTextBox>
            <%--<asp:TextBox ID="txtEmail2" runat="server" CssClass="input" Width="80%"></asp:TextBox>--%>
        </td>
    </tr>
    <tr>
        <td>
            State/ City:
        </td>
        <td>
            <eluc:State ID="ucState" CssClass="RadComboBox RadComboBox_Metro input" runat="server" AppendDataBoundItems="true"
                AutoPostBack="true" OnTextChangedEvent="ddlState_TextChanged" />
            &nbsp;/
            <eluc:City ID="ddlCity" runat="server" AppendDataBoundItems="true" CssClass="RadComboBox RadComboBox_Metro input" />
        </td>
        <td>
            Web Site:
        </td>
        <td>
            <%--<asp:TextBox ID="txtURL" runat="server" CssClass="input" Width="80%"></asp:TextBox>--%>
            <telerik:RadTextBox ID="txtURL" runat="server" CssClass="input" Width="80%"></telerik:RadTextBox>

        </td>
    </tr>
    <tr>
        <td colspan="4">
            <hr />
        </td>
    </tr>
    <tr>
        <td>
            QA Grading
        </td>
        <td>
            <eluc:Quick ID="ucQAGrading" runat="server" AppendDataBoundItems="true" CssClass="input"
                Width="240px" />
        </td>
        <td>
            In-Charge:
        </td>
        <td>
            <%--<asp:TextBox ID="txtInCharge" runat="server" CssClass="input"></asp:TextBox>--%>
            <telerik:RadTextBox ID="txtInCharge" runat="server" CssClass="input"></telerik:RadTextBox>

        </td>
    </tr>
</table>
