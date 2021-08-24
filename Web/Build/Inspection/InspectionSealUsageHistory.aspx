<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSealUsageHistory.aspx.cs" Inherits="InspectionSealUsageHistory" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seal Usage History List</title>
    <telerik:RadCodeBlock runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSealUsageHistory" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <div class="subHeader" style="position: relative">
                <div id="div1">
                    <eluc:Title runat="server" ID="ucTitle" Text="Seal Usage History" ShowMenu="false" />
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
            </div>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuSeal" runat="server" OnTabStripCommand="MenuSeal_TabStripCommand"></eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative;">
                <%-- <asp:GridView ID="gvSealUsage" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        Font-Size="11px" OnRowDataBound="gvSealUsage_RowDataBound" OnRowCommand="gvSealUsage_RowCommand" 
                        AllowSorting="true" OnSorting="gvSealUsage_Sorting" 
                        ShowFooter="false" ShowHeader="true" Width="100%">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvSealUsage" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                   
                    OnNeedDataSource="gvSealUsage_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false"  TableLayout="Fixed" Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridButtonColumn CommandName="Edit" Text="DoubleClick" Visible="false" />
                            <telerik:GridTemplateColumn>
                                <itemstyle horizontalalign="Left" wrap="False" width="200px" />
                                <headertemplate>
                                    <asp:LinkButton ID="lnkLocationHeader" runat="server" CommandName="Sort" CommandArgument="FLDLOCATIONNAME"
                                       >Location&nbsp;</asp:LinkButton>
                                    <img id="FLDLOCATIONNAME" runat="server" visible="false" />
                                </headertemplate>
                                <itemtemplate>
                                    <telerik:RadLabel ID="lblSealUsageId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALUSAGEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblLocationName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'></telerik:RadLabel>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <itemstyle horizontalalign="Left" wrap="true" width="200px" />
                                <headertemplate>
                                    <telerik:RadLabel ID="lblSealPointHeader" runat="server">Seal Point</telerik:RadLabel>
                                </headertemplate>
                                <itemtemplate>
                                    <telerik:RadLabel ID="lblSealPoint" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALPOINTNAME") %>'></telerik:RadLabel>                                    
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <itemstyle horizontalalign="Left" wrap="False" width="80px" />
                                <headertemplate>
                                    <asp:LinkButton ID="lblSealNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDSEALNUMBER"
                                       >Seal Number&nbsp;</asp:LinkButton>
                                    <img id="FLDSEALNUMBER" runat="server" visible="false" />
                                </headertemplate>
                                <itemtemplate>
                                    <telerik:RadLabel ID="lblSealNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALNO") %>'></telerik:RadLabel>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <itemstyle horizontalalign="Left" wrap="False" width="80px" />
                                <headertemplate>
                                   <telerik:RadLabel ID="lblsealTypeHeader" runat="server"> Seal Type</telerik:RadLabel>
                                </headertemplate>
                                <itemtemplate>
                                    <telerik:RadLabel ID="lblSealType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALTYPENAME") %>'></telerik:RadLabel>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <itemstyle horizontalalign="Left" wrap="False" width="100px" />
                                <headertemplate>
                                    <telerik:RadLabel ID="lblSealAffixedByHeader" runat="server">Seal Affixed by</telerik:RadLabel>
                                </headertemplate>
                                <itemtemplate>
                                    <telerik:RadLabel ID="lblSealAffixedby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERSONAFFIXINGSEAL") %>'></telerik:RadLabel>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <itemstyle horizontalalign="Left" wrap="False" width="80px" />
                                <headertemplate>
                                    <asp:LinkButton ID="lnkDateAffixedHeader" runat="server" CommandName="Sort" CommandArgument="FLDDATEAFFIXED"
                                        >Date Affixed&nbsp;</asp:LinkButton>
                                    <img id="FLDDATEAFFIXED" runat="server" visible="false" />
                                </headertemplate>
                                <itemtemplate>
                                    <telerik:RadLabel ID="lblDateAffixed" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEAFFIXED")) %>'></telerik:RadLabel>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <itemstyle horizontalalign="Left" wrap="False" width="90px" />
                                <headertemplate>
                                    <telerik:RadLabel ID="lblReasonHeader" runat="server">Reason</telerik:RadLabel>
                                </headertemplate>
                                <itemtemplate>
                                    <telerik:RadLabel ID="lblReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASONNAME") %>'></telerik:RadLabel>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" ScrollHeight="" UseStaticHeaders="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>

        </div>

    </form>
</body>
</html>
