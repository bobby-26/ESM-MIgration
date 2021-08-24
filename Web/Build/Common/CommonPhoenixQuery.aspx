<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPhoenixQuery.aspx.cs"
    Inherits="CommonPhoenixQuery" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Query</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body> 
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server" Width="100%">
            <table width="100%"> 
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblChooseaQuery" runat="server" Text="Choose a Query"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlPhoenixQuery" AppendDataBoundItems="true" Width="40%"
                              OnSelectedIndexChanged="ddlPhoenixQuery_SelectedIndexChanged" AutoPostBack="true" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="Dummy"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:Error runat="server" ID="ucError" Visible="false" />
            <eluc:TabStrip ID="MenuPhoenixQuery" runat="server" OnTabStripCommand="MenuPhoenixQuery_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvParameter" runat="server" Height="" AllowCustomPaging="false" 
                AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" EnableViewState="false"
                OnNeedDataSource="gvParameter_NeedDataSource" OnItemDataBound="gvParameter_ItemDataBound"
                GroupingEnabled="false" EnableHeaderContextMenu="true" AutoGenerateColumns="false" Width="99.9%">
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
                        <telerik:GridTemplateColumn HeaderText="Country Name">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSearchBy" runat="server" Text="Search By"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMandatory" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANDATORYYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDataType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATATYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblParameter" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARAMETER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCaption" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Country Name">
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblValue" runat="server" Text="Value"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadTextBox ID="txtEntry" runat="server" Visible="false" Width="40%"></telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtHidden" runat="server" Visible="false"></telerik:RadTextBox>
                                <%--<ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MMM/yyyy"
                                    Enabled="True" TargetControlID="txtHidden" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>--%>
                                <eluc:UserControlDate ID="txtEntryDate" runat="server" CssClass="input_mandatory" Visible="false" Width="20%" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <input type="text" id="lblProgress" value="" style="border: 0px; width: 480px; font-weight: bold;" />
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvQuery" runat="server" Height="500px" AllowCustomPaging="false" AllowSorting="true" 
                AllowPaging="false" CellSpacing="0" GridLines="None" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false" Width="99.9%">
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
                    </Columns>
                    <%--<PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />--%>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
