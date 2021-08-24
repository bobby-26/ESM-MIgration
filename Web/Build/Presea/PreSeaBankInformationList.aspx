<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBankInformationList.aspx.cs"
    Inherits="PreSeaBankInformationList" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagName="Currency" TagPrefix="eluc" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bank Information</title>
   <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
   </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBankInformation" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManagerbank"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <%-- <asp:UpdatePanel runat="server" ID="pnlBankInformation">
        <ContentTemplate>--%>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader" style="position: relative">
            <div id="div1" style="vertical-align: top">
                <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Address" Width="360px"></asp:Label>
            </div>
        </div>
        <div class="navSelectHeader" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuBankInformation" runat="server" OnTabStripCommand="AddressMain_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div class="navSelectHeader" style="top: 28px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuBankMain" runat="server" OnTabStripCommand="BankMain_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuRegistersBankInformation" runat="server" OnTabStripCommand="RegistersBankInformation_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div id="divGrid" style="position: relative; z-index: 0">
            <asp:GridView ID="gvBankInformation" runat="server" AutoGenerateColumns="False" CellPadding="3"
                Font-Size="11px" OnRowCommand="gvBankInformation_RowCommand" OnRowDataBound="gvBankInformation_RowDataBound"
                OnRowDeleting="gvBankInformation_RowDeleting" OnRowEditing="gvBankInformation_RowEditing"
                OnRowCancelingEdit="gvBankInformation_RowCancelingEdit" ShowHeader="true" Width="100%"
                ShowFooter="true" EnableViewState="false">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderTemplate>
                            Bank Information
                        </HeaderTemplate>
                        <ItemTemplate>
                            <font size="2px"><b><u>Bank</u></b></font>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td width="10%">
                                        <b>Bank Name:</b>
                                    </td>
                                    <td width="19%">
                                        <asp:Label ID="lblBankid" Visible="false" Text='<%#Bind("FLDBANKID") %>' runat="server"></asp:Label>
                                        <asp:Label ID="lblBankname" Text='<%#Bind("FLDBANKNAME") %>' runat="server"></asp:Label>
                                    </td>
                                    <td width="10%">
                                        <b>ACH Bank Code:</b>
                                    </td>
                                    <td width="20%">
                                        <asp:Label ID="lblBcode" runat="server" Text='<%#Bind("FLDBANKCODE") %>'></asp:Label>
                                    </td>
                                    <td width="10%">
                                        <b>ACH Branch Code:</b>
                                    </td>
                                    <td width="21%">
                                        <asp:Label ID="lblBrcode" runat="server" Text='<%#Bind("FLDBRANCHCODE") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <asp:Label ID="lblswiftcodecaption" runat="server" Text='<%#Bind("FLDBANKSWIFTCODECAPTION") %>'></asp:Label>:</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSwiftcode" runat="server" Text='<%#Bind("FLDSWIFTCODE") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b>IBAN Number:</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblIBANNumber" runat="server" Text='<%#Bind("FLDIBANNUMBER") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b>Account Number:</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAccountNumber" runat="server" Text='<%#Bind("FLDACCOUNTNUMBER") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Currency:</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCurrency" runat="server" Text='<%#Bind("FLDCURRENCYNAME") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b>Beneficiary Name: </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBeneficiaryName" runat="server" Text='<%#Bind("FLDBENEFICIARYNAME") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b>Contact Person Name: </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblContactName" runat="server" Text='<%#Bind("FLDCONTACTNAME") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>E-Mail: </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEmailid" runat="server" Text='<%#Bind("FLDEMAILID") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b>Phone Number: </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPhoneNumber" runat="server" Text='<%#Bind("FLDPHONENUMBER") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b>Bank Country: </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBankCountry" runat="server" Text='<%#Bind("FLDBANKCOUNTRYCODE") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Additional Bank Information: </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAdditionalbankinformation" runat="server" Text='<%#Bind("FLDADDITIONALBANKINFO") %>'></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <font size="2px"><b><u>Intermediate Bank</u></b></font>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Bank Name:</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbliBankid" Visible="false" Text='<%#Bind("FLDIBANKID") %>' runat="server"></asp:Label>
                                        <asp:Label ID="txtibankname" runat="server" Text='<%#Bind("FLDIBANKNAME") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b>ACH Bank Code:</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblibankcode" runat="server" Text='<%#Bind("FLDIBANKCODE") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b>ACH Branch Code:</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblibranchcode" runat="server" Text='<%#Bind("FLDIBRANCHCODE") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <asp:Label ID="lbliswiftcodecaption" runat="server" Text='<%#Bind("FLDIBANKSWIFTCODECAPTION") %>'></asp:Label>:</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbliswiftcode" runat="server" Text='<%#Bind("FLDISWIFTCODE") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b>IBAN Number:</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbliinanumber" runat="server" Text='<%#Bind("FLDIIBANNUMBER") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b>Account Number:</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbliAccountNumber" runat="server" Text='<%#Bind("FLDIACCOUNTNUMBER") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Currency:</b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbliCurrency" runat="server" Text='<%#Bind("FLDICURRENCYNAME") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b>Bank Country: </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblibankcountry" runat="server" Text='<%#Bind("FLDIBANKCOUNTRYCODE") %>'></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <font size="2px"><b><u>Bank</u></b></font>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td>
                                        Bank Name:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBankidEdit" Visible="false" Text='<%#Bind("FLDBANKID") %>' runat="server"></asp:Label>
                                        <asp:TextBox ID="txtBankNameEdit" runat="server" Text='<%#Bind("FLDBANKNAME") %>'
                                            CssClass="input_mandatory" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        ACH Bank Code:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBankCodeEdit" runat="server" Text='<%#Bind("FLDBANKCODE") %>'
                                            CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        ACH Branch Code:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBranchCodeEdit" runat="server" Text='<%#Bind("FLDBRANCHCODE") %>'
                                            CssClass="input_mandatory" MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <eluc:Hard ID="ddlSwiftcodetypeedit" runat="server" HardList="<%# PhoenixRegistersHard.ListHard(1, 198) %>"
                                                CssClass="input" HardTypeCode="198" Width="300px" />
                                            <asp:Label ID="lblswiftcaptioncode" Visible="false" Text='<%#Bind("FLDISWIFTCODECAPTIONCODE") %>'
                                                runat="server"></asp:Label>
                                        </b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSwiftCodeEdit" runat="server" Text='<%#Bind("FLDSWIFTCODE") %>'
                                            CssClass="input_mandatory" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        IBAN Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIBANNumberEdit" runat="server" Text='<%#Bind("FLDIBANNUMBER") %>'
                                            CssClass="input_mandatory" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        Account Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAccountNumberEdit" runat="server" Text='<%#Bind("FLDACCOUNTNUMBER") %>'
                                            CssClass="input_mandatory" MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Currency:
                                    </td>
                                    <td>
                                        <eluc:Currency runat="server" ID="ucCurrencyBankEdit" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                            AppendDataBoundItems="true" AutoPostBack="false" CssClass="dropdown_mandatory" />
                                        <asp:Label ID="lblCurrencyId" Visible="false" Text='<%#Bind("FLDCURRENCYID") %>'
                                            runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        Beneficiary Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBeneficiaryEdit" runat="server" Text='<%#Bind("FLDBENEFICIARYNAME") %>'
                                            CssClass="input_mandatory" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        Contact Name:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtContactNameEdit" CssClass="input" Text='<%#Bind("FLDCONTACTNAME") %>'
                                            MaxLength="100"></asp:TextBox>
                                    </td>
                                    <tr>
                                        <td>
                                            EMail:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtEmailidEdit" MaxLength="50" Text='<%#Bind("FLDEMAILID") %>'
                                                CssClass="input" Width="250"></asp:TextBox>
                                        </td>
                                        <td>
                                            Phone Number:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPhoneNumberEdit" CssClass="input" Text='<%#Bind("FLDPHONENUMBER") %>'
                                                MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td>
                                            Bank Country:
                                        </td>
                                        <td>
                                            <eluc:Country runat="server" ID="ucBankCountryEdit" CssClass="input" AppendDataBoundItems="true"
                                                CountryList="<%# PhoenixRegistersCountry.ListCountry(null)%>" />
                                            <asp:Label ID="lblcountryid" Visible="false" Text='<%#Bind("FLDCOUNTRYCODE") %>'
                                                runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Additional Bank Information:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtAdditionalbankinfoEdit" MaxLength="30" CssClass="input"
                                                Width="250"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <font size="2px"><b><u>Intermediate Bank</u></b></font>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Name:
                                    </td>
                                    <td>
                                        <asp:Label ID="lbliBankidEdit" Visible="false" Text='<%#Bind("FLDIBANKID") %>' runat="server"></asp:Label>
                                        <asp:TextBox ID="txtIBankNameEdit" runat="server" Text='<%#Bind("FLDIBANKNAME") %>'
                                            CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        ACH Bank Code:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIBankCodeEdit" runat="server" Text='<%#Bind("FLDIBANKCODE") %>'
                                            CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        ACH Branch Code:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIBranchCodeEdit" runat="server" Text='<%#Bind("FLDIBRANCHCODE") %>'
                                            CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <eluc:Hard ID="ddlSwiftcodetypeinteredit" runat="server" HardList="<%# PhoenixRegistersHard.ListHard(1, 198) %>"
                                                CssClass="input" HardTypeCode="198" Width="300px" />
                                            <asp:Label ID="lbliswiftcaptioncode" Visible="false" Text='<%#Bind("FLDISWIFTCODECAPTIONCODE") %>'
                                                runat="server"></asp:Label>
                                        </b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtISwiftCodeEdit" runat="server" Text='<%#Bind("FLDISWIFTCODE") %>'
                                            CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        IBAN Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIBANNumberEdit" runat="server" Text='<%#Bind("FLDIIBANNUMBER") %>'
                                            CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        Account Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIAccountNumberEdit" runat="server" Text='<%#Bind("FLDIACCOUNTNUMBER") %>'
                                            CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Currency:
                                    </td>
                                    <td>
                                        <eluc:Currency runat="server" ID="ucCurrencyIBankEdit" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                            AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                                        <asp:Label ID="lblICurrencyId" Visible="false" Text='<%#Bind("FLDICURRENCYID") %>'
                                            runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        Bank Country:
                                    </td>
                                    <td>
                                        <eluc:Country runat="server" ID="ucIntermediateBankCountryEdit" CssClass="input"
                                            AppendDataBoundItems="true" CountryList="<%# PhoenixRegistersCountry.ListCountry(null)%>" />
                                        <asp:Label ID="lblIBankCountryid" Visible="false" Text='<%#Bind("FLDICOUNTRYCODE") %>'
                                            runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <font size="2px"><b><u>Bank</u></b></font>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td>
                                        Bank Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBankNameAdd" runat="server" CssClass="input_mandatory" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        ACH Bank Code:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBankCodeAdd" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        ACH Branch Code:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBranchCodeAdd" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <eluc:Hard ID="ddlSwiftcodetype" runat="server" HardList="<%# PhoenixRegistersHard.ListHard(1, 198) %>"
                                                CssClass="input" HardTypeCode="198" Width="300px" />
                                        </b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSwiftCodeAdd" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        IBAN Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIBANNumberAdd" runat="server" CssClass="input_mandatory" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        Account Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAccountNumberAdd" runat="server" CssClass="input_mandatory" MaxLength="100">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Currency:
                                    </td>
                                    <td>
                                        <eluc:Currency runat="server" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                            ID="ucCurrencyBankAdd" AppendDataBoundItems="true" AutoPostBack="false" CssClass="dropdown_mandatory" />
                                    </td>
                                    <td>
                                        Beneficiary Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBeneficiaryName" runat="server" CssClass="input_mandatory" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        Contact Name:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtContactNameAdd" CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <tr>
                                        <td>
                                            EMail:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtEmailidAdd" MaxLength="50" CssClass="input" Width="250"></asp:TextBox>
                                        </td>
                                        <td>
                                            Phone Number:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPhoneNumberAdd" CssClass="input" MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td>
                                            Bank Country:
                                        </td>
                                        <td>
                                            <eluc:Country runat="server" ID="ucBankCountry" CssClass="input" AppendDataBoundItems="true"
                                                CountryList="<%# PhoenixRegistersCountry.ListCountry(null)%>" />
                                        </td>
                                    </tr>
                                </tr>
                                <tr>
                                    <td>
                                        Additional Bank Information:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtAdditionalbankinfo" MaxLength="30" CssClass="input"
                                            Width="250"></asp:TextBox>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <font size="2px"><b><u>Intermediate Bank</u></b></font>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Bank Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIBankNameAdd" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        ACH Bank Code:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIBankCodeAdd" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        ACH Branch Code:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIBranchCodeAdd" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <eluc:Hard ID="ddlSwiftcodetypeinter" runat="server" HardList="<%# PhoenixRegistersHard.ListHard(1, 198) %>"
                                                CssClass="input" HardTypeCode="198" Width="300px" />
                                            <b></b></b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtISwiftCodeAdd" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        IBAN Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIBANNumberAdd" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                    <td>
                                        Account Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIAccountNumberAdd" runat="server" CssClass="input" MaxLength="100">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Currency:
                                    </td>
                                    <td>
                                        <eluc:Currency runat="server" ID="ucCurrencyIBankAdd" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                            AppendDataBoundItems="true" CssClass="input" />
                                    </td>
                                    <td>
                                        Bank Country:
                                    </td>
                                    <td>
                                        <eluc:Country runat="server" ID="ucIntermediateBankCountry" CssClass="input" AppendDataBoundItems="true"
                                            CountryList="<%# PhoenixRegistersCountry.ListCountry(null)%>" />
                                    </td>
                                </tr>
                            </table>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                            <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                ToolTip="Delete"></asp:ImageButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                CommandName="Save" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                ToolTip="Save"></asp:ImageButton>
                            <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                ToolTip="Add New"></asp:ImageButton>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div id="divPage" style="position: relative;">
            <table width="100%" border="0" class="datagrid_pagestyle">
                <tr>
                    <td nowrap="nowrap" align="center">
                        <asp:Label ID="lblPagenumber" runat="server">
                        </asp:Label>
                        <asp:Label ID="lblPages" runat="server">
                        </asp:Label>
                        <asp:Label ID="lblRecords" runat="server">
                        </asp:Label>&nbsp;&nbsp;
                    </td>
                    <td nowrap="nowrap" align="left" width="50px">
                        <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                    </td>
                    <td width="20px">
                        &nbsp;
                    </td>
                    <td nowrap="nowrap" align="right" width="50px">
                        <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                    </td>
                    <td nowrap="nowrap" align="center">
                        <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                        </asp:TextBox>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                            Width="40px"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>
