<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReportAnnualLedgerAccountForPCL.aspx.cs"
    Inherits="AccountsReportAnnualLedgerReportForPCL" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AccountsReportAnnualLedgerAccount</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .style1 {
                width: 61px;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="AccReport" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" Visible="false" />
            <eluc:TabStrip ID="MenuMainFilter" runat="server" OnTabStripCommand="MenuMainFilter_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="AnnualReport" runat="server" OnTabStripCommand="AnnualReport_TabStripCommand"></eluc:TabStrip>
            <table style="width: 70%">
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblUserAccess" runat="server" Text="User Access Level"></telerik:RadLabel>
                    </td>
                    <td style="width: 25%">
                        <telerik:RadTextBox ID="txtUserAccess" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%"></td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadTextBox ID="txtCompany" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 25%">
                        <eluc:Date ID="txtFromDate" runat="server" Width="144px" CssClass="input_mandatory" />
                    </td>
                    <td style="width: 15%"></td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%">
                        <eluc:Date ID="txtToDate" runat="server" Width="144px" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td style="width: 25%">
                        <telerik:RadDropDownList ID="ddlType" runat="server" CssClass="input_mandatory" Width="144px">
                            <Items>
                                <telerik:DropDownListItem Value="1" Text="Base Currency"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Value="2" Text="Reportting Currency"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Value="3" Text="Prime Currency"></telerik:DropDownListItem>
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td style="width: 15%"></td>
                    <td style="width: 5%">
                        <telerik:RadLabel ID="lblcurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td style="width: 15%">
                        <eluc:Currency runat="server" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' Width="144px"
                            ID="ucCurrency" AppendDataBoundItems="true" AutoPostBack="true" />

                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <font color="blue">
                                    <br />Note: If User Access Level is Normal he can see only Normal Accounts.
                        <br />
                        If User Access Level is Restricted he can see both Normal and Restricted Accounts.
                        <br />
                        If User Access Level is Confidential he can see all Accounts.
                        </font>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccountCodeAccountDescription" runat="server" Text="Account Code/Account Description"></telerik:RadLabel>
                        <telerik:RadTextBox runat="server" ID="txtAccountSearch"
                            MaxLength="10" Width="150px">
                        </telerik:RadTextBox>
                        <asp:ImageButton runat="server"
                            ImageUrl="<%$ PhoenixTheme:images/search.png %>" ID="cmdAccountSearch" OnClick="cmdSearchAccount_Click"
                            ToolTip="Search" />
                    </td>
                    <td style="width: 3%"></td>
                    <td>
                        <telerik:RadLabel ID="lblFromAccount" runat="server" Text="From Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadMaskedTextBox ID="txtFromAccount" runat="server" Mask="#######" AutoPostBack="true" OnTextChanged="txtFromAccount_TextChanged"></telerik:RadMaskedTextBox>
                    </td>
                    <td style="width: 2%"></td>
                    <td>
                        <telerik:RadLabel ID="lblToAccount" runat="server" Text="To Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadMaskedTextBox ID="txtToAccount" runat="server" Mask="#######" AutoPostBack="true" OnTextChanged="txtToAccount_TextChanged"></telerik:RadMaskedTextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccount" runat="server" Text="Account"></telerik:RadLabel>
                    </td>
                    <td style="width: 23.5%"></td>
                    <td>
                        <telerik:RadCheckBoxList runat="server" ID="cblAccount" Height="143px" Width="400px" Style="overflow: auto;" CssClass="input" Columns="1" DataBindings-DataTextField="FLDaccoandept"
                            AutoPostBack="True" OnSelectedIndexChanged="AccountSelection" Direction="Vertical" Layout="Flow" DataBindings-DataValueField="FLDACCOUNTID">
                        </telerik:RadCheckBoxList>
                    </td>
                      <td>
                       <telerik:RadButton runat="server" ID="btnselectedsubaccount" Text="Shows Selected Account" OnClick="btnselectedsubaccount_Click" />
                    </td>
                </tr>
            </table>
            <table style="height: 23%">
                <tr>
                    <td style="width: 16%"></td>
                    <td style="height: 100%">
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvSelectedAccount" Height="50%" runat="server" AllowSorting="true" Width="50%"
                            CellSpacing="0" GridLines="None" OnItemCommand="gvSelectedAccount_ItemCommand" OnNeedDataSource="gvSelectedAccount_NeedDataSource"
                            ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Selected Account(s)">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblAccountName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                CommandName="DELETE" ID="cmdDelete" ToolTip="Delete"></asp:ImageButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
