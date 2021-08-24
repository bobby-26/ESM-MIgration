<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerBudgetOtherExpenses.aspx.cs" Inherits="OwnerBudgetOtherExpenses" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="../UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvContactType.ClientID %>"));
                }, 200);
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmContactType" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlContactType">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblCauseSearch" width="25%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblManagementType" runat="server" Text="Management Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlManagementType" runat="server" CssClass="input_mandatory" AutoPostBack="true">
                            <Items>
                                <telerik:DropDownListItem Text="Full" Value="1"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="Technical" Value="2"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="Crew" Value="3"></telerik:DropDownListItem>
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuInspectionContactType" runat="server" OnTabStripCommand="InspectionContactType_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvContactType" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowFooter="true" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                ShowHeader="true" EnableViewState="false" OnSortCommand="gvContactType_SortCommand"
                OnItemCommand="gvContactType_ItemCommand" OnNeedDataSource="gvContactType_NeedDataSource"
                OnItemDataBound="gvContactType_ItemDataBound"
                OnUpdateCommand="gvContactType_UpdateCommand"
                OnDeleteCommand="gvContactType_DeleteCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Seq" HeaderStyle-Width="340px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblExpense" runat="server" Text="Expense"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOtherExpense" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHEREXPENSE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOtherExpensesId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHEREXPENSESID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExpenseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.OTHEREXPENSE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Quick ID="ucOtherExpense" runat="server" CssClass="gridinput_mandatory" AppendDataBoundItems="true"
                                    QuickTypeCode="116" Width="180px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="330px" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="200px"></ItemStyle>
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucAmount" runat="server" IsPositive="true" DecimalPlace="2" CssClass="input_mandatory"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucAmountAdd" runat="server" IsPositive="true" DecimalPlace="2" CssClass="input_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="250px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFrequency" runat="server" Text="Frequency"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFrequency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FREQUENCY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="ddlFrequency" runat="server" CssClass="input_mandatory">
                                    <Items>
                                        <telerik:DropDownListItem Text="Month" Value="1"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Year" Value="2"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Day" Value="4"></telerik:DropDownListItem>
                                    </Items>

                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList ID="ddlFrequencyAdd" runat="server" CssClass="input_mandatory">
                                    <Items>
                                        <telerik:DropDownListItem Text="Month" Value="1"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Year" Value="2"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Day" Value="4"></telerik:DropDownListItem>
                                    </Items>
                                </telerik:RadDropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle Width="65px" />
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <telerik:RadCodeBlock runat="server">
                <script type="text/javascript">
                    Sys.Application.add_load(function () {
                        setTimeout(function () {
                            TelerikGridResize($find("<%= gvContactType.ClientID %>"));
                    }, 200);
                });
                </script>
            </telerik:RadCodeBlock>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
