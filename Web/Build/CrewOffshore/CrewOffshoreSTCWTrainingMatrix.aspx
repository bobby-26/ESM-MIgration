<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreSTCWTrainingMatrix.aspx.cs" Inherits="CrewOffshore_CrewOffshoreSTCWTrainingMatrix" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TrainingMatrix" Src="~/UserControls/UserControlTrainingMatrixStandard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <%--  <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>--%>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .hidden {
            display: none;
        }

        .center {
            background: fixed !important;
        }
    </style>
    <script type="text/javascript">
        function submitit(chkdetails) {
            document.getElementById(chkdetails).ck_CheckedChanged();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvmatrixView">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvmatrixView" />
                        <telerik:AjaxUpdatedControl ControlID="ucConfirm" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadAjaxPanel ID="ajaxpanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button ID="confirm" runat="server" OnClick="confirm_Click" />
            <eluc:TabStrip ID="MenuVesselTypeList" runat="server" OnTabStripCommand="MenuVesselTypeList_TabStripCommand" Title="Vessel Type"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuDocTypeList" runat="server" OnTabStripCommand="MenuDocTypeList_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr><td colspan="2">
                       <h1> <asp:Label ID="lbltypehead" Text=""  runat="server"></asp:Label></h1>

                    </td></tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadRadioButtonList ID="rblFormat" runat="server" AutoPostBack="true" CssClass="input_mandatory" Columns="6" OnSelectedIndexChanged="rblFormat_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Value="2" Text="STCW" />
                                <telerik:ButtonListItem Value="5" Text="Flag" />
                                <telerik:ButtonListItem Value="4" Text="Charter Requirement" />
                                <telerik:ButtonListItem Value="3" Text="Company Requirement" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblvalue" Text="Select the value"  runat="server"></asp:Label>
                    </td>
                    <td>
                           <telerik:RadComboBox ID="ddlvalue" runat="server" 
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"  Width="100%" OnSelectedIndexChanged="ddlvalue_SelectedIndexChanged" AutoPostBack="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
              
            </table>

            <div>
                <eluc:TabStrip ID="CrewTrainingMenu" runat="server" OnTabStripCommand="CrewTrainingMenu_TabStripCommand"></eluc:TabStrip>

                <telerik:RadGrid RenderMode="Lightweight" ID="gvmatrixView" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="Both" OnNeedDataSource="gvmatrixView_NeedDataSource" EnableViewState="false"
                    OnItemCommand="gvmatrixView_ItemCommand"
                    OnItemDataBound="gvmatrixView_ItemDataBound"
                    GroupingEnabled="true" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <GroupByExpressions>
                            <telerik:GridGroupByExpression>
                                <SelectFields>
                                    <telerik:GridGroupByField FieldName="FLDCATEGORYNAME" FieldAlias="Category" SortOrder="Ascending" />
                                </SelectFields>
                                <GroupByFields>
                                    <telerik:GridGroupByField FieldName="FLDCATEGORYNAME" SortOrder="Ascending" />
                                </GroupByFields>
                            </telerik:GridGroupByExpression>
                        </GroupByExpressions>
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>

                            <telerik:GridTemplateColumn HeaderText="Document Name" HeaderStyle-Width="200px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lbldocid" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTID") %>' Width="60px" runat="server"></asp:Label>

                                    <asp:Label ID="lbldoctype" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>' Width="60px" runat="server"></asp:Label>

                                    <asp:Label ID="lblcode" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOCCODE") %>'></asp:Label>

                                    <asp:Label ID="lbldocname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:Label>

                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" FrozenColumnsCount="2" EnableNextPrevFrozenColumns="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>


            </div>
              <h2>
                <asp:Label ID="lblexphead" runat="server" Text="Rank & Vessel Type Experience"></asp:Label></h2>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvmatrixexp" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="Both" OnNeedDataSource="gvmatrixexp_NeedDataSource"
                OnItemDataBound="gvmatrixexp_ItemDataBound"
                OnItemCommand="gvmatrixexp_ItemCommand"
                EnableViewState="true"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false" ShowFooter="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblrankname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:Label>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblranknameedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:Label>
                                <asp:Label ID="lblrankidedit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></asp:Label>

                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlRank" runat="server" DataTextField="FLDRANKNAME" DataValueField="FLDRANKID"
                                    EmptyMessage="Type to select rank" Filter="Contains" MarkFirstMatch="true" Width="100%">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Selected Rank" HeaderStyle-Width="200px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblranklist" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSELECTEDRANK") %>'></asp:Label>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblranklistedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPERIENCEINRANKLIST") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadComboBox ID="ddlRankselectedit" runat="server" DataTextField="FLDRANKNAME" DataValueField="FLDRANKID"
                                    EmptyMessage="Type to select rank" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="100%">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlRankselect" runat="server" DataTextField="FLDRANKNAME" DataValueField="FLDRANKID"
                                    EmptyMessage="Type to select rank" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="100%">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank Exp" HeaderStyle-Width="70px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblrankexp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKEXPERIENCE") %>'></asp:Label>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtrankexpedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKEXPERIENCE") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtrankexp" runat="server"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Selected Vessel Type" HeaderStyle-Width="200px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblvesseltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSELECTEDVESSELTYPE") %>'></asp:Label>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lbltypelistedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPERIENCEVESSELTYPELIST") %>' Visible="false"></telerik:RadLabel>

                                <telerik:RadComboBox ID="ddlvesseltypeselectedit" runat="server" DataTextField="FLDTYPEDESCRIPTION" DataValueField="FLDVESSELTYPEID"
                                    EmptyMessage="Type to select vessel type" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="100%">
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlvesseltypeselect" runat="server" DataTextField="FLDTYPEDESCRIPTION" DataValueField="FLDVESSELTYPEID"
                                    EmptyMessage="Type to select vessel type" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="100%">
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Type Exp" HeaderStyle-Width="70px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblvesselexp" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPEEXPERIENCE") %>'></asp:Label>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtvesseltypeexpedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPEEXPERIENCE") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtvesseltypeexp" runat="server"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="75px">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>

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
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
