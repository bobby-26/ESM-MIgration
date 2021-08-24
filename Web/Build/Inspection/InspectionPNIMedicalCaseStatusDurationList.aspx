<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPNIMedicalCaseStatusDurationList.aspx.cs"
    Inherits="Inspection_InspectionPNIMedicalCaseStatusDurationList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Doctor Visit</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>


</head>
<body>
    <form id="frmDeficiency" runat="server" autocomplete="off">
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
            <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

                <table>
                    <tr>
                        <td></td>
                        <td>
                            <telerik:RadLabel ID="lblRefNo" runat="server" Text="Reference No."></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">
                    <%--  <asp:GridView ID="gvDeficiency" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        GridLines="None" Width="100%" CellPadding="3" OnRowDataBound="gvDeficiency_ItemDataBound"
                        ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvDeficiency" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvDeficiency_NeedDataSource"
                        OnItemCommand="gvDeficiency_ItemCommand"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                      
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Pending With">
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                 
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblPendingWith" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGWITH") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblPendingWithId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSDURATIONID") %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="From">
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                 
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblFromDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDFROMDATE")) %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="To">
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                 
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblToDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTODATE")) %>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="415px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>

            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
