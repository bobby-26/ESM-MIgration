<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewVesselArrivalPortList.aspx.cs"
    Inherits="CrewVesselArrivalPortList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlRankList" Src="~/UserControls/UserControlRankList.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Arrival Port</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSeafarersList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuArrivalPort" runat="server" OnTabStripCommand="MenuArrivalPort_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" ReadOnly="true" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselPort" runat="server" EnableViewState="false" Height="80%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvVesselPort_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvVesselPort_ItemDataBound"
                OnItemCommand="gvVesselPort_ItemCommand" ShowFooter="false" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Tick" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="6%">
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkCheck" runat="server" Checked="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Port" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ETA" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblETA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETA") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPortCallId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTCALLID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ETD" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblETD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETD") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
