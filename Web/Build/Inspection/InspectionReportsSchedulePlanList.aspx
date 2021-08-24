<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionReportsSchedulePlanList.aspx.cs"
    Inherits="InspectionReportsSchedulePlanList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Schedule Plan</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvSchedulePlan").height(browserHeight - 40);
            });

        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSchedulePlan" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RadAjaxPanel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Audit / Inspection" ShowMenu="true" Visible="false"></eluc:Title>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselByCompany runat="server" ID="ucVessel" AppendDataBoundItems="true" VesselsOnly="true" Width="220px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAuditInspectionCategory" runat="server" Text="Audit / Inspection Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucAuditCategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            Width="220px" HardTypeCode="144" OnTextChangedEvent="Bind_UserControls" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAuditInspection" runat="server" Text="Audit / Inspection"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAudit" runat="server" Width="220px"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                        <eluc:Inspection runat="server" ID="ucAudit" Visible="false" AppendDataBoundItems="true" Width="180px" />
                        <eluc:Hard ID="ucAuditType" runat="server" Visible="false" ShortNameFilter="AUD"
                            AutoPostBack="true" HardTypeCode="148" OnTextChangedEvent="Bind_UserControls" />
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblDueOverdue" runat="server" Width="220px" Direction="Horizontal">
                            <Items>
                                <%--<telerik:RadComboBoxItem Text="Completed" Value="1"></telerik:RadComboBoxItem>--%>
                                <telerik:ButtonListItem Text="Due" Value="1"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="Over Due" Value="2"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlStatus" runat="server" Width="220px"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Not Planned" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Planned" Value="2"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Completed" Value="3"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lblRCANotcompletedYN" runat="server" Text="RCA Not Completed Y/N"></telerik:RadLabel>
                        <asp:CheckBox ID="chkRCANotcompletedYN" runat="server" />
                        &nbsp;&nbsp;
                            <telerik:RadLabel ID="lblCARNotCompletedYN" runat="server" Text="CAR Not Completed Y/N"></telerik:RadLabel>
                        <asp:CheckBox ID="chkCARNotCompletedYN" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPastDateRange" runat="server" Text="Last"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlPastDateRange" runat="server" Width="220px"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="1 Week" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="2 Weeks" Value="2" Selected="True"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="1 Month" Value="3"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="2 Months" Value="4"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="3 Months" Value="5"></telerik:RadComboBoxItem>
                                <%--<telerik:RadComboBoxItem Text="6 Months" Value="6"></telerik:RadComboBoxItem>--%>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFromPortName" runat="server" Text="From Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SeaPort ID="ucFromPort" runat="server" AppendDataBoundItems="true" Width="220px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFutureDateRange" runat="server" Text="Next"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlFutureDateRange" runat="server" Width="220px"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="1 Week" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="2 Weeks" Value="2"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="1 Month" Value="3"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="2 Months" Value="4"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="3 Months" Value="5" Selected="True"></telerik:RadComboBoxItem>
                                <%--<telerik:RadComboBoxItem Text="6 Months" Value="6"></telerik:RadComboBoxItem>--%>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToPortName" runat="server" Text="To Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SeaPort ID="ucToPort" runat="server" AppendDataBoundItems="true" Width="220px" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td></td>
                    <td>&nbsp;
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuSchedulePlan" runat="server" OnTabStripCommand="MenuSchedulePlan_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSchedulePlan" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" Height="65.5%" CellPadding="3" OnItemCommand="gvSchedulePlan_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvSchedulePlan_NeedDataSource" OnItemDataBound="gvSchedulePlan_ItemDataBound"
                EnableViewState="false" AllowSorting="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup Name="DeficiencyCount" HeaderText="Def Count" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <NoRecordsTemplate>
                        <table width="99.9%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDVESSELNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblReviewScheduleId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWSCHEDULEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'
                                    Width="95%">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ref.Number" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDREFERENCENO">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReferenceNumber" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREFERENCENO").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucReferenceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Audit/Inspection" HeaderStyle-Width="110px" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDINSPECTIONNAME">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="16%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuditInspection" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDINSPECTIONNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucAuditInspection" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Done" HeaderStyle-Width="74PX" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDLASTDONEDATE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastDoneDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>'
                                    Width="95%">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due" HeaderStyle-Width="74PX" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDDUEDATE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'
                                    Width="95%">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Planned" HeaderStyle-Width="74PX" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDRANGEFROMDATE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlannedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDRANGEFROMDATE")) %>'
                                    Width="95%">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed" HeaderStyle-Width="78PX" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPLETIONDATE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'
                                    Width="95%">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From" ColumnGroupName="FromToPort">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPortFrom" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPORTFROM").ToString().Length > 15 ? DataBinder.Eval(Container, "DataItem.FLDPORTFROM").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDPORTFROM").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucPortFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTFROM") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To" ColumnGroupName="FromToPort">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPortTo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPORTTO").ToString().Length > 15 ? DataBinder.Eval(Container, "DataItem.FLDPORTTO").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDPORTTO").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucPortTo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTTO") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Auditor">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuditor" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAUDITOR").ToString().Length > 15 ? DataBinder.Eval(Container, "DataItem.FLDAUDITOR").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDAUDITOR").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucAuditor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAUDITOR") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Attending Supdt.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAttendingSupdt" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDATTENDINGSUPDT").ToString().Length > 15 ? DataBinder.Eval(Container, "DataItem.FLDATTENDINGSUPDT").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDATTENDINGSUPDT").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucAttendingSupdt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTENDINGSUPDT") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total" HeaderStyle-Width="50px" ColumnGroupName="DeficiencyCount" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDDEFICIENCYCOUNT">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="2%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDeficiencyCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="MNC" HeaderStyle-Width="45px" ColumnGroupName="DeficiencyCount">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="2%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMajorNCCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAJORNCCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="NC" HeaderStyle-Width="40px" ColumnGroupName="DeficiencyCount">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="2%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNCCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNCCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="OBS" ColumnGroupName="DeficiencyCount" HeaderStyle-Width="40px">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="2%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOBSCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOBSCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="85px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="55px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Summary" CommandName="DEFICIENCYSUMMARY"
                                    ID="cmdDeficiencySummary" ToolTip="View Deficiency Summary">
                                     <span class="icon"><i class="fas fa-eye"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
