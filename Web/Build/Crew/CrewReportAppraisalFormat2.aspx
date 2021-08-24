<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportAppraisalFormat2.aspx.cs"
    Inherits="CrewReportAppraisalFormat2" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PoolList" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Appraisals received</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvAQ.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAppraisalQuestion" runat="server">
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

            <eluc:TabStrip ID="MenuReport" runat="server" OnTabStripCommand="MenuReport_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>

            <table border="1" style="border-collapse: collapse;">
                <tr>
                    <td colspan="2" style="color: Blue; font-weight: bold;">
                        <telerik:RadLabel ID="lblNote" runat="server" CssClass="guideline_text">
                            <font color="blue" size="1px">Note: Period filter checks 'Appraisal Date'</font>
                        </telerik:RadLabel>
                    </td>

                </tr>
                <tr>
                    <td valign="top">
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblPeriodFrom" runat="server" Text="Period From"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <eluc:Date ID="txtFromDate" runat="server" CssClass="input_mandatory" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblPeriodTo" runat="server" Text="Period To"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>

                                    <eluc:PoolList ID="ucPool" runat="server" AppendDataBoundItems="true" />

                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblVesseltype" runat="server" Text="Vessel type"></telerik:RadLabel>

                                    <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" />

                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblBatch" runat="server" Text="Batch"></telerik:RadLabel>

                                    <eluc:Batch ID="ucBatch" runat="server" AppendDataBoundItems="true" />

                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>

                                    <eluc:Rank ID="lstRank" runat="server" AppendDataBoundItems="true" />

                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblManager" runat="server" Text="Manager"></telerik:RadLabel>

                                    <eluc:Address ID="ucManager" runat="server" AddressType="126" AppendDataBoundItems="true" />

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuCrewAppraisal" runat="server" OnTabStripCommand="MenuCrewAppraisal_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAQ" runat="server"  EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" OnNeedDataSource="gvAQ_NeedDataSource" OnItemCommand="gvAQ_ItemCommand"
                GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true" OnSortCommand="gvAQ_SortCommand"
                OnItemDataBound="gvAQ_ItemDataBound" ShowFooter="False"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDCREWAPPRAISALID">
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
                        <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" HeaderStyle-Width="35px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDROWNUMBER")%>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No" AllowSorting="false" HeaderStyle-Width="40px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDFILENO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="120px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDEMPLOYEENAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="65px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" AllowSorting="false" HeaderStyle-Width="120px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDCATEGORY")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Question" AllowSorting="false" HeaderStyle-Width="220px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDQUESTION")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rating" AllowSorting="false" HeaderStyle-Width="45px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDRATING")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                        EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
