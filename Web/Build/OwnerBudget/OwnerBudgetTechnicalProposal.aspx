<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerBudgetTechnicalProposal.aspx.cs"
    Inherits="OwnerBudgetTechnicalProposal" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Technical Proposal</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvTechnicalProposal.ClientID %>"));
                }, 200);
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="ToolkitScriptManager1"
            runat="server">
        </telerik:RadScriptManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlTechnical" EnableAJAX="false">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <%--            <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" />--%>


            <eluc:TabStrip ID="MenuTechnicalProposal" runat="server" TabStrip="true" OnTabStripCommand="MenuTechnicalProposal_TabStripCommand" />

            <telerik:RadGrid RenderMode="Lightweight" ID="gvTechnicalProposal" runat="server" AutoGenerateColumns="False"
                Font-Size="11px" OnRowCreated="gvTechnicalProposal_RowCreated" Width="100%" CellPadding="3" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnItemCommand="gvTechnicalProposal_ItemCommand" OnItemDataBound="gvTechnicalProposal_ItemDataBound"
                OnUpdateCommand="gvTechnicalProposal_UpdateCommand"
                OnNeedDataSource="gvTechnicalProposal_NeedDataSource"
                ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                OnSortCommand="gvTechnicalProposal_SortCommand">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" CommandItemDisplay="Top">
                    <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Budget Group" HeaderStyle-Width="153px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetGroupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUP") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ucBudgetGroup" AppendDataBoundItems="true" HardTypeCode="30" HardList='<%#PhoenixRegistersHard.ListHard(1, 30,byte.Parse("0"), string.Empty)%>'
                                    runat="server" CssClass="input_mandatory" OnTextChangedEvent="ucBudgetGroupAdd_Changed" EnableViewState="true"
                                    ShortNameFilter="66,69,72,75,78,81,84,85,90,93,10" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code" HeaderStyle-Width="293px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListMainBudgetEdit">
                                    <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Width="60px" CssClass="input_mandatory" Enabled="False" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="180px" CssClass="input_mandatory" Enabled="False" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadTextBox>
                                    <asp:LinkButton ID="ImgShowBudgetEdit" runat="server" CommandArgument="<%# Container.DataItem %>">
                                       <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListMainBudget">
                                    <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="60px" CssClass="input_mandatory" Enabled="False"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="180px" CssClass="input_mandatory" Enabled="False"></telerik:RadTextBox>
                                    <asp:LinkButton ID="ImgShowBudgetAdd" runat="server">
                                       <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Current budget Per Year" HeaderStyle-Width="170px">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrentBudgetPerYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTBUDGET","{0:0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucCurrentBudgetPerYear" DecimalPlace="2" CssClass="input"
                                    runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTBUDGET","{0:0.00}") %>' />
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                            <FooterTemplate>
                                <eluc:Number ID="ucCurrentBudgetPerYearAdd" CssClass="input" DecimalPlace="2"
                                    runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Current budget Per Month" HeaderStyle-Width="183px">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrentBudgetPerMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTBUDGETMONTHLY","{0:0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Proposed budget Per Year" HeaderStyle-Width="182px">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProposedBudgetPerYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDBUDGET","{0:0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucProposedBudgetPerYear" DecimalPlace="2" CssClass="input_mandatory"
                                    runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDBUDGET") %>' />
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                            <FooterTemplate>
                                <eluc:Number ID="ucProposedBudgetPerYearAdd" CssClass="input_mandatory" DecimalPlace="2"
                                    runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Proposed budget Per Month" HeaderStyle-Width="220px">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProposedBudgetPerMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDBUDGETMONTHLY","{0:0.00}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Left" />
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtRemarksAdd" runat="server" CssClass="input"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="88px" />
                            <ItemStyle Width="20px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd"
                                    ToolTip="Add New"> <span class="icon"><i class="fas fa-plus-circle"></i> </span>  </asp:LinkButton>
                            </FooterTemplate>
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
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <telerik:RadCodeBlock runat="server">
                <script type="text/javascript">
                    Sys.Application.add_load(function () {
                        setTimeout(function () {
                            TelerikGridResize($find("<%= gvTechnicalProposal.ClientID %>"));
                    }, 200);
                });
                </script>
            </telerik:RadCodeBlock>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
