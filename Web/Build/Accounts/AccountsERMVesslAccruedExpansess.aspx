<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsERMVesslAccruedExpansess.aspx.cs"
    Inherits="AccountsERMVesslAccruedExpansess" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CargoType" Src="~/UserControls/UserControlCargoType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cargo Type</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmERMOtherSubAccount" runat="server">
        <telerik:RadScriptManager runat="server" ID="ToolkitScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlERMVesslAccruedExpansess" Height="100%" EnableAJAX="false">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblaccount" Text="Account Id" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtaccountid" CssClass="input" Width="270px"></telerik:RadTextBox></td>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" Text="Account Code" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtcode" CssClass="input" Width="270px"></telerik:RadTextBox></td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" Text="Description" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAccountdescription" CssClass="input" Width="270px"></telerik:RadTextBox></td>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" Text="Budget Id" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtBudgetId" CssClass="input" Width="270px"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel4" Text="X Account" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtXAccount" CssClass="input" Width="270px"></telerik:RadTextBox></td>
                    <td>
                        <telerik:RadLabel ID="RadLabel5" Text="X Account Description" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtXAccountDescription" CssClass="input" Width="270px"></telerik:RadTextBox></td>
                </tr>

            </table>
            <eluc:TabStrip ID="MenuERMOtherSubAccount" runat="server" OnTabStripCommand="ERMOtherSubAccount_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvERMVesselAccruedExpensess" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvERMVesselAccruedExpensess_ItemCommand" OnItemDataBound="gvERMVesselAccruedExpensess_ItemDataBound"
                AllowPaging="true" AllowCustomPaging="true" Height="78%" AllowSorting="true" EnableViewState="false" ShowFooter="true"
                OnNeedDataSource="gvERMVesselAccruedExpensess_NeedDataSource" OnSortCommand="gvERMVesselAccruedExpensess_Sorting"
                GroupingEnabled="false" EnableHeaderContextMenu="true">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDTKEY">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Account Id" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                            </ItemTemplate>

                            <EditItemTemplate>
                                <telerik:RadLabel ID="lbldtkeyEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtlAccountIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'
                                    CssClass="input" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>

                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtlAccountIdAdd" runat="server" CssClass="input" Width="98%" MaxLength="200"
                                    ToolTip="Enter Account Id">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Code" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>

                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'
                                    CssClass="input" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>

                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtCodeAdd" runat="server" CssClass="input" Width="98%" MaxLength="200"
                                    ToolTip="Enter Acount Code">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>

                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                    CssClass="input" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>

                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" CssClass="input" Width="98%" MaxLength="200"
                                    ToolTip="Account Description">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Id" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadLabel>
                            </ItemTemplate>

                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'
                                    CssClass="input" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>

                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtBudgetIdadd" runat="server" CssClass="input" Width="98%" MaxLength="200"
                                    ToolTip="Enter Budget Id">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="X Account" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblXAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXACC") %>'></telerik:RadLabel>
                            </ItemTemplate>

                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtXAccountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXACC") %>'
                                    CssClass="input" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>

                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtXAccountAdd" runat="server" CssClass="input" Width="98%" MaxLength="200"
                                    ToolTip="Enter Company Name">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="X Account Description" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblXAccountDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXACCOUNTDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>

                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtXAccountDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDXACCOUNTDESCRIPTION") %>'
                                    CssClass="input" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>

                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtXAccountDescriptionAdd" runat="server" CssClass="input" Width="95%" MaxLength="200"
                                    ToolTip="Enter X Account Description">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>

                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>

                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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
