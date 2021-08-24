<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerBudgetProposal.aspx.cs"
    Inherits="OwnerBudgetProposal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Budget Proposal</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvBudgetProposal.ClientID %>"));
                }, 200);
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBudgetProposal" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlBudgetProposal">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td width="10%">
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td width="15%">
                        <telerik:RadTextBox ID="txtVesselName" runat="server" Width="80%" CssClass="input" />
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td width="30%">
                        <telerik:RadTextBox ID="txtProposalTitle" runat="server" CssClass="input" Width="80%" MaxLength="100" />
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblProposedDatebetween" runat="server" Text="Proposed Date between"></telerik:RadLabel>
                    </td>
                    <td width="20%">
                        <eluc:Date ID="ucFromProposedDate" DatePicker="true" runat="server" />
                        <eluc:Date ID="ucToProposedDate" DatePicker="true" runat="server" />
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuBudgetProposalTap" runat="server" OnTabStripCommand="BudgetProposal_TabStripCommand"></eluc:TabStrip>
            <telerik:RadButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvBudgetProposal" runat="server" AutoGenerateColumns="False" Font-Size="11px" GroupingEnabled="false" EnableHeaderContextMenu="true"
                Width="100%" CellPadding="3" OnItemCommand="gvBudgetProposal_ItemCommand" OnItemDataBound="gvBudgetProposal_ItemDataBound"
                OnDeleteCommand="gvBudgetProposal_DeleteCommand" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnNeedDataSource="gvBudgetProposal_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="280px" HeaderText="Proposal Title">
                            <ItemStyle Wrap="False" Width="40%" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkVoyageId" runat="server" CommandName="REVISION" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALTITLE") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblProposalId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSALID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name" HeaderStyle-Width="265px">
                            <ItemStyle Wrap="False" Width="30%" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Proposal Date" HeaderStyle-Width="265px">
                            <ItemStyle Wrap="False" Width="20%" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProposalDate" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDPROPOSALDATE", "{0:dd-MMM-yyyy}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Existing/New Vessel" HeaderStyle-Width="210px">
                            <ItemStyle Wrap="False" Width="20%" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExistingNew" runat="server"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="88px" />
                            <ItemStyle Width="20px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="REVISION" ID="cmdRevision" ToolTip="Revision">
                                                   <span class="icon"><i class="fas fa-copy"></i></span> 
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <telerik:RadCodeBlock runat="server">
                <script type="text/javascript">
                    Sys.Application.add_load(function () {
                        setTimeout(function () {
                            TelerikGridResize($find("<%= gvBudgetProposal.ClientID %>"));
                    }, 200);
                });
                </script>
            </telerik:RadCodeBlock>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
