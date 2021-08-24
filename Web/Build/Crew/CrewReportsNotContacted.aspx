<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsNotContacted.aspx.cs"
    Inherits="CrewReportsNotContacted" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="RankList" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ZoneList" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register Src="../UserControls/UserControlVesselTypeList.ascx" TagName="UserControlVesselTypeList"
    TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Not Contacted</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewNotContacted" runat="server">
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
        <table>
            <tr style="color: Blue">
            &nbsp;&nbsp;
                    <span id="Span3" class="icon" style="align-self: center" runat="server"><font color="blue" size="2px">Note : To view the Guidelines, place the mouse pointer over the&nbsp;&nbsp;<i class="fas fa-question-circle"></i></span>&nbsp;&nbsp;button.</font>
                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="GuidlinesTooltip" Width="300px" ShowEvent="onmouseover" 
                        RelativeTo="Element" Animation="Fade" TargetControlID="Span3" IsClientID="true"
                        HideEvent="LeaveToolTip" Position="MiddleRight" EnableRoundedCorners="true" HideDelay="5000"
                        Text="Please note: <br/> 1) Either Last contact Prior or Last contact After is mandatory. <br/> 2) Press Control for multiple selection. <br/> 3) If 'Include past experience' checked, the filter will search for vessel type experiece in full past experience,if not will check only last/onboard vessels">
                    </telerik:RadToolTip>
           
            </tr>
        </table>
        <table width="100%" border="1" style="border-collapse: collapse;">
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblLastContPrior" runat="Server" Text="Last Contact Prior">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucLastContactedPrior" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblLastContAfter" runat="Server" Text="Last Contact After">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucLastContactedAfter" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDOA" runat="Server" Text="DOA Between">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="ucDOAFrom" runat="server" />
                                <eluc:Date ID="ucDOATo" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblStatus" runat="Server" Text="Status">
                                </telerik:RadLabel>
                            </td>
                            <td>
           
                                <telerik:RadComboBox ID="ddlStatus" runat="server" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE"
                                    Filter="Contains"  MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPrincipal" runat="Server" Text="Principal">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Principal ID="ucPrinicipal" runat="server" AddressType="128" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblShowSefarerFrom" runat="Server" Text="Show seafarers from"
                                    Visible="false">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadRadioButtonList ID="rblCrewFrom" runat="server" RepeatDirection="Horizontal"
                                    Visible="false">
                                    <Items>
                                        <telerik:ButtonListItem Text="New Applicants" Value="1"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Text="Personnel Master" Value="2"></telerik:ButtonListItem>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr valign="top">
                            <td colspan="2">
                                <telerik:RadLabel ID="lblVesselType" runat="Server" Text="Vessel Type">
                                </telerik:RadLabel>
                                <eluc:UserControlVesselTypeList ID="ddlVesselType" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="Server" Text="Rank">
                                </telerik:RadLabel>
                                <eluc:RankList ID="ucRank" runat="server" AppendDataBoundItems="true" RankList='<%# PhoenixRegistersRank.ListRank() %>'
                                    AutoPostBack="false" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPool" runat="Server" Text="Pool">
                                </telerik:RadLabel>
                                <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblZone" runat="Server" Text="Zone">
                                </telerik:RadLabel>
                                <eluc:ZoneList ID="ucZone" AppendDataBoundItems="true" runat="server" ZoneList='<%# PhoenixRegistersMiscellaneousZoneMaster.ListMiscellaneousZoneMaster() %>' />
                            </td>
                        </tr>
                        <td>
                            <asp:CheckBox ID="chkIncludepastexp" runat="server" />
                            <font color="blue">(Include past experience)</font>
                        </td>
                    </table>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" Height="55%"
            EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnNeedDataSource="gvCrew_NeedDataSource" OnItemCommand="gvCrew_ItemCommand" OnItemDataBound="gvCrew_ItemDataBound"
            ShowFooter="False" OnSortCommand="gvCrew_SortCommand" AutoGenerateColumns="false">
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
                    <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" HeaderStyle-Width="55px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblserialno" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="File No" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmpCode" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>' />
                            <telerik:RadLabel ID="lblEmpFileNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="200px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                            <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="65px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRankCode" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Zone" AllowSorting="false" HeaderStyle-Width="70px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblZone" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Batch" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBatch" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="OnBoard Vessel" AllowSorting="false" HeaderStyle-Width="155px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVslOnBoard" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONBOARDVESSELNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel SignOn Date" AllowSorting="false"
                        HeaderStyle-Width="90px" ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSignOnDate" Visible="true" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDONBOARDSIGNONDATE","{0:dd/MMM/yyyy}")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last SignOff Vessel" AllowSorting="false"
                        HeaderStyle-Width="155px" ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLastSignOffVessel" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTSIGNOFFVESSELNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Vessel SignOff Date" AllowSorting="false"
                        HeaderStyle-Width="90px" ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLastSignoffdate" Visible="true" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTSIGNOFFDATE","{0:dd/MMM/yyyy}")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Onboard Vessel Type" AllowSorting="false"
                        HeaderStyle-Width="100px" ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOnboardVesselType" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTIONONBOARD") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last SignOff Vessel Type" AllowSorting="false"
                        HeaderStyle-Width="100px" ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSignOffVesselType" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPEDESCRIPTIONLASTSIGNOFFVESSEL") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="DOA" AllowSorting="false" HeaderStyle-Width="90px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDOA" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOA") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Contact Date" AllowSorting="false" HeaderStyle-Width="90px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLastContactDate" Visible="true" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTCONTACTDATE","{0:dd/MMM/yyyy}")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="415px"/>
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
