<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersReadOnlyBankInformationtionList.aspx.cs"
    Inherits="RegistersReadOnlyBankInformationtionList" MaintainScrollPositionOnPostback="true" %>

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
                OnNeedDataSource="gvBankInformation_NeedDataSource">
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
                                            <eluc:ToolTip ID="ucToolTipAdditionalbankinformation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDITIONALINFORMATION1")+"<br>"+ DataBinder.Eval(Container,"DataItem.FLDADDITIONALINFORMATION2")+"<br>"+ DataBinder.Eval(Container,"DataItem.FLDADDITIONALINFORMATION3")+"<br>"+ DataBinder.Eval(Container,"DataItem.FLDADDITIONALINFORMATION4") %>' />
                                            <telerik:RadLabel ID="lbldtkey" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>' Visible="false"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblDefaBankCh" runat="server" Text="Default Bank Charges"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblDefaBankCh1" runat="server" Text='<%#Bind("FLDSUPPLIERBANKCHARGEBASIS") %>'></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <b>
                                                <telerik:RadLabel ID="lblItemRemarks" runat="server" Text="Remarks:"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td>
                                            <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%#Bind("FLDREMARKS") %>'></telerik:RadLabel>
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
            <%--    <asp:Button ID="ucConfirm" runat="server" Text="confirm" OnClick="btnConfirm_Click" CssClass="hidden" />--%>
            <%--<eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnConfirm_Click" OKText="Yes" CancelText="No" />--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
