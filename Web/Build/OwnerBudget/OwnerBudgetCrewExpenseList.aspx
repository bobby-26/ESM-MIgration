<%@ Page Language="C#" AutoEventWireup="True" CodeFile="OwnerBudgetCrewExpenseList.aspx.cs" Inherits="OwnerBudgetCrewExpenseList"
    MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="QuickType" Src="~/UserControls/UserControlQuickType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Quick</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvQuick.ClientID %>"));
                }, 200);
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersQuick" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlCountryEntry">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblConfigureQuick" width="50%">
                <tr>
                    <td>
                        <asp:Literal ID="lblNationality" runat="server" Text="Nationality"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Nationality runat="server" ID="ucNationality" CssClass="input" AppendDataBoundItems="true"
                            AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblExpenseType" runat="server" Text="Expense Type"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlExpenseType" runat="server" AutoPostBack="true" CssClass="input">
                            <Items>
                                <telerik:DropDownListItem Text="Crew Expenses" Value="112" Selected="True"></telerik:DropDownListItem>
                                <telerik:DropDownListItem Text="Other Crew Expenses" Value="113"></telerik:DropDownListItem>
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersQuick" runat="server" OnTabStripCommand="RegistersQuick_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvQuick" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvQuick_ItemCommand" OnItemDataBound="gvQuick_ItemDataBound"
                OnRowCreated="gvQuick_RowCreated" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                OnDeleteCommand="gvQuick_DeleteCommand" OnNeedDataSource="gvQuick_NeedDataSource"
                OnUpdateCommand="gvQuick_UpdateCommand" OnSortCommand="gvQuick_SortCommand" ShowFooter="true"
                ShowHeader="true" EnableViewState="false">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />

                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn FooterText="New Short" HeaderStyle-Width="330px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblExpense" runat="server" Text="Expense"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpense" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.EXPENSE") %>' Width="300px"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblExpenseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPENSEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFldExpense" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPENSE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Quick ID="ucExpenseAdd" runat="server" CssClass="gridinput_mandatory" Width="300px" ExpandDirection="Up"/>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="330px" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle HorizontalAlign="Right" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblAmount" runat="server" Text="Amount"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' Width="100px"></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucAmount" runat="server" IsPositive="true" DecimalPlace="2" CssClass="input_mandatory"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT") %>' Width="100px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucAmountAdd" runat="server" IsPositive="true" DecimalPlace="2" CssClass="input_mandatory"
                                    Width="100px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="300px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblFrequency" runat="server" Text="Frequency"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFrequency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FREQUENCY") %>' Width="100px"></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadDropDownList ID="ddlFrequency" runat="server" CssClass="input_mandatory" Width="100px">
                                    <Items>
                                        <telerik:DropDownListItem Text="Month" Value="1"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Year" Value="2"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Contract" Value="3"></telerik:DropDownListItem>
                                    </Items>
                                </telerik:RadDropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList ID="ddlFrequencyAdd" runat="server" CssClass="input_mandatory" Width="100px" ExpandDirection="Up">
                                    <Items>
                                        <telerik:DropDownListItem Text="Month" Value="1"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Year" Value="2"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Text="Contract" Value="3"></telerik:DropDownListItem>
                                    </Items>
                                </telerik:RadDropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle Width="70px" />
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
                            TelerikGridResize($find("<%= gvQuick.ClientID %>"));
                    }, 200);
                });
                </script>
            </telerik:RadCodeBlock>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
