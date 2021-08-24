<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsERMSubAccountTravel.aspx.cs"
    Inherits="AccountsERMSubAccountTravel" %>

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
    <form id="frmERMSubAccountTravel" runat="server">
        <telerik:RadScriptManager runat="server" ID="ToolkitScriptManager1" />
        <telerik:RadAjaxPanel runat="server" ID="pnlERMSubAccountTravel" Height="100%" EnableAJAX="false">
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblaccount" Text="Sub Account" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="subaccount" CssClass="input" Width="95%"></telerik:RadTextBox></td>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" Text="Sub Account Description" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="SubAccountDescription" CssClass="input" Width="95%"></telerik:RadTextBox></td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" Text="Account Description" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="Accountdescription" CssClass="input" Width="95%"></telerik:RadTextBox></td>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" Text="Mapped ERM Travel Allow" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="MappedERMTravelAllow" CssClass="input" Width="95%"></telerik:RadTextBox></td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuERMSubAccountTravel" runat="server" OnTabStripCommand="MenuERMSubAccountTravel_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvERMSubAccountTravel" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvERMSubAccountTravel_ItemCommand" OnItemDataBound="gvERMSubAccountTravel_ItemDataBound"
                AllowPaging="true" AllowCustomPaging="true" Height="82%" AllowSorting="true" EnableViewState="false" ShowFooter="true"
                OnNeedDataSource="gvERMSubAccountTravel_NeedDataSource" OnSortCommand="gvERMSubAccountTravel_Sorting"
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
                        <telerik:GridTemplateColumn HeaderText="Sub Account Id" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSubAccountId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNTID") %>'></telerik:RadLabel>
                            </ItemTemplate>

                            <EditItemTemplate>
                                <telerik:RadLabel ID="lbldtkeyEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtSubAccountIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNTID") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>

                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSubAccountIdAdd" runat="server" CssClass="gridinput_mandatory" Width="98%" MaxLength="200"
                                    ToolTip="Enter Sub Account Id">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub Account" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubAccountDesc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>

                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtSubAccountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>

                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSubAccountAdd" runat="server" CssClass="gridinput_mandatory" Width="98%" MaxLength="200"
                                    ToolTip="Enter Sub Acount">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub Account Description" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubAccDesc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDBOTBLSUBACCOUNTFLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>

                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtSubAccDescEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDBOTBLSUBACCOUNTFLDDESCRIPTION") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>

                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtSubAccDescAdd" runat="server" CssClass="gridinput_mandatory" Width="95%" MaxLength="200"
                                    ToolTip="Enter Sub Account Description">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Id" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                            </ItemTemplate>

                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtAccountIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>

                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtAccountIdadd" runat="server" CssClass="gridinput_mandatory" Width="98%" MaxLength="200"
                                    ToolTip="Enter Account Id">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Description" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccDesc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDBOTBLACCOUNTFLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>

                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtAccDescEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDBOTBLACCOUNTFLDDESCRIPTION") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>

                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtAccDescAdd" runat="server" CssClass="gridinput_mandatory" Width="95%" MaxLength="200"
                                    ToolTip="Enter Account Description">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Mapped ERM Travel Allow" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMappedERMTravelAllow" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPPEDERMTRAVELALLOW") %>'></telerik:RadLabel>
                            </ItemTemplate>

                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtMappedERMTravelAllowEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAPPEDERMTRAVELALLOW") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>

                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtMappedERMTravelAllowAdd" runat="server" CssClass="gridinput_mandatory" Width="95%" MaxLength="200"
                                    ToolTip="Enter Mapped ERM Travel Allow">
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
