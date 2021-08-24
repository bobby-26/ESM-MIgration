<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFalLevel.aspx.cs" Inherits="PurchaseFalLevel" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:TabStrip ID="MenuPurchaseConfig" runat="server" OnTabStripCommand="MenuPurchaseConfig_TabStripCommand" TabStrip="true" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblSearch">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtname" runat="server" Text=""></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuFalLevel" runat="server" OnTabStripCommand="MenuFalLevel_TabStripCommand" TabStrip="true" />
            <div id="divPage1" style="position: relative; z-index: +2">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvFalLevel" runat="server" CellSpacing="0" GridLines="None"
                    AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableHeaderContextMenu="true" EnableViewState="false"
                    GroupingEnabled="false" OnNeedDataSource="gvFalLevel_NeedDataSource" OnItemDataBound="gvFalLevel_ItemDataBound"
                    OnItemCommand="gvFalLevel_ItemCommand">

                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">

                        <NoRecordsTemplate>
                            <table runat="server" width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>

                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Exceed Budget" Name="Exceed Budget" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Within Budget" Name="In Budget" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>

                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Level" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbllevel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVEL") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Level Name" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="180px"></ItemStyle>
                                <HeaderStyle Width="180px" />
                                <ItemTemplate>
                                    <telerik:RadLabel runat="server" ID="lblId" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDLEVELID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lblLevelName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELNAME") %>' CommandName="LEVEL"></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Active" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadCheckBox runat="server" AutoPostBack="true" ID="chkRequiredYN" BackColor="Transparent" CommandName="REQUIRED" />
                                    <telerik:RadLabel ID="lblRequired" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUIRED") %>' Visible="false"></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                               <telerik:GridTemplateColumn HeaderText="Minimum" ColumnGroupName="In Budget" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblMinimum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINBUDGETMINIMUM","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Maximum" ColumnGroupName="In Budget" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblMaximum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINBUDGETMAXIMUM","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Minimum" ColumnGroupName="Exceed Budget"  HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblExcMinimum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCBUDGETMINIMUM","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Maximum" ColumnGroupName="Exceed Budget" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblExcMaximum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCBUDGETMAXIMUM","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                         

                            <telerik:GridTemplateColumn HeaderText="Group" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.GROUPNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Target" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTarget" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TARGETNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Form Type" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFormType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblQuickName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="FALDELETE" ID="cmdFalDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />

                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                        ColumnsReorderMethod="Reorder">

                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>

                </telerik:RadGrid>
            </div>
            <br />

            <eluc:TabStrip ID="MenuFalLevelMapping" runat="server" Title="FAL Level Mapping " TabStrip="true"></eluc:TabStrip>
            <div id="divPage2" style="position: relative; z-index: +1">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvFalLevelMapping" runat="server" CellSpacing="0" GridLines="None"
                    EnableHeaderContextMenu="true" EnableViewState="false" GroupingEnabled="false" AllowPaging="true" AllowCustomPaging="true"
                    OnNeedDataSource="gvFalLevelMapping_NeedDataSource" OnItemCommand="gvFalLevelMapping_ItemCommand" OnItemDataBound="gvFalLevelMapping_ItemDataBound">

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
                        <NoRecordsTemplate>
                            <table runat="server" width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Exceed Budget" Name="Exceed Budget" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Within Budget" Name="In Budget" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Level" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLevelMappingId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFALLEVELMAPPINGID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblLevelId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblLevelMappingName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Stock Type" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="txtStockType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                              <telerik:GridTemplateColumn HeaderText="Minimum" ColumnGroupName="In Budget" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="txtInMinimum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINBUDGETMINIMUM","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Maximum" ColumnGroupName="In Budget" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="txtInMaximum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINBUDGETMAXIMUM","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Minimum" ColumnGroupName="Exceed Budget" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="txtExcMinimum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCBUDGETMINIMUM","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Maximum" ColumnGroupName="Exceed Budget" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="txtExcMaximum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCBUDGETMAXIMUM","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                          

                            <telerik:GridTemplateColumn HeaderText="Vessel List" AllowFiltering="false">
                                <HeaderStyle HorizontalAlign="Center" Font-Bold="true" Width="80px" />
                                <ItemStyle HorizontalAlign="Center" Width="80px" Wrap="false" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="btnappliesto" ToolTip="Vessel List">
                                            <span class="icon"><i class="fa fa-ship" ></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="MAPDELETE" ID="cmdMapDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />

                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                        ColumnsReorderMethod="Reorder">

                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>

                </telerik:RadGrid>
            </div>
            <br />

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
