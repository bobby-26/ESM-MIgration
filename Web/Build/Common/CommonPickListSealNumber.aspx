<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListSealNumber.aspx.cs"
    Inherits="CommonPickListSealNumber" %>

<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seal Number</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


        <eluc:TabStrip ID="Menuseal" runat="server" Title="Seal Number" OnTabStripCommand="Menuseal_TabStripCommand"></eluc:TabStrip>


        <div id="search">
            <table id="tblConfigureSealNumber" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSealType" runat="server" Text="Seal Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucSealType" runat="server" AppendDataBoundItems="true" QuickTypeCode="87"
                            AutoPostBack="true" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTypeSealNumber" runat="server" Text="Seal Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSealNumber" runat="server" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divGrid" style="position: relative; z-index: 0">
            <%--  <asp:GridView ID="gvSeal" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowCommand="gvSeal_RowCommand" OnRowDataBound="gvSeal_ItemDataBound"
                ShowHeader="true" OnSorting="gvSeal_Sorting" AllowSorting="true" EnableViewState="false">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSeal" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvSeal_NeedDataSource"
                OnItemCommand="gvSeal_ItemCommand"
              OnSortCommand="gvSeal_SortCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
        
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
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
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Seal Number">
                            <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                            <headerstyle wrap="false" horizontalalign="Center" />
                          
                            <itemtemplate>
                            <telerik:RadLabel ID="lblSealid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALID") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkSealNumber" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataSetIndex %>"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALNO") %>'></asp:LinkButton>
                        </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Seal Type">
                            <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                            <headerstyle wrap="false" horizontalalign="Center" />
                    
                            <itemtemplate>
                            <telerik:RadLabel ID="lblSealTypeid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALTYPE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblSealType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEALTYPENAME") %>'></telerik:RadLabel>
                        </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                            <headerstyle wrap="false" horizontalalign="Center" />
                     
                            <itemtemplate>
                            <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                        </itemtemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </div>


    </form>
</body>
</html>
