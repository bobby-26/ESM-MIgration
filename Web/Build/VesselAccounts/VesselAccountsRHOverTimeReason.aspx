<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsRHOverTimeReason.aspx.cs"
    Inherits="VesselAccountsRHOverTimeReason" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="RHCrew" Src="~/UserControls/UserControlRestHourEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rest Hours OT Reason</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script>
            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvOTReason");
                grid._gridDataDiv.style.height = (browserHeight - 140) + "px";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmRegistersOTReason" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblConfigureCity" width="50%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReportfortheMonthof" runat="server" Text="Show records for the Month of :"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="input_mandatory" AutoPostBack="true" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Text="January" Value="1" />
                                <telerik:RadComboBoxItem Text="February" Value="2" />
                                <telerik:RadComboBoxItem Text="March" Value="3" />
                                <telerik:RadComboBoxItem Text="April" Value="4" />
                                <telerik:RadComboBoxItem Text="May" Value="5" />
                                <telerik:RadComboBoxItem Text="June" Value="6" />
                                <telerik:RadComboBoxItem Text="July" Value="7" />
                                <telerik:RadComboBoxItem Text="August" Value="8" />
                                <telerik:RadComboBoxItem Text="September" Value="9" />
                                <telerik:RadComboBoxItem Text="October" Value="10" />
                                <telerik:RadComboBoxItem Text="November" Value="11" />
                                <telerik:RadComboBoxItem Text="December" Value="12" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year :"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input_mandatory" AutoPostBack="true" Filter="Contains"
                            OnDataBound="ddlYear_DataBound" Sort="Descending">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRHOTReason" runat="server" OnTabStripCommand="MenuRHOTReason_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvOTReason" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None" Width="100%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" EnableHeaderContextMenu="true" GroupingEnabled="false" 
                OnItemCommand="gvOTReason_ItemCommand" OnItemDataBound="gvOTReason_ItemDataBound" OnSortCommand="gvOTReason_SortCommand"
                ShowFooter="false" ShowHeader="true" OnUpdateCommand="gvOTReason_UpdateCommand" OnNeedDataSource="gvOTReason_NeedDataSource"
                EnableViewState="false">
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
                        <telerik:GridTemplateColumn HeaderText="Employee Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Left" Width="15%"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRestHourOTreasonid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESTHOUROTREASONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRankGroupid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUP") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRestHourStartid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESTHOURSTARTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRestHourEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESTHOUREMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeName" Width="200px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" Width="150px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total OT hours">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOTHours" Width="50px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALOTHOURS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="OT Reason">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOTReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERTIMEREASON") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlOTReasonEdit" runat="server" AppendDataBoundItems="true"
                                    CssClass="input" Filter="Contains" Width="100%">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
