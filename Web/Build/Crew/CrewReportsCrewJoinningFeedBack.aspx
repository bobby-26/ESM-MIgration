<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsCrewJoinningFeedBack.aspx.cs"
    Inherits="CrewReportsCrewJoinningFeedBack" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Short Contract</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrew.ClientID %>"));
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
            <eluc:TabStrip ID="MenuCrewFeedback" runat="server" OnTabStripCommand="MenuCrewFeedback_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuReport" runat="server" OnTabStripCommand="Report_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
            <table width="100%" border="1" style="border-collapse: collapse;">
                <tr>
                    <td valign="top">
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblFromDate" runat="server" Text="Sign On From Date">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="ucFromDate" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblToDate" runat="server" Text="Sign On To Date">
                                    </telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="ucToDate" runat="server" />
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
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblFeedbackStatus" runat="server" Text="Feedback Status">
                                    </telerik:RadLabel>
                                </td>
                                <td style="text-align: left;">
                                    <telerik:RadComboBox ID="ddlFeedbackStatus" runat="server" Width="100%" EmptyMessage="Type to select Status"
                                        Filter="Contains" MarkFirstMatch="true">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="--All--" Value="DUMMY"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="Yes" Value="1"></telerik:RadComboBoxItem>
                                            <telerik:RadComboBoxItem Text="No" Value="0"></telerik:RadComboBoxItem>
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel">
                                    </telerik:RadLabel>
                                    <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                        VesselsOnly="true" Width="240px" Entitytype="VSL" AssignedVessels="true" ActiveVesselsOnly="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblvesselType" runat="server" Text="Vessel Type">
                                    </telerik:RadLabel>
                                    <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblZone" runat="Server" Text="Zone">
                                    </telerik:RadLabel>
                                    <eluc:Zone runat="server" ID="ucZone" AppendDataBoundItems="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblPool" runat="server" Text="Pool">
                                    </telerik:RadLabel>
                                    <eluc:Pool ID="lstPool" runat="server" AppendDataBoundItems="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblRank" runat="server" Text="Rank">
                                    </telerik:RadLabel>
                                    <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server"
                EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvCrew_NeedDataSource"
                OnItemCommand="gvCrew_ItemCommand" EnableHeaderContextMenu="true" OnItemDataBound="gvCrew_ItemDataBound"
                ShowFooter="False" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="S.No" AllowSorting="false" HeaderStyle-Width="45px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSrNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No" AllowSorting="false" HeaderStyle-Width="55px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                <telerik:RadLabel ID="lblSignOnOffId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>' />
                                <telerik:RadLabel ID="lblVesselId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' />
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                                <telerik:RadLabel ID="lblFeedbackYN" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKYN") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="120px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="75px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblrankCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Feedback Vessel" AllowSorting="false" HeaderStyle-Width="110px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Feedback Date" AllowSorting="false" HeaderStyle-Width="65px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDFEEDBACKDATE", "{0:dd/MMM/yyyy}"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign On Date" AllowSorting="false" HeaderStyle-Width="65px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE", "{0:dd/MMM/yyyy}"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Feedback YN" AllowSorting="false" HeaderStyle-Width="55px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFeedbackStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKYNNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="false" HeaderStyle-Width="65px"
                            ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Report" ID="cmdShowReport" ToolTip="Report"
                                    CommandName="REPORT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-file"></i></span>
                                </asp:LinkButton>

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
