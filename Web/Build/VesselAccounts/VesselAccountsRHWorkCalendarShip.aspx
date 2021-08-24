<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsRHWorkCalendarShip.aspx.cs"
    Inherits="VesselAccountsRHWorkCalendarShip" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rest Hour Work Calender</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function pageLoad() {
                PaneResized();
                fade('statusmessage');
            }
            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvWorkCalender");
                grid._gridDataDiv.style.height = (browserHeight - 215) + "px";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmRestHourWOrkCalenderShip" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:TabStrip ID="MenuRHGeneral" runat="server" OnTabStripCommand="RHGeneral_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td width="4%">
                        <telerik:RadLabel ID="lblnote" runat="server" EnableViewState="false" Text="Notes :" Font-Bold="true" ForeColor="Blue"></telerik:RadLabel>
                    </td>
                </tr>
                <%--<tr>
                    <td>&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbl1" runat="server" Text="1."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblClickHeader" runat="server" Text="Click '"></telerik:RadLabel>
                        <i class="fa fa-plus-circle"></i>
                        '
                        <telerik:RadLabel ID="lbltoaddnextdate" runat="server" Text="to add next date."></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbl2" runat="server" Text="2."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCrewmembers"
                            runat="server" Text="Crew members will not be able to record Work Hours for any date until the Date is added into the calendar here. you may add one or more dates."></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbl3" runat="server" Text="3."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblClick" runat="server" Text="Click '"></telerik:RadLabel>
                        <i class="fas fa-calendar-alt"></i>
                        '
                            <telerik:RadLabel ID="lbltoaddonemonth" runat="server"
                                Text="to add one month. Repeat the steps to add up to 2 month in advance."></telerik:RadLabel>
                    </td>
                </tr>--%>
                <tr>
                    <%--<td>&nbsp;
                    </td>--%>
                    <td width="2%">
                        <telerik:RadLabel ID="lbl4" runat="server" Text="1." ForeColor="Blue"></telerik:RadLabel>
                    </td>
                    <td width="95%">
                        <telerik:RadLabel ID="lblToviewdates" ForeColor="Blue"
                            runat="server" Text="To view dates in a different month change the month and year in the dropdownlist."></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2" width="50%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlmonth" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Text="Jan" Value="1" />
                                <telerik:RadComboBoxItem Text="Feb" Value="2" />
                                <telerik:RadComboBoxItem Text="Mar" Value="3" />
                                <telerik:RadComboBoxItem Text="Apr" Value="4" />
                                <telerik:RadComboBoxItem Text="May" Value="5" />
                                <telerik:RadComboBoxItem Text="Jun" Value="6" />
                                <telerik:RadComboBoxItem Text="Jul" Value="7" />
                                <telerik:RadComboBoxItem Text="Aug" Value="8" />
                                <telerik:RadComboBoxItem Text="Sep" Value="9" />
                                <telerik:RadComboBoxItem Text="Oct" Value="10" />
                                <telerik:RadComboBoxItem Text="Nov" Value="11" />
                                <telerik:RadComboBoxItem Text="Dec" Value="12" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" Filter="Contains"
                            OnDataBound="ddlYear_DataBound" Sort="Descending" >
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuWorkHour" runat="server" OnTabStripCommand="MenuWorkHour_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvWorkCalender" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="78%"
                CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" EnableViewState="false"
                OnItemDataBound="gvWorkCalender_ItemDataBound" OnNeedDataSource="gvWorkCalender_NeedDataSource" OnItemCommand="gvWorkCalender_ItemCommand">
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
                        <telerik:GridTemplateColumn HeaderText="Date">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShipCalendarId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPCALENDARID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hours">
                            <HeaderStyle HorizontalAlign="Right" Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHours" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOURS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="IDL [E-W (or) W-E (or) NONE]">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIdealEW" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLOCKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Advance/Retard/Reset">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRetardAdvance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCERETARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Work At">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkAt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKPLACENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click1" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
