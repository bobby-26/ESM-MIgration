<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreReportCrewListFormatOSVISPetronasMatrix.aspx.cs" Inherits="CrewOffshore_CrewOffshoreReportCrewListFormatOSVISPetronasMatrix" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CldYear" Src="~/UserControls/UserControlCalenderYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="cldMonth" Src="~/UserControls/UserControlCalenderMonths.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Patronas Matrix</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button runat="server" Visible="false" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
       
        <eluc:TabStrip ID="MenuShellMatrixWeekly" runat="server" OnTabStripCommand="MenuShellMatrixWeekly_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblreptitle" runat="server" Text="VESSELS CREW - PETRONAS MATRIX "></telerik:RadLabel>
                    </b>
                </td>
            </tr>
        </table>
        <br />

        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel Name/ Vessel Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                        Width="260px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtLocation" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                        Width="150px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblOwner" runat="server" Text="Owner"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtOwner" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                        Width="260px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                        Width="150px"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />




        <%-- <b>
                            <asp:Literal ID="lblTitle" runat="server" Text="Shell Weekly Matrix"></asp:Literal></b>--%>
        <telerik:RadLabel ID="lblDeckHeader" runat="server" Text="DECK DEPARTMENT" Style="font-weight: bolder; text-decoration: underline;">DECK DEPARTMENT</telerik:RadLabel>
        <div id="divGrid" style="position: relative; z-index: 0" runat="server">
            <%-- <asp:GridView ID="gvshellmatrixView" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                Width="100%" CellPadding="3" ShowFooter="false"
                OnRowCreated="gvshellmatrixView_RowCreated"
                EnableViewState="False">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvshellmatrixView" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="Both" OnNeedDataSource="gvshellmatrixView_NeedDataSource"
                OnItemDataBound="gvshellmatrixView_ItemDataBound"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="200px">
                            <ItemStyle Wrap="true" Width="25%" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>

                                <telerik:RadLabel ID="lblheading" Text='<%# ((DataRowView)Container.DataItem)["FLDDISPLAYNAME"].ToString() %>' runat="server"></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <asp:GridView ID="gvShellNoRecordFound" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                Width="100%" CellPadding="3" ShowFooter="false" Visible="false"
                EnableViewState="False">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <Columns>
                    <asp:TemplateField HeaderText="Course" HeaderStyle-Width="30%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# ((DataRowView)Container.DataItem)["FLDDISPLAYNAME"].ToString() %>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>
        <div>
            <br />
            <br />
            <br />
        </div>
        <telerik:RadLabel ID="Label1" runat="server" Text="ENGINE DEPARTMENT" Style="font-weight: bolder; text-decoration: underline;">ENGINE DEPARTMENT</telerik:RadLabel>
        <div id="div1" style="position: relative; z-index: 0" runat="server">
            <%--<asp:GridView ID="gvshellmatrixViewEngine" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                Width="100%" CellPadding="3" ShowFooter="false"
                OnRowCreated="gvshellmatrixViewEngine_RowCreated"
                EnableViewState="False">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvshellmatrixViewEngine" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="Both" OnNeedDataSource="gvshellmatrixView_NeedDataSource"
                OnItemDataBound="gvshellmatrixViewEngine_ItemDataBound"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="60px">
                            <ItemStyle Wrap="true" Width="25%" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                               
                                <telerik:RadLabel ID="lblheading" Text='<%# ((DataRowView)Container.DataItem)["FLDDISPLAYNAME"].ToString() %>' runat="server"></telerik:RadLabel>
                               
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                Width="100%" CellPadding="3" ShowFooter="false" Visible="false"
                EnableViewState="False">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <Columns>
                    <asp:TemplateField HeaderText="Course" HeaderStyle-Width="30%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# ((DataRowView)Container.DataItem)["FLDDISPLAYNAME"].ToString() %>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>

        <eluc:Status ID="ucStatus" runat="server" />

    </form>
</body>
</html>

