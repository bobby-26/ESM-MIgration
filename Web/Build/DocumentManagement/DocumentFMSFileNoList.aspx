<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentFMSFileNoList.aspx.cs"
    Inherits="DocumentFMSFileNoList" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PageNumber" Src="~/UserControls/UserControlPageNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>File Number</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvFMSFileNo.ClientID %>"));
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
    <form id="frmRegistersFMSFileNo" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuFMS" runat="server" OnTabStripCommand="MenuFMS_TabStripCommand" Visible="false"
                TabStrip="true"></eluc:TabStrip>
            <table id="tblfilenosearch" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblfileno" runat="server" Text="File No">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileNoSearch" runat="server" MaxLength="100" CssClass="input"
                            Width="60%" EmptyMessage="Type the File No">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbldescription" runat="server" Text="Description">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" runat="server" MaxLength="100" CssClass="input"
                            Width="60%" EmptyMessage="Type the Description">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSource" runat="server" Text="Source Type">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlsource" runat="server" Width="200px" AppendDataBoundItems="true" AutoPostBack="true"
                            Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select source" OnSelectedIndexChanged="ddlsource_SelectedIndexChanged">
                        </telerik:RadComboBox>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblvessellist" runat="server" Text="Vessel">
                        </telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ddlvessel" AppendDataBoundItems="true"  Width="200px"
                            CssClass="input" SyncActiveVesselsOnly="true" />
                    </td>
                </tr>

            </table>
            <eluc:TabStrip ID="MenuRegistersFMSFileNo" runat="server" OnTabStripCommand="RegistersFMSFileNo_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvFMSFileNo" runat="server"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
                GridLines="None" OnNeedDataSource="gvFMSFileNo_NeedDataSource" OnItemDataBound="gvFMSFileNo_ItemDataBound"
                OnItemCommand="gvFMSFileNo_ItemCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDFMSMAILFILENOID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true"
                        ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <NoRecordsTemplate>
                        <table id="Table1" runat="server" width="100%" border="0">
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
                        <telerik:GridTemplateColumn HeaderText="File No" ColumnGroupName="FileNo" AllowSorting="true"
                            ShowSortIcon="true" SortExpression="FLDFMSMAILFILENOID">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description">
                            <HeaderStyle Width="65%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNoId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFMSMAILFILENOID") %>'>
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lblFileNoDescription" CommandName="LINK" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENODESCRIPTION") %>'>
                                </asp:LinkButton>
                                <telerik:RadLabel ID="lblattachmentcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTCODE") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblurltype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFMSURLTYPE") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblurlfilter" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFMSURLFILTER") %>'>
                                </telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="SourceType">
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFMSTYPENAME") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lbltypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFMSTYPEID") %>'>
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
