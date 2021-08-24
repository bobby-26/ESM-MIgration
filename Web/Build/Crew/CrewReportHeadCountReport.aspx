<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportHeadCountReport.aspx.cs"
    Inherits="CrewReportHeadCountReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Head Count Report</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigureCity"
        runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
            TabStrip="false"></eluc:TabStrip>
        <table width="100%" border="1" style="border-collapse: collapse;">
            <tr valign="top">
                <td width="100%">
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblReportason" runat="server" Text="Report As On">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucDate" runat="server" DatePicker="true" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPrincipal" runat="server" Text="Principal">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Principal ID="ucPrinicipal" runat="server" AddressType="128" AppendDataBoundItems="true"
                                    Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblManager" runat="server" Text="Manager">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Address ID="ucManager" runat="server" AddressType="126" AppendDataBoundItems="true"
                                    Width="100%" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr valign="top">
                            <td>
                                <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type">
                                </telerik:RadLabel>
                                <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPool" runat="server" Text="Pool">
                                </telerik:RadLabel>
                                <eluc:Pool ID="lstPool" runat="server" AppendDataBoundItems="true" Width="120px" />
                            </td>
                            <td>
                                <telerik:RadLabel runat="server" Text="Zone" ID="lblZone">
                                </telerik:RadLabel>
                                <eluc:Zone ID="lstZone" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank">
                                </telerik:RadLabel>
                                <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" Width="180px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblBatch" runat="server" Text="Batch">
                                </telerik:RadLabel>
                                <eluc:Batch ID="ucBatch" runat="server" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" Height="70%"
            EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvCrew_NeedDataSource"
            EnableHeaderContextMenu="true" OnItemDataBound="gvCrew_ItemDataBound" OnItemCommand="gvCrew_ItemCommand"
            ShowFooter="False" OnSortCommand="gvCrew_SortCommand" AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDRANKID">
                <HeaderStyle Width="102px" />
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                    Font-Bold="true">
                                </telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" ShowSortIcon="true">
                        <HeaderStyle Width="20%" HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTotal" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' />
                            <telerik:RadLabel ID="lblRankName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Active" AllowSorting="false" ShowSortIcon="true">
                        <HeaderStyle Width="05%" HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRankId1" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                            <telerik:RadLabel ID="lblActive" Visible="false" runat="server" Text="1" />
                            <center>
                                <asp:LinkButton ID="lnkActive" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVE") %>' />
                                <telerik:RadLabel ID="lblTActive" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVE") %>' />
                            </center>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="InActive" AllowSorting="false" ShowSortIcon="true">
                        <HeaderStyle Width="08%" HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRankId2" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                            <telerik:RadLabel ID="lblInactive" Visible="false" runat="server" Text="2" />
                            <center>
                                <asp:LinkButton ID="lnkInActive" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINACTIVE") %>' />
                                <telerik:RadLabel ID="lblTInactive" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINACTIVE") %>' />
                            </center>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="InActive Exam" AllowSorting="false" ShowSortIcon="true">
                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRankId6" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                            <telerik:RadLabel ID="lblInactiveExm" Visible="false" runat="server" Text="6" />
                            <center>
                                <asp:LinkButton ID="lnkInActiveExm" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINACTIVEEXM") %>' />
                                <telerik:RadLabel ID="lblTInactiveExm" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINACTIVEEXM") %>' />
                            </center>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="OnBoard" AllowSorting="false" ShowSortIcon="true">
                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRankId3" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                            <telerik:RadLabel ID="lblOnBoard" Visible="false" runat="server" Text="3" />
                            <center>
                                <asp:LinkButton ID="lnkOnBoard" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONBOARD") %>' />
                                <telerik:RadLabel ID="lblTOnboard" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONBOARD") %>' />
                            </center>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="OnLeave" AllowSorting="false" ShowSortIcon="true">
                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRankId4" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                            <telerik:RadLabel ID="lblOnLeave" Visible="false" runat="server" Text="4" />
                            <center>
                                <asp:LinkButton ID="lnkOnLeave" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONLEAVE") %>' />
                                <telerik:RadLabel ID="lblTOnLeave" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONLEAVE") %>' />
                            </center>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="NTBR" AllowSorting="false" ShowSortIcon="true">
                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="50px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRankId5" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                            <telerik:RadLabel ID="lblNtbr" Visible="false" runat="server" Text="5" />
                            <center>
                                <asp:LinkButton ID="lnkNtbr" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNTBR") %>' />
                                <telerik:RadLabel ID="lblTNtbr" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNTBR") %>' />
                            </center>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Total" AllowSorting="false" ShowSortIcon="true">
                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <center>
                                <telerik:RadLabel ID="lblrTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRSUM") %>'>
                                </telerik:RadLabel>
                            </center>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
