<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCRoleConfiguration.aspx.cs" Inherits="InspectionMOCRoleConfiguration" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Role</title>

    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server" Height="100%">
            <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <table cellpadding="2" cellspacing="2">
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblShortCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td style="padding-right: 40px">
                        <telerik:RadTextBox ID="txtShortCode" MaxLength="20" runat="server" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRole" runat="server" Text="Role"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRole" runat="server" Width="180px"></telerik:RadTextBox>
                    </td>

                </tr>
            </table>

            <eluc:TabStrip ID="MenuMOCApproverRole" runat="server" OnTabStripCommand="MenuMOCApproverRole_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvMOCApproverRole" runat="server" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true" Height="88%"
                Width="100%" CellPadding="3" OnItemCommand="gvMOCApproverRole_ItemCommand" OnNeedDataSource="gvMOCApproverRole_NeedDataSource"
                OnItemDataBound="gvMOCApproverRole_ItemDataBound" EnableHeaderContextMenu="true" GroupingEnabled="false"
                ShowHeader="true" EnableViewState="false" AllowSorting="true" OnSorting="gvMOCApproverRole_Sorting">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" Height="10px">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>

                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20px"></ItemStyle>
                            <HeaderStyle Width="40%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>' Width="50px"></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Role" AllowSorting="true" SortExpression="FLDMOCAPPROVERROLE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="45%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRole" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCAPPROVERROLE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRoleId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCAPPROVERROLEID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblMappingID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCROLEPROCESSMAPINGID") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Selected Process" AllowSorting="true" SortExpression="FLDMOCAPPROVERROLE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="45%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblselectedprocess" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSGROUPNAME") %>'></telerik:RadLabel>
                                <%--<telerik:RadLabel ID="lblRoleId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMOCAPPROVERROLEID") %>' Visible="false"></telerik:RadLabel>--%>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                         <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Role Mapping" ToolTip="Role Mapping" Width="20PX" Height="20PX"
                                    CommandName="ROLEMAPPING" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRoleMapping">
                                <span class="icon"><i class="fas fa-list-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave"
                                    ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel"
                                    ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />

                        </telerik:GridTemplateColumn>


                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
