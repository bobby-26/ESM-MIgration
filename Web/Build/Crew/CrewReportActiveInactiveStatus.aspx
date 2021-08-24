<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportActiveInactiveStatus.aspx.cs"
    Inherits="Crew_CrewReportActiveInactiveStatus" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manager" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BatchList" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCNTBRReason" Src="~/UserControls/UserControlNTBRReasonList.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Seafarers Active In-Active Status</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
        <table border="1" style="border-collapse: collapse; font-size= smaller">
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblActiveInactiveBetween" Text="Active/Inactive Between" runat="server">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucDateFrom" runat="server" CssClass="input_mandatory" />
                                <eluc:Date ID="ucDateTo" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblFileno" Text="File No" runat="server">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server"  ID="txtFileNo"></telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPromotionCategory" Text="Promotion Category" runat="server">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlCategory" runat="server"  EmptyMessage="Type to select Category" Filter="Contains" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem Value="" Text="Select"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="1" Text="Category1"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="2" Text="Category2"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="3" Text="Category3"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="4" Text="Category4"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td colspan="2">
                                <telerik:RadRadioButtonList runat="server" ID="rbtnActive" OnSelectedIndexChanged="rbtnActive_SelectedIndexChanged" Layout="Flow" Columns="4"
                                    AutoPostBack="true" RepeatDirection="Horizontal">
                                    <Items>
                                    <telerik:ButtonListItem Value="1" Text="Active"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Value="2" Text="In Active"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Value="3" Text="Ntbr"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Value="4" Text="De-Ntbr"></telerik:ButtonListItem>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr valign="top">
                            <td colspan="2">
                                <telerik:RadLabel ID="lblVesselType" Text="Vessel Type" runat="server">
                                </telerik:RadLabel>
                                <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true"  />
                            </td>
                            <td>
                                <td>
                                    <telerik:RadLabel ID="lblInActiveReasons" runat="server" Text="In Active Reasons"
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblNtbrReasons" runat="server" Text="Ntbr Reasons" Visible="false">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadListBox ID="lstInActiveReasons" runat="server" SelectionMode="Multiple" Visible="false"
                                        DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE"></telerik:RadListBox>
                                    <eluc:UCNTBRReason ID="ucNTBRReason" runat="server" AppendDataBoundItems="true" Visible="false"
                                         />
                                </td>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblBatch" Text="Batch" runat="server">
                                </telerik:RadLabel>
                                <eluc:BatchList ID="ucBatchList" AppendDataBoundItems="true" runat="server"  />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPool" Text="Pool" runat="server">
                                </telerik:RadLabel>
                                <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true"  />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblZone" Text="Zone" runat="server">
                                </telerik:RadLabel>
                                <eluc:Zone ID="ucZone" runat="server"  AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="Server" Text="Rank">
                                </telerik:RadLabel>
                                <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true"  />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPrincipal" Text="Principal" runat="server">
                                </telerik:RadLabel>
                                <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" 
                                    AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" Height="62%"
            EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvCrew_NeedDataSource"
            EnableHeaderContextMenu="true" OnItemDataBound="gvCrew_ItemDataBound" OnItemCommand="gvCrew_ItemCommand"
            ShowFooter="False" AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDEMPLOYEEID" CommandItemDisplay="Top">
                <HeaderStyle Width="102px" />
                <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" HeaderStyle-Width="50px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSlNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="File No" AllowSorting="false" HeaderStyle-Width="66px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmpCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="150px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                            <telerik:RadLabel ID="lblReasonID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASONID") %>' />
                            <asp:LinkButton ID="lnkName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'
                                Enabled='<%# bool.Parse(Eval("FLDREASONID").ToString() == "933" ? "False":"True") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Present Rank" AllowSorting="false" HeaderStyle-Width="70px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKPOSTEDNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Sailed Rank" AllowSorting="false" HeaderStyle-Width="70px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDSAILEDRANK") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Batch" AllowSorting="false" HeaderStyle-Width="90px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblBatchHeader" runat="server">
                                Batch</telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHNO") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Sign-Off Date" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTSIGNOFFDATE"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Vessel" AllowSorting="false" HeaderStyle-Width="140px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDLASTVESSELNAME") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Sign-On  Date" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Present Vessel" AllowSorting="false" HeaderStyle-Width="140px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDPRESENTVESSEL") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="No. of Tenures" AllowSorting="false" HeaderStyle-Width="60px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNoofTenures" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTTENURE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Total Service" AllowSorting="false" HeaderStyle-Width="65px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTTlService" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTTLSERVICE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
             
                    <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" HeaderStyle-Width="70px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Inactive/NTBR Reason" AllowSorting="false"
                        HeaderStyle-Width="100px" ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNTBRReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNTBRREASON") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Join Date" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDOJ" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFJOINING"))%>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Active/Inactive/NTBR Date" AllowSorting="false"
                        HeaderStyle-Width="135px" ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNTBRDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="User" AllowSorting="false" HeaderStyle-Width="150px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblUser" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSER") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
