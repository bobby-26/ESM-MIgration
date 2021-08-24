<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentManagementFMSMailList.aspx.cs"
    Inherits="DocumentManagementFMSMailList" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FMS Mail</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvFMSEmail.ClientID %>"));
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
    <form id="frmSearch" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager2" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="RadAjaxPanel1" runat="server"
            DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text=""></eluc:Status>
            <eluc:TabStrip ID="MenuFMS" runat="server" OnTabStripCommand="MenuFMS_TabStripCommand" Visible="false"
                TabStrip="true"></eluc:TabStrip>
            <%--<eluc:TabStrip ID="MenuFMSmail" runat="server" OnTabStripCommand="MenuFMSmail_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>--%>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFileNumber" runat="server" Text="File Number">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <%-- <telerik:RadTextBox ID="txtFileNumber" runat="server" MaxLength="200" Width="132px" />--%>
                        <telerik:RadComboBox ID="ddlfileno" runat="server" DataTextField="FLDFILENODESCRIPTION" DataValueField="FLDFILENO"
                            EmptyMessage="Type to select file no" Filter="Contains" MarkFirstMatch="true" Width="50%">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true" SyncActiveVesselsOnly="true"
                            Width="50%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSender" runat="server" Text="Sender">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSender" runat="server" MaxLength="200" Width="80%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRecipient" runat="server" Text="Recipient">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRecipient" runat="server" MaxLength="200" Width="80%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubject" runat="server" MaxLength="200" Width="80%" />
                    </td>
                    <td colspan="2" />
                </tr>
            </table>
            <eluc:TabStrip ID="MenuFMSEmail" runat="server" OnTabStripCommand="MenuFMSEmail_TabStripCommand"></eluc:TabStrip>
            <%-- <telerik:RadGrid RenderMode="Lightweight" ID="gvFMSEmail" runat="server"
                EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvFMSEmail_NeedDataSource"
                OnItemCommand="gvFMSEmail_ItemCommand" EnableHeaderContextMenu="true" OnItemDataBound="gvFMSEmail_ItemDataBound"
                ShowFooter="False" OnSortCommand="gvFMSEmail_SortCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvFMSEmail" runat="server" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
                GridLines="None" OnNeedDataSource="gvFMSEmail_NeedDataSource" OnItemDataBound="gvFMSEmail_ItemDataBound"
                OnItemCommand="gvFMSEmail_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDMAILID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"
                        ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
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
                        <telerik:GridTemplateColumn HeaderText="Sender">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldtkey" Visible="false" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDDTKEY")) %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblMailid" Visible="false" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDMAILID")) %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblFrom" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDFROM")) %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Recipient">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTo" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDTO")) %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Subject">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSubject" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSUBJECT")) %>'
                                    CommandName="SUBJECT">
                                </asp:LinkButton>
                                <eluc:ToolTip ID="ucToolTipKeyword" runat="server" TargetControlId="lnkSubject" Text='<%# DataBinder.Eval(Container,"DataItem.FLDKEYWORD") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received On">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReceived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDON") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No's">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="10px"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Left" Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNos" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENOLIST") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox"
                        AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
