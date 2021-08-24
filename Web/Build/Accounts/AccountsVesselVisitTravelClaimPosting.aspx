<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselVisitTravelClaimPosting.aspx.cs"
    Inherits="AccountsVesselVisitTravelClaimPosting" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Projectcode" Src="~/UserControls/UserControlProjectCode.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Travel Claim</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVisitRegister" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" CssClass="hidden" />
            <eluc:Title runat="server" ID="Title1" Text="Travel Claim" Visible="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                CssClass="hidden" />
            <eluc:TabStrip ID="MenuTravelClaimMain" runat="server" OnTabStripCommand="MenuTravelClaimMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuTravelClaimSub" runat="server" OnTabStripCommand="MenuTravelClaimSub_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblemployee" runat="server" Text="Sub Account"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox ID="txtEmployee" Width="75%" runat="server" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblVesselVisitId" runat="server" Text="Form Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox ID="txtVesselVisitId" Width="75%" runat="server" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblReimbursementCurrency" runat="server" Text="Reimbursement Currency"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <%-- <telerik:RadTextBox ID="txtReimbursementCurrency" Width="75%" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true"></telerik:RadTextBox>--%>
                        <eluc:UserControlCurrency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" AutoPostBack="true" />
                    </td>
                    <td style="width: 20%">
                        <%--  <telerik:RadLabel ID="lblRevokeRemarks" runat="server" Text="Revoke Remarks"></telerik:RadLabel>--%>
                    </td>
                    <td style="width: 30%">
                        <%--<telerik:RadTextBox ID="txtRevokeRemarks" runat="server" CssClass="input" Width="90%" TextMode="MultiLine"></telerik:RadTextBox>--%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblPostingDate" runat="server" Text="Posting Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:UserControlDate ID="ucPostingDate" runat="server" CssClass="readonlytextbox"
                            ReadOnly="true" DatePicker="true" />
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblPaymentMode" runat="server" Text="Payment Mode"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadComboBox ID="ddlPaymentMode" runat="server" CssClass="dropdown_mandatory"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlPaymentMode_OnSelectedIndexChanged">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblPaymentVoucher" runat="server" Text="Payment Voucher Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox ID="txtPaymentVoucher" Width="75%" runat="server" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblBankingDetails" runat="server" Text="Employee Banking Details"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadComboBox ID="ddlBankAccount" runat="server" CssClass="dropdown_mandatory">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory"
                            DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID" Width="300px" OnTextChanged="ddlAccountDetails_TextChanged">
                        </telerik:RadComboBox>
                        <%-- <telerik:RadTextBox ID="txtVesselAccount" Width="75%" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true"></telerik:RadTextBox>--%>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Claim Voucher Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox ID="txtVoucherNumber" Width="75%" runat="server" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblcompany" runat="server" Text="Posting Company"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:UserControlCompany ID="ddlLiabilitycompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            Readonly="true" CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" />
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblPurpose" runat="server" Text="Purpose"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox ID="txtPurpose" Width="90%" runat="server" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblFromToDate" runat="server" Text="Visit start and End date"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:UserControlDate ID="ucFromDate" runat="server" />
                        &nbsp;<eluc:UserControlDate ID="ucToDate" runat="server" />
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblvoucher" runat="server" Text="Voucher"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox ID="txtVoucher" runat="server" CssClass="readonlytextbox" ReadOnly="true" Resize="Both"
                            Width="90%" Height="70px" TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="Onboard and Disembarkation Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucVisitStartDate" runat="server" />
                        &nbsp;<eluc:UserControlDate ID="ucVisitEndDate" runat="server" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblProjectCode" runat="server" Text="Project Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Projectcode ID="ucProjectcode" runat="server" AppendDataBoundItems="true" />
                    </td>

                </tr>

                <tr>
                    <td>Budget Code
                    </td>
                    <td>
                        <span id="spnPickListBulkBudget">
                            <telerik:RadTextBox ID="txtBulkBudgetCode" runat="server" MaxLength="20" CssClass="input"
                                Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBulkBudgetName" runat="server" Width="0px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowBulkBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtBulkBudgetId" runat="server" Width="0px" CssClass="input" OnTextChanged="txtBulkBudgetIdClick"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBulkBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>Owner Budget Code
                    </td>
                    <td>
                        <span id="spnPickListBulkOwnerBudget">
                            <telerik:RadTextBox ID="txtBulkOwnerBudgetCode" runat="server" MaxLength="20" CssClass="input"
                                Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBulkOwnerBudgetName" runat="server" Width="0px" CssClass="input"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowBulkOwnerBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtBulkOwnerBudgetId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBulkOwnerBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                        <asp:ImageButton runat="server" AlternateText="Refresh Budget Codes" ImageUrl="<%$ PhoenixTheme:images/refresh.png %>"
                            CommandName="RefreshBudgetCode" ID="btnRefreshBudgetCode" ToolTip="Refresh Budget Codes"
                            OnClick="RefreshBudgetCode"></asp:ImageButton>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRevokeRemarks" runat="server" Text="Revoke Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRevokeRemarks" runat="server" CssClass="input" Width="90%" TextMode="MultiLine" Resize="Both" Height="61px"></telerik:RadTextBox></td>
                    <td>
                        <telerik:RadLabel ID="lblremarks2" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td class="auto-style1">
                        <telerik:RadTextBox ID="txtremarks2" runat="server" CssClass="input" Width="76%" Height="70px" TextMode="MultiLine" Resize="Both"></telerik:RadTextBox>
                        <asp:ImageButton runat="server" AlternateText="AddRemark" ImageUrl="<%$ PhoenixTheme:images/add.png %>"
                            CommandName="AddRemark" ID="ImageButton1" ToolTip="Add" OnClick="RefreshRemarks"></asp:ImageButton>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuTravelClaim" runat="server" OnTabStripCommand="MenuTravelClaim_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvLineItem" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" CellPadding="3" OnItemDataBound="gvLineItem_ItemDataBound" ShowFooter="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnItemCommand="gvLineItem_ItemCommand" OnNeedDataSource="gvLineItem_NeedDataSource"
                ShowHeader="true" OnRowDeleting="gvLineItem_RowDeleting" EnableViewState="false" AllowSorting="true">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Item No." HeaderStyle-Width="5%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClaimLineitemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLAIMLINEITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRowNo" runat="server" CommandName="Select" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNO") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRejected" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREJECTED") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMarkup" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMARKUP") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="11%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:UserControlDate ID="ucDateEdit" runat="server" CssClass="input_mandatory" Width="99%"
                                    Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE") )%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:UserControlDate ID="ucDateAdd" runat="server" CssClass="input_mandatory" Width="99%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Country" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCountry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Country runat="server" ID="ucCountryEdit" CountryList='<%# PhoenixRegistersCountry.ListCountry(1)%>'
                                    AppendDataBoundItems="true" AutoPostBack="false" CssClass="dropdown_mandatory"
                                    Width="99%" />
                                <telerik:RadLabel ID="lblCountryCode" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYCODE") %>'
                                    runat="server">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Country runat="server" CountryList='<%# PhoenixRegistersCountry.ListCountry(1)%>'
                                    ID="ucCountryAdd" AppendDataBoundItems="true" AutoPostBack="false" CssClass="dropdown_mandatory"
                                    Width="99%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="12%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLAIMTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlTypeEdit" runat="server" CssClass="dropdown_mandatory" DataSource='<%# PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 134)%>'
                                    DataTextField="FLDQUICKNAME" DataValueField="FLDSHORTNAME" Width="99%">
                                </telerik:RadComboBox>
                                <telerik:RadLabel ID="lblTypeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLAIMTYPE") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlTypeAdd" runat="server" CssClass="dropdown_mandatory" DataSource='<%# PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 134)%>'
                                    DataTextField="FLDQUICKNAME" DataValueField="FLDSHORTNAME" Width="99%" AutoPostBack="true" EmptyMessage="Type to select Type" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code" HeaderStyle-Width="14%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListBudgetEdit">
                                    <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                        MaxLength="20" CssClass="input_mandatory" Width="80%" Enabled="false">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="hidden"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItem %>" />
                                    <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Width="0px" AutoPostBack="true" OnTextChanged="txtBudgetIdEdit_TextChanged" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListBudgetAdd">
                                    <telerik:RadTextBox ID="txtBudgetCodeAdd" runat="server" MaxLength="20" CssClass="input_mandatory"
                                        Width="80%">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetNameAdd" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowBudgetAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItem %>" />
                                    <telerik:RadTextBox ID="txtBudgetIdAdd" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdAdd" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner Budget Code" HeaderStyle-Width="14%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOwnerBudgetCodeEdit">
                                    <telerik:RadTextBox ID="txtOwnerBudgetCodeEdit" runat="server" CssClass="input_mandatory"
                                        MaxLength="20" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODENAME") %>'
                                        Width="60px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowOwnerBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItem%>" />
                                    <telerik:RadTextBox ID="txtOwnerBudgetIdEdit" runat="server" Width="0px" CssClass="input"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListOwnerBudgetCode">
                                    <telerik:RadTextBox ID="txtOwnerBudgetCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                                        Width="60px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetName" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowOwnerBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItem %>" />
                                    <telerik:RadTextBox ID="txtOwnerBudgetId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetgroupId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Project Code" HeaderStyle-Width="9%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProjectCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROJECTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Projectcode ID="ucProjectcodeEdit" runat="server" AppendDataBoundItems="true" Width="99%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Projectcode ID="ucProjectcodeAdd" runat="server" AppendDataBoundItems="true" CssClass="input" Width="99%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Expense Description" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpenseDesc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPENSEDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtExpenseDescEdit" runat="server" CssClass="input" Width="99%"
                                    TextMode="MultiLine" Resize="Both" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPENSEDESCRIPTION") %>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtExpenseDescAdd" runat="server" CssClass="input" Width="99%"
                                    TextMode="MultiLine" Resize="Both">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="9%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Currency runat="server" ID="ucCurrencyEdit" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                    AppendDataBoundItems="true" AutoPostBack="false" CssClass="dropdown_mandatory" Width="99%" />
                                <telerik:RadLabel ID="lblCurrencyId" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>'
                                    runat="server">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Currency runat="server" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' Width="99%"
                                    ID="ucCurrencyAdd" AppendDataBoundItems="true" AutoPostBack="false" CssClass="dropdown_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="7%" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input_mandatory txtNumber"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' Width="99%"></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtAmountAdd" runat="server" CssClass="input_mandatory txtNumber"
                                    Width="99%"></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Exchange" HeaderStyle-Width="12%" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExchange" Width="99%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASEEXCHANGERATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reimbursement Amount" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReimAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREIMBURSEMENTAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <%--<EditItemTemplate>
                                    <eluc:Number ID="txtReimAmountEdit" runat="server" CssClass="input_mandatory txtNumber"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDREIMBURSEMENTAMOUNT") %>' Width="120px">
                                    </eluc:Number>
                                </EditItemTemplate>--%>
                            <FooterTemplate>
                                <b>
                                    <%=strAmountTotal%>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="7%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="Edit" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Reject" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Delete" CommandArgument='<%# Container.DataItem %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataItem %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataItem%>' ID="cmdEditCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="ADDNEW" CommandArgument='<%# Container.DataItem %>' ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table id="Table2" width="100%" style="color: Blue">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbltext1" runat="server" Text="(This grid is for advance taken for this vessel visit id)" Font-Bold="true"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuTravelAdvance" runat="server" OnTabStripCommand="MenuTravelAdvance_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTravelAdvance" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" CellPadding="3" OnItemDataBound="gvTravelAdvance_ItemDataBound" ShowFooter="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnItemCommand="gvTravelAdvance_ItemCommand" OnRowDeleting="gvTravelAdvance_RowDeleting" OnNeedDataSource="gvTravelAdvance_NeedDataSource"
                ShowHeader="true" EnableViewState="false" AllowSorting="true">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Travel Advance Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTravelAdvanceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELADVANCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkTravelAdvanceNo" runat="server" CommandName="Select" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELADVANCENUMBER") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblManualYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANUALYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Paid Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaidDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPAIDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Amount" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaymentAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Return Amount" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReturnAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRETURNAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="MoneyChanger">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMoneyChanger" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONEYCHANGER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Claim Amount" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClaimAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLAIMAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Travelling Allowance Paid" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTravelAllowancePaid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELALLOWANCEPAID","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Travelling Allowance Charged" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTravelAllowanceCharged" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELALLOWANCECHARGED","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Balance" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALANCE","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>Total</b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Balance in SGD" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBalanceSGD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALANCESGD","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <%=strBalanceInSGD%>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Advance Status">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAdvanceStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAdvanceStatusCode" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCESTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="false" HeaderText="Visit Form No.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Reject" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Delete" CommandArgument='<%# Container.DataItem %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <table id="Table1" width="100%" style="color: Blue">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbltext" runat="server" Text=" (This grid is for advance taken for all other vessel visits with status of
                            Paid from Travel advance form)"
                            Font-Bold="true">
                        </telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuTravelAdvanceOther" runat="server" OnTabStripCommand="MenuTravelAdvanceOther_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTravelAdvanceOther" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowCustomPaging="true"
                Font-Size="11px" Width="100%" CellPadding="3" OnItemDataBound="gvTravelAdvanceOther_ItemDataBound" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnItemCommand="gvTravelAdvanceOther_ItemCommand" OnNeedDataSource="gvTravelAdvanceOther_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" AllowSorting="true">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="true">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Travel Advance Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTravelAdvanceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELADVANCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkTravelAdvanceNo" runat="server" CommandName="Select" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELADVANCENUMBER") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Amount" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaymentAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Return Amount" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReturnAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRETURNAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Balance" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALANCE","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Balance in SGD" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBalanceSGD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALANCESGD","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Advance Status">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAdvanceStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAdvanceStatusCode" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCESTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Visit Form No.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" CommandArgument='<%# Container.DataItem %>' ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
