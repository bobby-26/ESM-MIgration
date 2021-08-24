<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterCrewApprovalRoleConfiguration.aspx.cs" Inherits="Registers_RegisterCrewApprovalRoleConfiguration" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Role Configuration</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrewApproverRole.ClientID %>"));
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
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server" Height="100%">
            <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <table cellpadding="2" cellspacing="2">
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblShortCode" runat="server" Text="Short Code"></telerik:RadLabel>
                    </td>
                    <td style="padding-right: 40px">
                        <telerik:RadTextBox ID="txtShortCode" MaxLength="20" runat="server" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRole" runat="server" Text="Role Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRole" runat="server" Width="180px"></telerik:RadTextBox>
                    </td>

                </tr>
            </table>

            <eluc:TabStrip ID="MenuCrewApproverRole" runat="server" OnTabStripCommand="MenuCrewApproverRole_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvCrewApproverRole" runat="server" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true" Height="88%"
                Width="100%" CellPadding="3"
                OnNeedDataSource="gvCrewApproverRole_NeedDataSource"
                OnItemCommand="gvCrewApproverRole_ItemCommand"
                OnItemDataBound="gvCrewApproverRole_ItemDataBound"
                EnableHeaderContextMenu="true" GroupingEnabled="false" 
                ShowHeader="true" EnableViewState="true" AllowSorting="true" ShowFooter="true">

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
                        <telerik:GridTemplateColumn HeaderText="Short Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20px"></ItemStyle>
                            <HeaderStyle Width="40%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGURATIONCODE") %>' Width="50px"></telerik:RadLabel>
                            </ItemTemplate>
                               <EditItemTemplate>
                                <telerik:RadTextBox ID="txtcodeEdit" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGURATIONCODE") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtcodeAdd"  Width="100%" runat="server" ></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Role Name" AllowSorting="true" SortExpression="FLDCONFIGURATIONNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="45%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRole" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGURATIONNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRoleId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROLECONFIGURATIONID") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblRoleIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROLECONFIGURATIONID") %>' Visible="false"></telerik:RadLabel>

                                <telerik:RadTextBox ID="txtRoleEdit" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGURATIONNAME") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtRoleAdd"  Width="100%" runat="server" ></telerik:RadTextBox>
                            </FooterTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="SAVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                 <asp:ImageButton runat="server" AlternateText="Add" ImageUrl="<%$ PhoenixTheme:images/add.png %>"
                                    CommandName="ADD" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
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
