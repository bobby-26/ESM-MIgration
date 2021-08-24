<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayRollEmployeePFSingapore.aspx.cs" Inherits="PayRoll_PayRollEmployeePFSingapore" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PayRoll Employee PF Singapore</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvEmployeePFSingapore.ClientID %>"));
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
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>

        <%-- For Popup Relaod --%>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />

        <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
        <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvEmployeePFSingapore" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" ShowFooter="false" Style="margin-bottom: 0px" EnableViewState="true"
            OnNeedDataSource="gvEmployeePFSingapore_NeedDataSource"
            OnItemCommand="gvEmployeePFSingapore_ItemCommand"
            OnItemDataBound="gvEmployeePFSingapore_ItemDataBound">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                <Columns>

                <%--    <telerik:GridTemplateColumn HeaderText='' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPAYROLLPFCONTRIBUTIONID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLPFCONTRIBUTIONID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPAYROLLTAXID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLTAXID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>


                    <telerik:GridTemplateColumn HeaderText='Minimum Age Value' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMINAGEVALUE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINAGEVALUE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Maximum Age Value' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMAXAGEVALUE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXAGEVALUE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Minimum Wage Value' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMINWAGEVALUE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINWAGEVALUE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Maximum Wage Value' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMAXWAGEVALUE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXWAGEVALUE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText='Formula' AllowSorting='true'>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFORMULA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMULA") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="50px" AllowSorting='true' HeaderTooltip="Action">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="EDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLPFCONTRIBUTIONID") %>' ID="cmdEdit"
                                ToolTip="Edit" Width="20PX">
                                  <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>

                            <asp:LinkButton runat="server" AlternateText="Delete"
                                CommandName="DELETE" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDPAYROLLPFCONTRIBUTIONID") %>' ID="cmdDelete"
                                ToolTip="Delete" Width="20PX">
                                     <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>

                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                </Columns>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
