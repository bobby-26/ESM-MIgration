<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportSeafarersOnBoardOnLeave.aspx.cs"
    Inherits="CrewReportSeafarersOnBoardOnLeave" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manager" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EmpFleet" Src="~/UserControls/UserControlFleetList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="FleetList" Src="~/UserControls/UserControlFleetList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Seafarers OnBoard OnLeave Report</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
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
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="ReportNotRelievedOnTime"
        runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
            TabStrip="false"></eluc:TabStrip>
        <table width="100%">
            <tr style="color: Blue">
                &nbsp;&nbsp; <span id="Span3" class="icon" style="align-self: center" runat="server">
                    <font color="blue" size="2px">Note : To view the Guidelines, place the mouse pointer over the&nbsp;&nbsp;<i
                        class="fas fa-question-circle"></i></span>&nbsp;&nbsp;button.</font>
                <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="GuidlinesTooltip"
                    Width="300px" ShowEvent="onmouseover" RelativeTo="Element" Animation="Fade" TargetControlID="Span3"
                    IsClientID="true" HideEvent="LeaveToolTip" Position="MiddleRight" EnableRoundedCorners="true"
                    HideDelay="5000" Text="Please note: <br/> 1) Status column shows the present status of the seafarer <br/> 2) Last vessel would be the last vessel seafarer sailed if onleave as of today. <br/> 3) Present vessel would be the vessel seafarer is on as of today.">
                </telerik:RadToolTip>
            </tr>
        </table>
        <table border="1" style="border-collapse: collapse;">
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td valign="top">
                                As on Date
                                <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td valign="top">
                                Principal
                                <eluc:Principal ID="ucPrinicipal" runat="server" AddressType="128" AppendDataBoundItems="true" />
                            </td>
                            <td valign="top">
                                Refferred By
                                <eluc:Quick ID="ddlrefferredby" runat="server" QuickTypeCode="127" AppendDataBoundItems="true" />
                            </td>
                            <td valign="top">
                                Status
                                <telerik:RadComboBox runat="server" ID="rbtnOnBoardOnLeave" EmptyMessage="Type ToolTip Select Status"
                                    Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="" Selected="True" Text="All"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="0" Text="On Board"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Value="1" Text="On Leave"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadCheckBox ID="chkIncludeInactive" runat="server" Text=" Include InActive Seafarers" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVesselType" runat="Server" Text="Vessel Type">
                                </telerik:RadLabel>
                                <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel">
                                </telerik:RadLabel>
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true"
                                    Width="240px" EntityType="VSL" ActiveVesselsOnly="true" AssignedVessels="true"/>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPool" runat="server" Text="Pool">
                                </telerik:RadLabel>
                                <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblZone" runat="server" Text="Zone">
                                </telerik:RadLabel>
                                <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank">
                                </telerik:RadLabel>
                                <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality">
                                </telerik:RadLabel>
                                <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblEmpFleet" runat="server" Text=" Emp Fleet">
                                </telerik:RadLabel>
                                <eluc:FleetList ID="ucFleet" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblManager" runat="server" Text="Manager">
                                </telerik:RadLabel>
                                <eluc:Manager ID="ucManager" AddressType="126" runat="server" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server"
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
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                    Font-Bold="true">
                                </telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" HeaderStyle-Width="50px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblHeaderSlNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="File No" AllowSorting="false" HeaderStyle-Width="65px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmpNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="175px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmpId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                            <asp:LinkButton ID="lnkName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="75px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Batch" AllowSorting="false" HeaderStyle-Width="75px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDBATCH")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Nationality" AllowSorting="false" HeaderStyle-Width="95px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Civil Status" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDCIVILSTATUS") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Present Status" AllowSorting="false" HeaderStyle-Width="110px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Present Vessel" AllowSorting="false" HeaderStyle-Width="135px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDPRESENTVESSEL")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="From Date" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDPRESENTVESSELSIGNONDATE"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="To Date" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDPRESENTVESSELSIGNOFFDATE"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Due Off" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDPRESENTVESSELDUEOFF"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel Type" AllowSorting="false" HeaderStyle-Width="200px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDPRESENTVESSELTYPE")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Days OnBoard" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDPRESENTVESSELDURATION")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Vessel" AllowSorting="false" HeaderStyle-Width="135px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDLASTVESSEL")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="From Date" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTVESSELSIGNONDATE"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="To Date" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTVESSELSIGNOFFDATE"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Due Off" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTVESSELDUEOFF"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Days OnBoard" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDLASTVESSELDURATION")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel Type" AllowSorting="false" HeaderStyle-Width="200px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDLASTVESSELTYPE")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Joined Date" AllowSorting="false" HeaderStyle-Width="80px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFJOINING"))%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                       <telerik:GridTemplateColumn HeaderText="Email" AllowSorting="false" HeaderStyle-Width="200px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDEMAIL")%>
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
                    EnableNextPrevFrozenColumns="true"  />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
