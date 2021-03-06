<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersSOAWithSupportings.aspx.cs" Inherits="OwnersSOAWithSupportings" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselAccount" Src="~/UserControls/UserControlUserVesselAccount.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Statement of Accounts</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmOwnersAccounts" DecoratedControls="All" />
    <form id="frmOwnersAccounts" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <table width="100%" cellpadding="1" cellspacing="1" style="z-index: 2;">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel Account"></telerik:RadLabel>
                </td>
                <td>
                    <%-- <telerik:RadDropDownList ID="ddlVesselAccount" runat="server" Width="240px"
                            AppendDataBoundItems="true">
                        </telerik:RadDropDownList>--%>
                    <eluc:VesselAccount ID="ucVesselAccount" runat="server" AddressType="128" CssClass="input" AutoPostBack="true"
                        AppendDataBoundItems="true" Width="240px" />
                </td>

                <td>
                    <telerik:RadLabel ID="lblyear" runat="server" Text="Year"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Year ID="ucYear" runat="server" AppendDataBoundItems="true" QuickTypeCode="55" Width="120px" />
                </td>

                <td>
                    <telerik:RadLabel ID="lblmonth" runat="server" Text="Month"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Month ID="ucMonth" runat="server" AppendDataBoundItems="true" HardTypeCode="55" SortByShortName="true" Width="120px" />
                </td>
            </tr>

        </table>
        <eluc:TabStrip ID="MenuOwnersSOAWithSupportings" runat="server" OnTabStripCommand="OwnersSOAWithSupportings_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvOwnersAccount" Height="85%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvOwnersAccount_ItemCommand" OnItemDataBound="gvOwnersAccount_ItemDataBound" EnableViewState="false"
            ShowFooter="true" ShowHeader="true" OnNeedDataSource="gvOwnersAccount_NeedDataSource">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Account Name">
                        <HeaderStyle Width="20%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOwnerId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAccountName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="SOA Reference">
                        <HeaderStyle Width="15%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAccountCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblSoaReference" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblSoaReferenceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBINOTEREFERENCEID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkSoaReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'
                                CommandName="Select"></asp:LinkButton>
                            <asp:LinkButton ID="lnkVariance" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'
                                CommandName="SelectVariance"></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Month">
                        <HeaderStyle Width="12%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMonthId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTH") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Year">
                        <HeaderStyle Width="12%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblYearId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <a id="aEPSSHelp" href='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' style="color: blue;" target="_blank" fore-color="" runat="server">SOA</a>
                            <%--<asp:LinkButton runat="server" AlternateText="SOA" Text="SOA" PostBackUrl='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>'
                                    CommandName="SOA" ID="aEPSSHelp" ToolTip="SOA">                                     <span class="icon"><i class="fa fa-file-contract"></i></span>
                                </asp:LinkButton>--%>
                            <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />

                            <asp:LinkButton runat="server" AlternateText="Vessel Trial Balance" Text="TB"
                                CommandName="TBPDF" ID="cmdTBPdf" ToolTip="Vessel Trial Balance" Visible="false">
                                     <span class="icon"><i class="fa fa-money-check-alt"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Vessel Trial Balance" Text="TB"
                                CommandName="TBYTDPDF" ID="cmdTBYTD"
                                ToolTip="Vessel Trial Balance" Visible="false">
                                     <span class="icon"><i class="fa fa-balance-scale"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Vessel Trial Balance" Text="TB"
                                CommandName="TBYTDOWNPDF" ID="cmdTBYTDOwner"
                                ToolTip="Vessel Trial Balance" Visible="false">
                                    <span class="icon"><i class="fa fa-balance-scale"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Vessel Summary Balance" Text="SUMMARY"
                                CommandName="summaryDF" ID="cmdSummaryPdf" ToolTip="Vessel Summary Balance" Visible="false">
                                     <span class="icon"><i class="fa fa-file-alt"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Excel" CommandName="EXCEL" ID="cmdExcel" ToolTip="Excel">
                                    <span class="icon"><i class="fa fa-file-excel"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAtt" ToolTip="Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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
