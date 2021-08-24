<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersBankInformationList.aspx.cs"
    Inherits="RegistersBankInformationtionList" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagName="Currency" TagPrefix="eluc" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bank Information</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function ucConfirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBankInformation" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
       <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="false">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuBankMain" runat="server" OnTabStripCommand="BankMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuRegistersBankInformation" runat="server" OnTabStripCommand="RegistersBankInformation_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvBankInformation" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="true" EnableViewState="false"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowCustomPaging="true" AllowSorting="true" EnableHeaderContextMenu="true" ShowFooter="true"
                OnNeedDataSource="gvBankInformation_NeedDataSource" OnItemDataBound="gvBankInformation_ItemDataBound" OnItemCommand="gvBankInformation_ItemCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Bank Information">
                            <ItemTemplate>
                                <font size="2px"><b><u>
                                    <telerik:RadLabel ID="lblBank" runat="server" Text="Bank"></telerik:RadLabel>
                                </u></b></font>
                                <table width="100%" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td width="10%">
                                            <b>
                                                <telerik:RadLabel ID="lblItemBankName" runat="server" Text="Bank Name:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td width="19%">
                                            <telerik:RadLabel ID="lblBankid" Visible="false" Text='<%#Bind("FLDBANKID") %>' runat="server"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblBankname" Text='<%#Bind("FLDBANKNAME") %>' runat="server"></telerik:RadLabel>
                                        </td>
                                        <td width="10%">
                                            <b>
                                                <telerik:RadLabel ID="lblItemACHBankCode" runat="server" Text="ACH Bank Code:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td width="20%">
                                            <telerik:RadLabel ID="lblBcode" runat="server" Text='<%#Bind("FLDBANKCODE") %>'></telerik:RadLabel>
                                        </td>
                                        <td width="10%">
                                            <b>
                                                <telerik:RadLabel ID="lblItemACHBranchCode" runat="server" Text="ACH Branch Code:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td width="21%">
                                            <telerik:RadLabel ID="lblBrcode" runat="server" Text='<%#Bind("FLDBRANCHCODE") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblswiftcodecaption" runat="server" Text='<%#Bind("FLDBANKSWIFTCODECAPTION") %>'></telerik:RadLabel>
                                                :</b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblSwiftcode" runat="server" Text='<%#Bind("FLDSWIFTCODE") %>'></telerik:RadLabel>
                                        </td>
                                        <%--                                    <td>
                                        <b><telerik:RadLabel ID="lblIBANNumber" runat="server" Text="IBAN Number:"></telerik:RadLabel></b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblIBANNumber" runat="server" Text='<%#Bind("FLDIBANNUMBER") %>'></telerik:RadLabel>
                                    </td>--%>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblInActiveYN" runat="server" Text="InActive YN:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkActiveYNItem" />
                                        </td>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemAccountNumber" runat="server" Text="Account Number:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblAccountNumber" runat="server" Text='<%#Bind("FLDACCOUNTNUMBER") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%#Bind("FLDCURRENCYNAME") %>'></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemBeneficiaryName" runat="server" Text="Beneficiary Name:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblBeneficiaryName" runat="server" Text='<%#Bind("FLDBENEFICIARYNAME") %>'></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblContactPersonName" runat="server" Text="Contact Person Name:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblContactName" runat="server" Text='<%#Bind("FLDCONTACTNAME") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>

                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemPhoneNumber" runat="server" Text="Phone Number:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblPhoneNumber" runat="server" Text='<%#Bind("FLDPHONENUMBER") %>'></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemBankCountry" runat="server" Text="Bank Country:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td colspan="3">
                                            <telerik:RadLabel ID="lblBankCountry" runat="server" Text='<%#Bind("FLDBANKCOUNTRYCODE") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemAdditionalBankInformation" runat="server" Text="Additional Bank Information:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <%-- <telerik:RadLabel ID="lblAdditionalbankinformation" runat="server" Text='<%#Bind("FLDADDITIONALBANKINFO") %>'></telerik:RadLabel> --%>
                                            <telerik:RadLabel ID="lblAdditionalbankinformation" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDADDITIONALBANKINFO").ToString().Length>50 ? DataBinder.Eval(Container, "DataItem.FLDADDITIONALBANKINFO").ToString().Substring(0, 50) + "..." : DataBinder.Eval(Container, "DataItem.FLDADDITIONALBANKINFO").ToString() %>'></telerik:RadLabel>
                                            <eluc:ToolTip ID="ucToolTipAdditionalbankinformation" TargetControlId="lblAdditionalbankinformation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDITIONALINFORMATION1")+"<br>"+ DataBinder.Eval(Container,"DataItem.FLDADDITIONALINFORMATION2")+"<br>"+ DataBinder.Eval(Container,"DataItem.FLDADDITIONALINFORMATION3")+"<br>"+ DataBinder.Eval(Container,"DataItem.FLDADDITIONALINFORMATION4") %>' />
                                            <telerik:RadLabel ID="lbldtkey" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>' Visible="false"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblDefaultBankCharges" runat="server" Text="Default Bank Charges"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel4" runat="server" Text='<%#Bind("FLDSUPPLIERBANKCHARGEBASIS") %>'></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemRemarks" runat="server" Text="Remarks:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%#Bind("FLDREMARKS") %>'></telerik:RadLabel>
                                             <eluc:ToolTip ID="ToolTip2" TargetControlId="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS")%>' Position="TopLeft" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblEMail" runat="server" Height="35px" Text="E-Mail:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td colspan="5">
                                            <telerik:RadTextBox ID="lblEmailid" runat="server" Wrap="true" Rows="2" BackColor="Transparent" ReadOnly="true" BorderStyle="None" BorderWidth="0" Width="99%" Text="" TextMode="MultiLine"></telerik:RadTextBox>
                                            <eluc:ToolTip ID="ToolTip1" TargetControlId="lblEmailid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAILID")%>' Position="TopLeft" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <font size="2px"><b><u>
                                                <telerik:RadLabel ID="lblIntermediateBank" runat="server" Text="Intermediate Bank"></telerik:RadLabel>
                                            </u></b></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemIntermediateBankName" runat="server" Text="Bank Name:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lbliBankid" Visible="false" Text='<%#Bind("FLDIBANKID") %>' runat="server"></telerik:RadLabel>
                                            <telerik:RadLabel ID="txtibankname" runat="server" Text='<%#Bind("FLDIBANKNAME") %>'></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemIntermediateACHBankCode" runat="server" Text="ACH Bank Code:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblibankcode" runat="server" Text='<%#Bind("FLDIBANKCODE") %>'></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemIntermediateACHBranchCode" runat="server" Text="ACH Branch Code:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblibranchcode" runat="server" Text='<%#Bind("FLDIBRANCHCODE") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lbliswiftcodecaption" runat="server" Text='<%#Bind("FLDIBANKSWIFTCODECAPTION") %>'></telerik:RadLabel>
                                                :</b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lbliswiftcode" runat="server" Text='<%#Bind("FLDISWIFTCODE") %>'></telerik:RadLabel>
                                        </td>
                                        <%--                                    <td>
                                        <b><telerik:RadLabel ID="lblIBANNumber" runat="server" Text="IBAN Number:"></telerik:RadLabel></b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lbliinanumber" runat="server" Text='<%#Bind("FLDIIBANNUMBER") %>'></telerik:RadLabel>
                                    </td>--%>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemIntermediateBankCountry" runat="server" Text="Bank Country:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblibankcountry" runat="server" Text='<%#Bind("FLDIBANKCOUNTRYCODE") %>'></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemIntermediateAccountNumber" runat="server" Text="Account Number:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lbliAccountNumber" runat="server" Text='<%#Bind("FLDIACCOUNTNUMBER") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemIntermediateCurrency" runat="server" Text="Currency:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lbliCurrency" runat="server" Text='<%#Bind("FLDICURRENCYNAME") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemIntermediateModifiedBy" runat="server" Text="Modified By:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblModifiedBy" runat="server" Text='<%#Bind("FLDMODIFIEDBY") %>'></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemIntermediateModifiedDate" runat="server" Text="Modified Date:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblModifiedDate" runat="server" Text='<%#Bind("FLDMODIFIEDDATE") %>'></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <font size="2px"><b><u>
                                    <telerik:RadLabel ID="lblBank" runat="server" Text="Bank"></telerik:RadLabel>
                                </u></b></font>
                                <table width="100%" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblEditBankName" runat="server" Text="Bank Name:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblBankidEdit" Visible="false" Text='<%#Bind("FLDBANKID") %>' runat="server"></telerik:RadLabel>
                                            <telerik:RadTextBox ID="txtBankNameEdit" runat="server" Text='<%#Bind("FLDBANKNAME") %>'
                                                CssClass="input_mandatory" MaxLength="35">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblEditACHBankCode" runat="server" Text="ACH Bank Code:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBankCodeEdit" runat="server" Text='<%#Bind("FLDBANKCODE") %>'
                                                CssClass="input" MaxLength="100">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblEditACHBranchCode" runat="server" Text="ACH Branch Code:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBranchCodeEdit" runat="server" Text='<%#Bind("FLDBRANCHCODE") %>'
                                                CssClass="input" MaxLength="100">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <eluc:Hard ID="ddlSwiftcodetypeedit" runat="server" HardList="<%# PhoenixRegistersHard.ListHard(1, 198) %>"
                                                    CssClass="dropdown_mandatory" HardTypeCode="198" Width="300px" />
                                                <telerik:RadLabel ID="lblswiftcaptioncode" Visible="false" Text='<%#Bind("FLDISWIFTCODECAPTIONCODE") %>'
                                                    runat="server">
                                                </telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtSwiftCodeEdit" runat="server" Text='<%#Bind("FLDSWIFTCODE") %>'
                                                CssClass="input_mandatory" MaxLength="11">
                                            </telerik:RadTextBox>
                                        </td>
                                        <%--                                    <td>
                                        <telerik:RadLabel ID="lblIBANNumber" runat="server" Text="IBAN Number:"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtIBANNumberEdit" runat="server" Text='<%#Bind("FLDIBANNUMBER") %>'
                                            CssClass="input_mandatory" MaxLength="35"></telerik:RadTextBox>
                                    </td>--%>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblInActiveYN" runat="server" Text="InActiveYN:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkActiveYN" runat="server" />
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblEditAccountNumber" runat="server" Text="Account Number:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtAccountNumberEdit" runat="server" Text='<%#Bind("FLDACCOUNTNUMBER") %>'
                                                CssClass="input_mandatory" MaxLength="35">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblEditCurrency" runat="server" Text="Currency:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <%--<eluc:Currency runat="server" ID="ucCurrencyBankEdit" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                                AppendDataBoundItems="true" AutoPostBack="false" CssClass="dropdown_mandatory" />--%>
                                            <telerik:RadComboBox ID="ucCurrencyBankEdit" runat="server" DataTextField="FLDCURRENCYCODE" DataValueField="FLDCURRENCYID" EnableLoadOnDemand="True"
                                             EmptyMessage= "--select--"  AutoPostBack="false" CssClass="dropdown_mandatory" Filter="Contains" DataSource='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' MarkFirstMatch="true"></telerik:RadComboBox>

                                            <telerik:RadLabel ID="lblCurrencyId" Visible="false" Text='<%#Bind("FLDCURRENCYID") %>'
                                                runat="server">
                                            </telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblEditBeneficiaryName" runat="server" Text="Beneficiary Name:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBeneficiaryEdit" runat="server" Text='<%#Bind("FLDBENEFICIARYNAME") %>'
                                                CssClass="input_mandatory" MaxLength="35">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblContactName" runat="server" Text="Contact Name:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox runat="server" ID="txtContactNameEdit" CssClass="input" Text='<%#Bind("FLDCONTACTNAME") %>'
                                                MaxLength="100">
                                            </telerik:RadTextBox>
                                        </td>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblEditAdditionalBankInformation" runat="server" Text="Additional Bank Information:"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <%--<telerik:RadTextBox runat="server" ID="txtAdditionalbankinfoEdit" MaxLength="35" CssClass="input"
                                                Width="250"></telerik:RadTextBox>--%>
                                                <telerik:RadLabel ID="lblAdditionalbankinformationEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDADDITIONALBANKINFO").ToString().Length>50 ? DataBinder.Eval(Container, "DataItem.FLDADDITIONALBANKINFO").ToString().Substring(0, 50) + "..." : DataBinder.Eval(Container, "DataItem.FLDADDITIONALBANKINFO").ToString() %>'></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblEditPhoneNumber" runat="server" Text="Phone Number:"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox runat="server" ID="txtPhoneNumberEdit" CssClass="input" Text='<%#Bind("FLDPHONENUMBER") %>'
                                                    MaxLength="50">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblEditBankCountry" runat="server" Text="Bank Country:"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Country runat="server" ID="ucBankCountryEdit" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                                    CountryList="<%# PhoenixRegistersCountry.ListCountry(null,1)%>" Type="1" />
                                                <telerik:RadLabel ID="lblcountryid" Visible="false" Text='<%#Bind("FLDCOUNTRYCODE") %>'
                                                    runat="server">
                                                </telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblEMail" runat="server" Text="EMail:"></telerik:RadLabel>
                                            </td>
                                            <td colspan="5">
                                                <telerik:RadTextBox runat="server" ID="txtEmailidEdit" MaxLength="500" Text='<%#Bind("FLDEMAILID") %>'
                                                    CssClass="input_mandatory" Width="95%">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Default Bank Charges"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Hard ID="ddlbankEdit" runat="server" CssClass="input" HardTypeCode="133"
                                                    HardList='<%# PhoenixRegistersHard.ListHard(1, 133) %>' AppendDataBoundItems="true"
                                                    Width="180px" />
                                                <telerik:RadLabel ID="lbleditbankcharges" Visible="false" Text='<%#Bind("FLDSUPPLIERBANKCHARGEBASISCODE") %>'
                                                    runat="server">
                                                </telerik:RadLabel>
                                            </td>
                                            <td>
                                                <b>
                                                    <telerik:RadLabel ID="lblEditRemarks" runat="server" Text="Remarks:"></telerik:RadLabel>
                                                </b>
                                            </td>
                                            <td colspan="5">
                                                <telerik:RadTextBox ID="txtRemarks" runat="server" Text='<%#Bind("FLDREMARKS") %>'
                                                    Rows="2" Width="95%" CssClass="input">
                                                </telerik:RadTextBox>
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
                                            <font size="2px"><b><u>
                                                <telerik:RadLabel ID="lblIntermediateBank" runat="server" Text="Intermediate Bank"></telerik:RadLabel>
                                            </u></b></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblName" runat="server" Text="Name:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lbliBankidEdit" Visible="false" Text='<%#Bind("FLDIBANKID") %>' runat="server"></telerik:RadLabel>
                                            <telerik:RadTextBox ID="txtIBankNameEdit" runat="server" Text='<%#Bind("FLDIBANKNAME") %>'
                                                CssClass="input" MaxLength="35">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblEditIntermediateACHBankCode" runat="server" Text="ACH Bank Code:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtIBankCodeEdit" runat="server" Text='<%#Bind("FLDIBANKCODE") %>'
                                                CssClass="input" MaxLength="35">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblEditIntermediateACHBranchCode" runat="server" Text="ACH Branch Code:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtIBranchCodeEdit" runat="server" Text='<%#Bind("FLDIBRANCHCODE") %>'
                                                CssClass="input" MaxLength="100">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <eluc:Hard ID="ddlSwiftcodetypeinteredit" runat="server" HardList="<%# PhoenixRegistersHard.ListHard(1, 198) %>"
                                                    CssClass="input" HardTypeCode="198" Width="300px" />
                                                <telerik:RadLabel ID="lbliswiftcaptioncode" Visible="false" Text='<%#Bind("FLDISWIFTCODECAPTIONCODE") %>'
                                                    runat="server">
                                                </telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtISwiftCodeEdit" runat="server" Text='<%#Bind("FLDISWIFTCODE") %>'
                                                CssClass="input" MaxLength="11">
                                            </telerik:RadTextBox>
                                        </td>
                                        <%--                                    <td>
                                        <telerik:RadLabel ID="lblIBANNumber" runat="server" Text="IBAN Number:"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtIIBANNumberEdit" runat="server" Text='<%#Bind("FLDIIBANNUMBER") %>'
                                            CssClass="input" MaxLength="35"></telerik:RadTextBox>
                                    </td>--%>
                                        <td>
                                            <telerik:RadLabel ID="lblEditIntermediateBankCountry" runat="server" Text="Bank Country:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Country runat="server" ID="ucIntermediateBankCountryEdit" CssClass="input"
                                                AppendDataBoundItems="true" CountryList="<%# PhoenixRegistersCountry.ListCountry(null,1)%>"
                                                Type="1" />
                                            <telerik:RadLabel ID="lblIBankCountryid" Visible="false" Text='<%#Bind("FLDICOUNTRYCODE") %>'
                                                runat="server">
                                            </telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblEditIntermediateAccountNumber" runat="server" Text="Account Number:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtIAccountNumberEdit" runat="server" Text='<%#Bind("FLDIACCOUNTNUMBER") %>'
                                                CssClass="input" MaxLength="35">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblEditIntermediateCurrency" runat="server" Text="Currency:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Currency runat="server" ID="ucCurrencyIBankEdit" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                                AppendDataBoundItems="true" CssClass="input" />
                                            <telerik:RadLabel ID="lblICurrencyId" Visible="false" Text='<%#Bind("FLDICURRENCYID") %>'
                                                runat="server">
                                            </telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <font size="2px"><b><u>
                                    <telerik:RadLabel ID="lblBank" runat="server" Text="Bank"></telerik:RadLabel>
                                </u></b></font>
                                <table width="100%" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblFooterBankName" runat="server" Text="Bank Name:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBankNameAdd" runat="server" CssClass="input_mandatory" MaxLength="35"></telerik:RadTextBox>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblFooterACHBankCode" runat="server" Text="ACH Bank Code:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBankCodeAdd" runat="server" CssClass="input" MaxLength="100"></telerik:RadTextBox>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblFooterACHBranchCode" runat="server" Text="ACH Branch Code:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBranchCodeAdd" runat="server" CssClass="input" MaxLength="100"></telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <eluc:Hard ID="ddlSwiftcodetype" runat="server" HardList="<%# PhoenixRegistersHard.ListHard(1, 198) %>"
                                                    CssClass="dropdown_mandatory" HardTypeCode="198" Width="300px" />
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtSwiftCodeAdd" runat="server" CssClass="input_mandatory" MaxLength="11"></telerik:RadTextBox>
                                        </td>
                                        <%--                                    <td>
                                        <telerik:RadLabel ID="lblIBANNumber" runat="server" Text="IBAN Number:"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtIBANNumberAdd" runat="server" CssClass="input_mandatory" MaxLength="35"></telerik:RadTextBox>
                                    </td>--%>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblInActiveYN" runat="server" Text="InActiveYN:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkActiveYNAdd" runat="server" />
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblFooterAccountNumber" runat="server" Text="AccountNumber"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtAccountNumberAdd" runat="server" CssClass="input_mandatory" MaxLength="35">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblFooterCurrency" runat="server" Text="Currency:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <%--<eluc:Currency runat="server" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                                ID="ucCurrencyBankAdd" AppendDataBoundItems="true" AutoPostBack="false" CssClass="dropdown_mandatory" />--%>
                                           
                                            <telerik:RadComboBox ID="ucCurrencyBankAdd" runat="server" DataTextField="FLDCURRENCYCODE" DataValueField="FLDCURRENCYID" EnableLoadOnDemand="True"
                                             EmptyMessage="--select--" CssClass="dropdown_mandatory" Filter="Contains" DataSource='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' MarkFirstMatch="true"></telerik:RadComboBox>

                                            
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblFooterBeneficiaryName" runat="server" Text="Beneficiary Name:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtBeneficiaryName" runat="server" CssClass="input_mandatory" MaxLength="35"></telerik:RadTextBox>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblContactName" runat="server" Text="Contact Name:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox runat="server" ID="txtContactNameAdd" CssClass="input" MaxLength="100"></telerik:RadTextBox>
                                        </td>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblEMail" runat="server" Text="EMail:"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox runat="server" ID="txtEmailidAdd" MaxLength="500" CssClass="input_mandatory" Width="250"></telerik:RadTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblFooterPhoneNumber" runat="server" Text="Phone Number:"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox runat="server" ID="txtPhoneNumberAdd" CssClass="input" MaxLength="50"></telerik:RadTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblFooterBankCountry" runat="server" Text="Bank Country:"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Country runat="server" ID="ucBankCountry" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                                    CountryList="<%# PhoenixRegistersCountry.ListCountry(null,1)%>" Type="1" />
                                            </td>
                                        </tr>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblFooterAdditionalBankInformation" runat="server" Text="Additional Bank Information:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox runat="server" ID="txtAdditionalbankinfo" MaxLength="35" CssClass="input"
                                                Width="250">
                                            </telerik:RadTextBox>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="RadLabel3" runat="server" Text="Default Bank Charges"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Hard ID="ddlBankAdd" runat="server" CssClass="input" HardTypeCode="133"
                                                HardList='<%# PhoenixRegistersHard.ListHard(1, 133) %>' AppendDataBoundItems="true"
                                                Width="180px" />
                                        </td>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblFooterRemarks" runat="server" Text="Remarks:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtRemarksAdd" runat="server" Text='<%#Bind("FLDREMARKS") %>' TextMode="MultiLine"
                                                Rows="2" Width="200px" CssClass="input">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6">
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <font size="2px"><b><u>
                                                <telerik:RadLabel ID="lblIntermediateBank" runat="server" Text="Intermediate Bank"></telerik:RadLabel>
                                            </u></b></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblFooterIntermediateBankName" runat="server" Text="Bank Name:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtIBankNameAdd" runat="server" CssClass="input" MaxLength="35"></telerik:RadTextBox>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblFooterIntermediateACHBankCode" runat="server" Text="ACH Bank Code:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtIBankCodeAdd" runat="server" CssClass="input" MaxLength="35"></telerik:RadTextBox>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblFooterIntermediateACHBranchCode" runat="server" Text="ACH Branch Code:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtIBranchCodeAdd" runat="server" CssClass="input" MaxLength="100"></telerik:RadTextBox>
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
                                            <telerik:RadTextBox ID="txtISwiftCodeAdd" runat="server" CssClass="input" MaxLength="11"></telerik:RadTextBox>
                                        </td>
                                        <%--                                    <td>
                                        <telerik:RadLabel ID="lblIBANNumber" runat="server" Text="IBAN Number:"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtIIBANNumberAdd" runat="server" CssClass="input" MaxLength="35"></telerik:RadTextBox>
                                    </td>--%>
                                        <td>
                                            <telerik:RadLabel ID="lblFooterIntermediateBankCountry" runat="server" Text="Bank Country:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Country runat="server" ID="ucIntermediateBankCountry" CssClass="input" AppendDataBoundItems="true"
                                                Type="1" CountryList="<%# PhoenixRegistersCountry.ListCountry(null,1)%>" />
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblFooterIntermediateAccountNumber" runat="server" Text="Account Number:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadTextBox ID="txtIAccountNumberAdd" runat="server" CssClass="input" MaxLength="35">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblFooterIntermediateCurrency" runat="server" Text="Currency:"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Currency runat="server" ID="ucCurrencyIBankAdd" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                                AppendDataBoundItems="true" CssClass="input" />
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" FooterStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete" ToolTip="Delete">
                                 <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Bank Identifier" CommandName="BANKIDENTIFIER" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdBankIdentifier" ToolTip="Bank Identifier">
                                 <span class="icon"><i class="fas fa-archive"></i></span>
                                </asp:LinkButton>
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachment"
                                    ToolTip="Attachment"></asp:ImageButton>
                                <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>" ID="cmdNoAttachment"
                                    CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' Visible="false" ToolTip="No Attachment"></asp:ImageButton>
                                <%--                            <asp:ImageButton runat="server" AlternateText="Bank Identifier" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                CommandName="BANKIDENTIFIER" CommandArgument="<%# Container.DataSetIndex %>"
                                ID="cmdBankIdentifier" ToolTip="Bank Identifier"></asp:ImageButton>--%>
                                <%--                            <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdNoAttachment"
                                ToolTip="No Attachment" Visible="false"></asp:ImageButton>--%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave" ToolTip="Save">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel" ToolTip="Cancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="ADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd" ToolTip="Add New">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" Position="Bottom" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling SaveScrollPosition="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="ucConfirm" runat="server" Text="confirm" OnClick="btnConfirm_Click" CssClass="hidden" />
            <%--<eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnConfirm_Click" OKText="Yes" CancelText="No" />--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
