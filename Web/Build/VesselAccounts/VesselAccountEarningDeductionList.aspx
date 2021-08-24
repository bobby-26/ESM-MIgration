<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountEarningDeductionList.aspx.cs"
    Inherits="VesselAccountEarningDeductionList" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlVesselMappingCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Earnings/Deductions</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <style type="text/css">
            .hidden {
                display: none;
            }
        </style>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="EarningandDetection">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuEarDedGeneral" runat="server" OnTabStripCommand="MenuEarDedGeneral_TabStripCommand"></eluc:TabStrip>
            <eluc:Status runat="server" ID="ucStatus" />
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblrank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMonth" runat="server" Text="Month-Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMonth" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="150">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="Literal1" runat="server" Text="Cash Onboard"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCashonBalance" runat="server" CssClass="readonlytextbox txtNumber"
                            ReadOnly="true" Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblwages" runat="server" Text="Balance of Wage"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBalanceonWage" runat="server"
                            CssClass="readonlytextbox txtNumber" ReadOnly="true" Width="150">
                        </telerik:RadTextBox>
                        <telerik:RadLabel ID="lblon" runat="server" Text="On"></telerik:RadLabel>
                        <eluc:Date ID="txtDate" runat="server"/>
                        <asp:ImageButton ID="cmdCalculateWage0" runat="server"
                            AlternateText="Calculate" CommandName="SAVE"
                            ImageUrl="<%$ PhoenixTheme:images/Cal.png %>" OnClick="cmdCalculateWage_Click"
                            Style="cursor: pointer; vertical-align: top" ToolTip="Calculate Wage" />
                    </td>
                </tr>
            </table>
            <telerik:RadGrid ID="gvEarning" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="true" EnableViewState="false"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" AllowPaging="true" AllowSorting="true" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvEarning_NeedDataSource" OnItemDataBound="gvEarning_ItemDataBound" OnItemCommand="gvEarning_ItemCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Transaction" Name="Transaction" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Report" Name="Report" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Entry Type">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEarningDeductionid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEARNINGDEDUCTIONID") %>' />
                                <telerik:RadLabel ID="lblEarningDeduction" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEARNINGDEDUCTION") %>' />
                                <telerik:RadLabel ID="lblEarningName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEARNINGNAME") %>' />
                                <telerik:RadLabel ID="lblhardcode" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDCODE") %>' />
                                <telerik:RadLabel ID="lblshortname" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>' />
                                <telerik:RadLabel ID="lblWageHeadId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDWAGEHEADID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEntryType" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENTRYTYPE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurpose" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>' />
                                <telerik:RadTextBox ID="txtPurposeEdit" runat="server" Width="95%"
                                    Text='<%#DataBinder.Eval(Container,"DataItem.FLDPURPOSE")%>'>
                                </telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="97px" ItemStyle-Width="97px" ColumnGroupName="Transaction">
                            <ItemTemplate>
                                <eluc:Currency ID="ddlCurrency" runat="server" CssClass="input_mandatory"
                                    VesselId='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELID")%>'
                                    AutoPostBack="true" Width="70px"
                                    OnTextChangedEvent="ddlCurrency_SelectedIndexChanged" />
                                <telerik:RadLabel ID="lblCurrency" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASECURRENCY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" ColumnGroupName="Transaction" HeaderStyle-Width="115px" ItemStyle-Width="115px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblamount" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASEAMOUNT") %>' />
                                <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input_mandatory" MaxLength="8"
                                    IsPositive='<%#DataBinder.Eval(Container,"DataItem.FLDEARNINGNAME").ToString() == "BRF" ? false : true %>'
                                    Width="90px" Text='<%#DataBinder.Eval(Container,"DataItem.FLDBASEAMOUNT")%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="70px" ItemStyle-Width="70px" ColumnGroupName="Report">
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDVESSELCURRENCYCODE") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" ColumnGroupName="Report" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvesselamount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="160px" ItemStyle-Width="160px">
                            <ItemTemplate>
                                <eluc:Date ID="txtdate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>' />
                                <telerik:RadLabel ID="lbldate" runat="server" Visible="false" Text='  <%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Exchange Rate" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <eluc:Number ID="txtExchangeRate" runat="server" DecimalPlace="17" MaxLength="25"
                                    Width="95%" Text='<%#DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATE")%>' />
                                <telerik:RadLabel ID="lblExchangeRate" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" Visible="false" CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
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
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" ScrollHeight="375px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
