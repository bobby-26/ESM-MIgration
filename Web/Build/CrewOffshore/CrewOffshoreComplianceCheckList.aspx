<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreComplianceCheckList.aspx.cs" Inherits="CrewOffshoreComplianceCheckList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Compliance List</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="CrewQueryMenu" runat="server" OnTabStripCommand="CrewQueryMenu_TabStripCommand"></eluc:TabStrip>

            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <%--<asp:GridView ID="gvComplianceList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="gvComplianceList_RowDataBound"
                    ShowHeader="true" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvComplianceList" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvComplianceList_NeedDataSource"
                    OnItemDataBound="gvComplianceList_ItemDataBound"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
               
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
                        <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Status">
                                <HeaderStyle Width="50px" />
                                <itemtemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Document Type">
                                 <HeaderStyle Width="150px" />
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblDocType" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDOCTYPE"]%>'></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Document Category">
                                 <HeaderStyle Width="200px" />
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblDocCategory" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCATEGORYNAME"]%>'></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Document Name">
                                <HeaderStyle Width="200px" />
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblDocName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDOCUMENTNAME"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeDocId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDID"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDocId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDDOCUMENTID"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDueOverdue" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDDUEOVERDUE"]%>'></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDUEOVERDUENAME"]%>'></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Document Number">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblDocNumber" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDOCUMENTNUMBER"]%>'></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Issue Date">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblIssueDate" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATEOFISSUE"])%>'></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Expiry Date">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATEOFEXPIRY"])%>'></telerik:RadLabel>
                            </itemtemplate>
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
            </div>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="Img1" src="<%$ PhoenixTheme:images/red-symbol.png%>" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="lblOverDue" runat="server" Text=" * Overdue / Missing / Not cross checked"></asp:Literal>
                    </td>
                    <td>
                        <img id="Img2" src="<%$ PhoenixTheme:images/yellow-symbol.png%>" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="lblDueWithin30days" runat="server" Text=" * Due within 30 days"></asp:Literal>
                    </td>
                    <td>
                        <img id="Img5" src="<%$ PhoenixTheme:images/green-symbol.png%>" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="lblDueWithin60Days" runat="server" Text=" * Due within 60 days"></asp:Literal>
                    </td>
                </tr>
            </table>
        </div>

    </form>
</body>
</html>
