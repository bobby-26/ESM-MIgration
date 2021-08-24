<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewRegisteredBankAccount.aspx.cs"
    Inherits="CrewRegisteredBankAccount" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Registered Bank Account</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewBankAccountList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
        </telerik:RadSkinManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvBankInformation" Height="72%" runat="server"
            AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
            OnNeedDataSource="gvBankInformation_NeedDataSource" EnableHeaderContextMenu="true"
            AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Bank Information :" HeaderStyle-Font-Bold="true">
                        <HeaderStyle Width="100%"  />
                        <ItemTemplate>
                            <font size="2px"><b><u>Bank</u></b></font>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td width="10%">
                                        <b>
                                            <telerik:RadLabel ID="lblBankNameH" Text="Bank Name:" runat="server"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td width="19%">
                                        <telerik:RadLabel ID="lblBankid" Visible="false" Text='<%#Bind("FLDBANKACCOUNTID") %>' runat="server"></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblBankname" Text='<%#Bind("FLDBANKNAME") %>' runat="server"></telerik:RadLabel>
                                    </td>
                                    <td width="10%">
                                        <b>
                                            <telerik:RadLabel ID="lblIFSCcode1" Text="IFSC Code:" runat="server"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td width="20%">
                                        <telerik:RadLabel ID="lblIFSCCode" runat="server" Text='<%#Bind("FLDBANKIFSCCODE") %>'></telerik:RadLabel>
                                    </td>
                                    <td width="10%">
                                        <b>
                                            <telerik:RadLabel ID="lblBranch1" runat="server" Text="Branch Branch:"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td width="21%">
                                        <telerik:RadLabel ID="lblBranch" runat="server" Text='<%#Bind("FLDBANKBRANCH") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblBankCode" runat="server" Text="Bank Code:"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblSwiftcode" runat="server" Text='<%#Bind("FLDBANKSWIFTCODE") %>'></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblInactive" runat="server" Text="InActive YN: "></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblInActiveYN" runat="server" Text='<%#Bind("FLDINACTIVEYNNAME") %>'></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblAccountno" runat="server" Text="Account Number:"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblAccountNumber" runat="server" Text='<%#Bind("FLDACCOUNTNUMBER") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblSwift" runat="server" Text="Swift Code:"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblBankSwiftCode" runat="server" Text='<%#Bind("FLDBANKSWIFTCODE") %>'></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblCurrency1" runat="server" Text="Currency:"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%#Bind("FLDCURRENCYNAME") %>'></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblBankCountry1" runat="server" Text="Bank Country:"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblBankCountry" runat="server" Text='<%#Bind("FLDBANKCOUNTRYCODE") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <font size="2px"><b><u>Intermediate Bank</u></b></font>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblBankName2" Text="Bank Name:" runat="server"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td>

                                        <telerik:RadLabel ID="txtibankname" runat="server" Text='<%#Bind("FLDINTERMEDIATEBANK") %>'></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblIFSCCode2" Text="IFSC Code:" runat="server"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblibankswiftcode" runat="server" Text='<%#Bind("FLDINTERMEDIATEBANKIFSCCODE") %>'></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblIBANno" runat="server" Text="IBAN Number (Int. Bank):"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblibranchcode" runat="server" Text='<%#Bind("FLDIBANNUMBER") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblSwiftcode1" runat="server" Text="Swift Code:"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lbliswiftcode" runat="server" Text='<%#Bind("FLDINTERMEDIATEBANKSWIFTCODE") %>'></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblAccountname" runat="server" Text="Account Name:"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="Label1" runat="server" Text='<%#Bind("FLDIBANKACCOUNTNAME") %>'></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblAccountno1" runat="server" Text="Account Number:"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lbliAccountNumber" runat="server" Text='<%#Bind("FLDIBANKACCOUNTNUMBER") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblCurrency2" runat="server" Text="Currency:"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lbliCurrency" runat="server" Text='<%#Bind("FLDICURRENCYNAME") %>'></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblBankCountry2" runat="server" Text="Bank Country:"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblibankcountry" runat="server" Text='<%#Bind("FLDINTERMEDIATEBANKCOUNTRYCODE") %>'></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblModifiedB" runat="server" Text="Modified By:"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblModifiedBy" runat="server" Text='<%#Bind("FLDMODIFIEDBYNAME") %>'></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <b>
                                            <telerik:RadLabel ID="lblModifiedD" runat="server" Text="Modified Date:"></telerik:RadLabel>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">

                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

    </form>
</body>
</html>
