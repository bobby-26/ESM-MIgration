<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOfficePortageBillPostingDraft.aspx.cs"
    Inherits="AccountsOfficePortageBillPostingDraft" EnableEventValidation="false" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register Src="~/UserControls/UserControlVessel.ascx" TagName="Vessel" TagPrefix="eluc" %>
<%@ Register Src="~/UserControls/UserControlStatus.ascx" TagName="Status" TagPrefix="eluc" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Voucher Draft View</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            body, html {
                width: 100%;
                height: 100%;
                margin: 0px;
                padding: 0px;
                background-color: #FFFFFF;
            }

            .modal {
                border: 0px;
                padding: 0px;
                margin: 0px;
                top: 0px;
                left: 0px;
                width: 100%;
                position: absolute;
                background-color: #000000;
                opacity: 0.5;
                filter: Alpha(opacity:50);
                z-index: 11;
                -moz-opacity: 0.8;
                min-height: 100%;
            }

            .loading {
                border: 0px;
                padding: 0px;
                display: none;
                position: absolute;
                border: 5px solid darkgrey;
                background-color: #FFFFFF;
                z-index: 12;
            }

            .RadGrid .rgHeader, .RadGrid th.rgResizeCol {
                padding: 2px !important;
                /*padding-right: 2px !important;*/
            }

            .rgview td {
                padding: 2px !important;
                /*padding-right: 2px !important;*/
            }
        </style>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="RadAjaxPanel1" DecoratedControls="All" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%" LoadingPanelID="RadAjaxLoadingPanel1">


            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuPB1" runat="server" OnTabStripCommand="MenuPB1_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuPB2" runat="server" OnTabStripCommand="MenuPB2_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr>
                    <td>
                        <asp:TextBox ID="txtvessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="280px"></asp:TextBox></td>
                    <td>
                        <asp:TextBox ID="txtTotal" runat="server" CssClass="txtNumber readonlytextbox" ReadOnly="true" Style="display: none;"
                            Width="80px"></asp:TextBox></td>
                </tr>
            </table>
            <telerik:RadGrid ID="gvHeader" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnItemDataBound="gvHeader_ItemDataBound" EnableViewState="false"
                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvHeader_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <ExportSettings IgnorePaging="true" ExportOnlyData="true">
                </ExportSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="">
                            <HeaderStyle Width="18%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDNAME"].ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Earnings">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDEARNINGS"].ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Earnings(with Budget Code)">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDEARNINGBUDGET"].ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Deductions">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDDEDUCTION"].ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Deductions(with Budget Code)">
                            <HeaderStyle Width="17%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDDEDUCTIONBUDGET"].ToString()%>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Balance Forward">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDBF"].ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Final Balance">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDFINALBALANCE"].ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound1" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:TabStrip ID="MenuPBExcel" runat="server" OnTabStripCommand="MenuPBExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvPB" runat="server" AutoGenerateColumns="false" AllowSorting="false" RegisterWithScriptManager="false"
                CellSpacing="0" GridLines="None" OnItemDataBound="gvPB_ItemDataBound" EnableViewState="false"
                ShowFooter="true" ShowHeader="true" OnNeedDataSource="gvPB_NeedDataSource">
                <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-Width="100%"
                    GroupHeaderItemStyle-Wrap="false" Width="100%" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <CommandItemStyle Height="20px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Earnings" Name="Earnings" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Deductions" Name="Deductions" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Balance Forward" Name="BF" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Final Balance" Name="FB" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" Name="Empty">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <%--<telerik:GridTemplateColumn HeaderText="File No.">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDFILENO"].ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Employee">
                            <HeaderStyle Width="240px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"].ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDRANKCODE"].ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>                                
                                <%# string.Format("{0:dd/MM/yyyy}",((DataRowView)Container.DataItem)["FLDFROMDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>                                
                                <%# string.Format("{0:dd/MM/yyyy}",((DataRowView)Container.DataItem)["FLDTODATE"])%>
                            </ItemTemplate>
                            <FooterStyle Wrap="false" Font-Bold="true" />
                            <FooterTemplate>
                                <asp:Literal ID="lblTotal" runat="server" Text="Total"></asp:Literal>
                            </FooterTemplate>

                        </telerik:GridTemplateColumn>--%>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound2" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="5" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <b>
                <asp:Literal ID="lblOnboardAccruals" runat="server" Text="Onboard Accruals"></asp:Literal></b>

            <eluc:TabStrip ID="MenuLWPBExcel" runat="server" OnTabStripCommand="MenuLWPBExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvLWPB" runat="server" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" RegisterWithScriptManager="false"
                CellSpacing="0" GridLines="None" OnItemDataBound="gvLWPB_ItemDataBound" EnableViewState="false"
                ShowFooter="true" ShowHeader="true" OnNeedDataSource="gvLWPB_NeedDataSource">
                <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-Width="100%"
                    GroupHeaderItemStyle-Wrap="false" Width="100%" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <CommandItemStyle Height="20px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Earnings" Name="Earnings" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Deductions" Name="Deductions" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Balance Forward" Name="BF" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Final Balance" Name="FB" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" Name="Empty">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <%--<telerik:GridTemplateColumn HeaderText="File No.">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>                              
                                <%# ((DataRowView)Container.DataItem)["FLDFILENO"].ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Employee">
                            <HeaderStyle Width="240px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"].ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDRANKCODE"].ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MM/yyyy}",((DataRowView)Container.DataItem)["FLDFROMDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MM/yyyy}",((DataRowView)Container.DataItem)["FLDTODATE"])%>
                            </ItemTemplate>
                            <FooterStyle Wrap="false" Font-Bold="true" />
                            <FooterTemplate>
                                <asp:Literal ID="lblTotal" runat="server" Text="Total"></asp:Literal>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>--%>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound3" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="5" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <b>
                <asp:Literal ID="lblOfficeAccruals" runat="server" Text="Office Accruals"></asp:Literal></b>

            <eluc:TabStrip ID="MenuSLExcel" runat="server" OnTabStripCommand="MenuSLExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvSL" runat="server" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" RegisterWithScriptManager="false"
                CellSpacing="0" GridLines="None" OnItemDataBound="gvSL_ItemDataBound" EnableViewState="false"
                ShowFooter="true" ShowHeader="true" OnNeedDataSource="gvSL_NeedDataSource">
                <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-Width="100%"
                    GroupHeaderItemStyle-Wrap="false" Width="100%" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <CommandItemStyle Height="20px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Earnings" Name="Earnings" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Deductions" Name="Deductions" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Balance Forward" Name="BF" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Final Balance" Name="FB" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="" Name="Empty">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <%--<telerik:GridTemplateColumn HeaderText="File No.">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDFILENO"].ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Employee">
                            <HeaderStyle Width="240px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"].ToString() %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDRANKCODE"].ToString()%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MM/yyyy}",((DataRowView)Container.DataItem)["FLDFROMDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MM/yyyy}",((DataRowView)Container.DataItem)["FLDTODATE"])%>
                            </ItemTemplate>
                            <FooterStyle Wrap="false" Font-Bold="true" />
                            <FooterTemplate>
                                <asp:Literal ID="lblTotal" runat="server" Text="Total"></asp:Literal>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>--%>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound4" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="5" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
