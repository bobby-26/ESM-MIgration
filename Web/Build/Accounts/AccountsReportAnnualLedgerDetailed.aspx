<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReportAnnualLedgerDetailed.aspx.cs" Inherits="Accounts_AccountsReportAnnualLedgerDetailed" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AccountsReportAnnualLedgerAccount</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <style type="text/css">
            .style1 {
                width: 61px;
            }
        </style>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
                    <td style="width: 12%">
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" Width="144px" CssClass="input_mandatory" />
                    </td>
                    <td style="width: 15%"></td>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>

                    <td style="width: 25%">
                        <eluc:Date ID="txtToDate" runat="server" Width="144px" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlType" Width="48%" runat="server" CssClass="input_mandatory">
                            <Items>
                                <telerik:DropDownListItem Value="1" Text="Base Currency"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Value="2" Text="Reportting Currency"></telerik:DropDownListItem>
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account Code"></telerik:RadLabel>
                    </td>
                    <td style="width: 5%"></td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAccountSearch" CssClass="input"
                            MaxLength="10" Width="150px">
                        </telerik:RadTextBox>
                        <asp:ImageButton runat="server"
                            ImageUrl="<%$ PhoenixTheme:images/search.png %>" ID="cmdAccountSearch" OnClick="cmdSearchAccount_Click"
                            ToolTip="Search" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccount" runat="server" Text="Account"></telerik:RadLabel>
                    </td>
                    <td style="width: 9%"></td>
                    <td>
                        <telerik:RadCheckBoxList runat="server" ID="cblAccount" Height="220px" Columns="1" Style="overflow: auto;" CssClass="input" DataBindings-DataTextField="FLDaccoandept"
                            AutoPostBack="True" OnSelectedIndexChanged="AccountSelection" Direction="Vertical" Width="420px" Layout="Flow" DataBindings-DataValueField="FLDACCOUNTID">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
            </table>
            <table style="height: 23%">
                <tr>
                    <td style="width: 8%"></td>
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
