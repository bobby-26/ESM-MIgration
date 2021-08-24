<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCompany.aspx.cs"
    Inherits="RegistersCompany" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Airport" Src="~/UserControls/UserControlAirport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="../UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlMappedDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserName" Src="~/UserControls/UserControlUserName.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Company</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuCompanyList" runat="server" OnTabStripCommand="CompanyList_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            </div>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td width="11.23%">
                        <telerik:RadLabel ID="lblCompanyPrefix" runat="server" Text="Company Prefix"></telerik:RadLabel>
                    </td>
                    <td width="63%">
                        <telerik:RadTextBox runat="server" ID="txtCompanyPrefix" MaxLength="2" Width="180px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompanyName" runat="server" Text="Company Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCompanyName" MaxLength="50" Width="360px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShortCode" runat="server" Text="Short Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtShortCode" CssClass="input_mandatory" MaxLength="8" Width="180px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompanyRegNo" runat="server" Text="Company Reg. No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRegNo" MaxLength="25" Width="180px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:Address runat="server" ID="ucAddress" />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td width="11.23%">
                        <telerik:RadLabel ID="lblTelephoneNumber" runat="server" Text="Telephone Number"></telerik:RadLabel>
                    </td>
                    <td width="63%">
                        <telerik:RadTextBox runat="server" ID="txtTelephoneNo" CssClass="input" MaxLength="20" Width="180px" Style="text-align: right;"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFaxNumber" runat="server" Text="Fax Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFaxNo" CssClass="input" MaxLength="20" Width="180px" Style="text-align: right;"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEMail" runat="server" Text="E-Mail"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmail" CssClass="input" MaxLength="200"
                            Width="360px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofIncorporation" runat="server" Text="Place of Incorporation"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPlaceOfIncorp" MaxLength="50" Width="360px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBaseCurrency" runat="server" Text="Base Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ucBaseCurrency" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReportingCurrency" runat="server" Text="Reporting Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ucReportingCurrency" runat="server" CssClass="input_mandatory"
                            AppendDataBoundItems="true" />
                    </td>
                </tr>
                 <tr>
                <td>
                    <telerik:RadLabel ID="lbldefaultcurrency" runat="server" Text="Default Currency"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Currency ID="ucdefaultcurrecy" runat="server" 
                        AppendDataBoundItems="true" />

                </td>
            </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRemarks" TextMode="MultiLine" Width="360px" Height="75px"
                            CssClass="input">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblgstno" runat="server" Text="Company GST No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtcompanugstno" Width="180px"
                            CssClass="input">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActive" runat="server" Text="Active"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="chkActive" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoicePostingBy" runat="server" Text="Invoice Posting By"></telerik:RadLabel>
                        <br />
                        <telerik:RadLabel ID="lblSupplierConfigurationOnly" runat="server" Text="Supplier Configuration Only"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="chkUseSupplierConfigYN" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAllowVesselEntries" runat="server" Text="Allow Vessel Entries"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="chkAllowVesselEntries" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompanyPIC" runat="server" Text="Company PIC"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserName runat="server" ID="ucCompanyPIC" CssClass="input" AppendDataBoundItems="true" Width="360px"></eluc:UserName>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInterCompanyAccount" runat="server" Text="Inter Company Account Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlInterCompanyAccount" runat="server" Filter="Contains" EmptyMessage="Type to select account Code" CssClass="dropdown_mandatory"
                            AutoPostBack="false" Width="360px">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInterCompanySupplier" runat="server" Text="Inter Company Supplier Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlInterCompanySupplier" runat="server" Filter="Contains" AutoPostBack="false"
                            EmptyMessage="Type to select supplier Code" CssClass="dropdown_mandatory" Width="360px">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
