<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListRHShipWork.aspx.cs"
    Inherits="CommonPickListRHShipWork" %>

<!DOCTYPE html>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ship Calendar</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <br clear="all" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselname" runat="server" CssClass="input" Enabled="false" Width="250px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlmonth" runat="server" CssClass="dropdown_mandatory" OnSelectedIndexChanged="ddlmonth_OnSelectedIndexChanged"
                            AutoPostBack="true" Filter="Contains">
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
                        <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="dropdown_mandatory" OnSelectedIndexChanged="ddlmonth_OnSelectedIndexChanged" OnDataBound="ddlYear_DataBound"
                              Sort="Descending" AutoPostBack="true" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <br />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td colspan="2" align="top">
                        <telerik:RadGrid ID="gvShipCalendar" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false"
                            Width="80%" CellPadding="3" OnItemCommand="gvShipCalendar_OnItemCommand"
                            ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvShipCalendar_NeedDataSource">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDSHIPCALENDARID">
                                <HeaderStyle Width="102px" />
                                <NoRecordsTemplate>
                                    <table runat="server" width="50%" border="0">
                                        <tr>
                                            <td align="top">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Date">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCalenderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPCALENDARID") %>'></telerik:RadLabel>
                                            <asp:LinkButton ID="lnkDate" runat="server" CommandName="EDIT"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE","{0:dd/MMM/yyyy}") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                            </MasterTableView>
                            <%--<ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>--%>
                        </telerik:RadGrid>
                    </td>
                    <td colspan="2">
                        <telerik:RadGrid ID="gvShipWork" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="90%" CellPadding="3" OnItemCommand="gvShipWork_OnItemCommand" ShowHeader="true"
                            EnableViewState="false" OnNeedDataSource="gvShipWork_NeedDataSource">
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
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblShipcalendarid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPCALENDARID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Reporting Hour">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkReportingHour" runat="server" CommandName="PICK"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTINGHOUR") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
