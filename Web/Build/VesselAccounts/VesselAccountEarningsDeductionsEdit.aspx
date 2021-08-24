<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountEarningsDeductionsEdit.aspx.cs"
    Inherits="VesselAccounts_VesselAccountEarningsDeductionsEdit" EnableEventValidation="false" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BankAccount" Src="~/UserControls/UserControlEmployeeBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlVesselMappingCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <style type="text/css">
            .hidden {
                display: none;
            }

            .aligntext {
                text-align: right;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:TabStrip ID="MenuSave" runat="server" OnTabStripCommand="MenuSave_TabStripCommand"></eluc:TabStrip>
        <eluc:Status runat="server" ID="ucStatus" />
        <table width="100%">
            <tr>
                <td>File No.
                </td>
                <td>
                    <telerik:RadTextBox ID="txtfileno" CssClass="readonlytextbox" ReadOnly="true" runat="server"></telerik:RadTextBox>
                </td>
                <td>Name
                </td>
                <td>
                    <telerik:RadTextBox ID="txtname" CssClass="readonlytextbox" ReadOnly="true" runat="server"></telerik:RadTextBox>
                </td>
                <td>Rank
                </td>
                <td>
                    <telerik:RadTextBox ID="txtrank" CssClass="readonlytextbox" ReadOnly="true" runat="server"></telerik:RadTextBox></td>
            </tr>
            <tr>
                <td>Month-Year
                </td>
                <td>
                    <telerik:RadTextBox ID="txtMonthAndYear" CssClass="readonlytextbox" ReadOnly="true" runat="server"></telerik:RadTextBox>
                </td>
                <td>Cash Onboard</td>
                <td>
                    <telerik:RadTextBox ID="txtcashonboard" CssClass="readonlytextbox txtNumber" ReadOnly="true"
                        runat="server">
                    </telerik:RadTextBox>
                </td>
                <td>Balance of Wage
                    &nbsp;
                </td>
                <td>
                    <telerik:RadTextBox ID="txtwage" CssClass="readonlytextbox txtNumber" ReadOnly="true" runat="server"></telerik:RadTextBox>
                    <telerik:RadLabel ID="lblon" Text="On" runat="server"></telerik:RadLabel>
                    <eluc:Date ID="txtDate" runat="server" CssClass="input" />
                    <asp:ImageButton runat="server" AlternateText="Calculate" ImageUrl="<%$ PhoenixTheme:images/Cal.png %>"
                        CommandName="SAVE" ID="cmdCalculateWage" ToolTip="Calculate Wage" Style="cursor: pointer; vertical-align: top"
                        OnClick="cmdCalculateWage_Click"></asp:ImageButton>
                    &nbsp;
                </td>
            </tr>
        </table>
        <div id="divGrid" runat="server">
            <telerik:RadGrid ID="gvCrewSearch" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="true" EnableViewState="false"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowSorting="true" EnableHeaderContextMenu="true" ShowFooter="true"
                OnNeedDataSource="gvCrewSearch_NeedDataSource" OnItemDataBound="gvCrewSearch_ItemDataBound" OnItemCommand="gvCrewSearch_ItemCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    DataKeyNames="FLDEARNINGDEDUCTIONID" GroupHeaderItemStyle-Wrap="false" Width="100%">
                    <%--                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="false" ShowRefreshButton="false" />--%>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Transaction" Name="Transaction" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Report" Name="Report" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="160px" ItemStyle-Width="160px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEarDedId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDEARNINGDEDUCTIONID"]%>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container, "DataItem.FLDDATE", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtDateedit" runat="server" CssClass="input_mandatory" Text='<%# ((DataRowView)Container.DataItem)["FLDDATE"]%>' />
                                <telerik:RadLabel ID="lblEarDedEditId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDEARNINGDEDUCTIONID"]%>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="txtDateadd" runat="server" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose" HeaderStyle-Width="160px" ItemStyle-Width="160px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpurpose" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDPURPOSE"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtpurposeEdit" CssClass="gridinput_mandatory" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDPURPOSE"]%>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtpurposeadd" CssClass="gridinput_mandatory" runat="server" Text='<%#(ViewState["type"]).ToString()=="3"?"Cash Advance":"Radio Log" %>'></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="80px" ItemStyle-Width="75px" ColumnGroupName="Transaction">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCURRENCY" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDBASECURRENCYCODE"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Currency ID="ddlCurrencyEdit" runat="server" CssClass="input_mandatory"
                                    VesselId='<%#((DataRowView)Container.DataItem)["FLDVESSELID"]%>'
                                    AutoPostBack="true" Width="70px" SelectedCurrency='<%#((DataRowView)Container.DataItem)["FLDBASECURRENCY"]%>'
                                    OnTextChangedEvent="ddlCurrency_SelectedIndexChanged" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Currency ID="ddlCurrencyAdd" runat="server" CssClass="input_mandatory"
                                    AutoPostBack="true" Width="70px" OnTextChangedEvent="ddlCurrency_SelectedIndexChanged" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" ColumnGroupName="Transaction" HeaderStyle-Width="80px" ItemStyle-Width="75px">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDBASEAMOUNT"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input_mandatory" MaxLength="8"
                                    Width="75px" Text='<%#((DataRowView)Container.DataItem)["FLDBASEAMOUNT"]%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtAmountadd" runat="server" CssClass="input_mandatory" MaxLength="8"
                                    Width="75px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="70px" ItemStyle-Width="70px" ColumnGroupName="Report">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVESSELCURRENCY" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELCURRENCYCODE"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" ColumnGroupName="Report" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVESSELAmount" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDAMOUNT"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Exchange Rate" HeaderStyle-Wrap="true" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDEXCHANGERATE"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtExchangeRateEdit" runat="server" CssClass="input" DecimalPlace="17" MaxLength="25"
                                    Width="90px" Text='<%#((DataRowView)Container.DataItem)["FLDEXCHANGERATE"]%>' />
                                <telerik:RadLabel ID="lblExchangeRate" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDEXCHANGERATE"]%>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtExchangeRateAdd" runat="server" CssClass="input" DecimalPlace="17" MaxLength="25"
                                    Width="90px" Text="" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
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
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" Scrolling-EnableNextPrevFrozenColumns="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>
        </div>
        <div id="div1" runat="server">
            <telerik:RadGrid ID="gvAllotment" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="true" EnableViewState="false"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowSorting="true" EnableHeaderContextMenu="true" ShowFooter="true"
                OnNeedDataSource="gvAllotment_NeedDataSource" OnItemDataBound="gvAllotment_ItemDataBound" OnItemCommand="gvAllotment_ItemCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    DataKeyNames="FLDEARNINGDEDUCTIONID" GroupHeaderItemStyle-Wrap="false" Width="100%">
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Transaction" Name="Transaction" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Report" Name="Report" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEarDedAllotmentId" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDEARNINGDEDUCTIONID"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDDTKEY"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose" HeaderStyle-Width="160px" ItemStyle-Width="160px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpurpose" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDPURPOSE"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblEarDedAllotmentEditId" Visible="false" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDEARNINGDEDUCTIONID"]%>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtpurposeEdit" CssClass="gridinput_mandatory" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDPURPOSE"]%>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtpurposeadd" CssClass="gridinput_mandatory" runat="server" Text='<%#(ViewState["type"]).ToString()=="4"?"Allotment":"Special Allotment" %>'></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="80" ItemStyle-Width="80px" ColumnGroupName="Transaction">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCURRENCY" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDBASECURRENCYCODE"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Currency ID="ddlCurrencyEdit" runat="server" CssClass="input_mandatory"
                                    VesselId='<%#((DataRowView)Container.DataItem)["FLDVESSELID"]%>'
                                    AutoPostBack="true" Width="75px" SelectedCurrency='<%#((DataRowView)Container.DataItem)["FLDBASECURRENCY"]%>'
                                    OnTextChangedEvent="ddlCurrency_SelectedIndexChanged" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Currency ID="ddlCurrencyAdd" runat="server" CssClass="input_mandatory"
                                    AutoPostBack="true" Width="75px" OnTextChangedEvent="ddlCurrency_SelectedIndexChanged" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" ColumnGroupName="Transaction" HeaderStyle-Width="90px" ItemStyle-Width="90px">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDBASEAMOUNT"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input_mandatory" MaxLength="8"
                                    Width="80px" Text='<%#((DataRowView)Container.DataItem)["FLDBASEAMOUNT"]%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtAmountadd" runat="server" CssClass="input_mandatory" MaxLength="8"
                                    Width="80px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Bank Account/Beneficiary" HeaderStyle-Width="210px" ItemStyle-Width="210px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblbank" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDBANKACCOUNTNUMBER"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:BankAccount ID="ddlBankAccountEdit" runat="server" CssClass="dropdown_mandatory"
                                    SelectedBankAccount='<%#((DataRowView)Container.DataItem)["FLDBANKACCOUNTID"]%>' Width="180px"
                                    AppendDataBoundItems="true" EmployeeId='<%#((DataRowView)Container.DataItem)["FLDEMPLOYEEID"] %>'
                                    BankAccountList='<%#PhoenixVesselAccountsEmployeeBankAccount.ListEmployeeBankAccount(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(((DataRowView)Container.DataItem)["FLDEMPLOYEEID"].ToString()))%>' />
                            </EditItemTemplate>
                            <FooterStyle Wrap="False" HorizontalAlign="Left"></FooterStyle>
                            <FooterTemplate>
                                <eluc:BankAccount ID="ddlBankAccount" runat="server" CssClass="dropdown_mandatory" Width="180px"
                                    AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="80px" ItemStyle-Width="75px" ColumnGroupName="Report">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVESSELCURRENCY" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELCURRENCYCODE"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" ColumnGroupName="Report" HeaderStyle-Width="80px" ItemStyle-Width="75px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVESSELAmount" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDAMOUNT"] %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Exchange Rate" HeaderStyle-Wrap="true" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDEXCHANGERATE"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtExchangeRateEdit" runat="server" CssClass="input" DecimalPlace="17" MaxLength="25"
                                    Width="90px" Text='<%#((DataRowView)Container.DataItem)["FLDEXCHANGERATE"]%>' />
                                <telerik:RadLabel ID="lblExchangeRate" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDEXCHANGERATE"]%>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtExchangeRateAdd" runat="server" CssClass="input" DecimalPlace="17" MaxLength="25"
                                    Width="90px" Text="" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" Visible="false">
                            <ItemTemplate>
                                <%--<asp:Label ID="lbldate" runat="server" Text='<%# DataBinder.Eval(Container,"FLDDATE"] %>'></asp:Label>--%>
                                <%# DataBinder.Eval(Container, "DataItem.FLDDATE", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtDateedit" runat="server" CssClass="input_mandatory" Text='<%# ((DataRowView)Container.DataItem)["FLDDATE"]%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="txtDateadd" runat="server" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" Visible="true" AlternateText="Confirm" CommandName="CONFIRM" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdConfirm"
                                    ToolTip="Confirm">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                    ToolTip="Upload Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
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
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" Scrolling-EnableNextPrevFrozenColumns="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>
        </div>
    </form>
</body>
</html>
