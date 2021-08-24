<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceJobJHAList.aspx.cs"
    Inherits="PlannedMaintenanceJobJHAList" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JHA</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript">
            function confirmDelete(args) {
                if (args) {
                    __doPostBack("<%=confirmDelete.UniqueID %>", "");
            }
        }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="formPMHistoryTemplate" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <table style="width: 100%">
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblCategory" Text="Process"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDropDownList ID="ddlCategory" runat="server" CssClass="input" AppendDataBoundItems="true" Width="240px"
                        DataTextField="FLDNAME" DataValueField="FLDCATEGORYID" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                        <Items>
                            <telerik:DropDownListItem Text="--Select--" Value="DUMMY" />
                        </Items>
                    </telerik:RadDropDownList>
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblHNumber" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtHNumber" Width="240px" OnTextChanged="txtHNumber_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lbl" Text="Job"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtJob" Width="240px" OnTextChanged="txtJob_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
                </td>
            </tr>
        </table>


        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="confirmDelete" runat="server" Text="confirm" OnClick="confirmDelete_Click" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvJHA" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Width="100%"
                CellSpacing="0" GridLines="None" OnItemCommand="gvJHA_ItemCommand" OnNeedDataSource="gvJHA_NeedDataSource" OnItemDataBound="gvJHA_ItemDataBound"
                EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="true" AddNewRecordText="Add JHA" ShowExportToPdfButton="false" />
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Hazard Number" UniqueName="NUMBER">
                            <HeaderStyle Width="180px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHazardID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID") %>'></telerik:RadLabel>

                                <asp:LinkButton ID="lnkNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItem %>"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnHazardEdit">
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtHazardNumberEdit" runat="server" Enabled="false"
                                        MaxLength="50" Width="30%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtHazardEdit" runat="server" Enabled="false"
                                        Width="60%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB") %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowHazardEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtHazardIdEdit" runat="server" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID") %>'></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Process" UniqueName="Category">
                            <HeaderStyle Width="180px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel RenderMode="Lightweight" runat="server" Visible="false" ID="lblCategoryID" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblCategory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job" UniqueName="JOB">
                            <HeaderStyle Width="180px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblJobText" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="210px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

