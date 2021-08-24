<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterDocumentGlobalComponent.aspx.cs" Inherits="Registers_RegisterDocumentGlobalComponent" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Global Component</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvRankGroup.ClientID %>"));
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
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmArchivedDocuments" DecoratedControls="All" />

    <form id="frmRanklist" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="radajaxpanel1" runat="server" Height="60%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <eluc:TabStrip ID="MenuRankList" runat="server" OnTabStripCommand="MenuRankList_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblComponentno" runat="server" Text="Component No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentno" runat="server" CssClass="input" OnTextChanged="txtComponentno_TextChanged" Width="250px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblComponentname" runat="server" Text="Component Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentname" runat="server" CssClass="input" OnTextChanged="txtComponentname_TextChanged" Width="250px"></telerik:RadTextBox>
                     
                        <asp:LinkButton runat="server" AlternateText="Find" CommandName="FIND" ID="cmdFind" ToolTip="Search"><span class="icon"><i class="fas fa-search"></i></span></asp:LinkButton>
                    </td>

                </tr>
            </table>


            <telerik:RadGrid RenderMode="Lightweight" ID="gvRankGroup" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvRankGroup_NeedDataSource"
                OnItemDataBound="gvRankGroup_ItemDataBound" OnItemCommand="gvRankGroup_ItemCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false" AllowFilteringByColumn="false" EnableLinqExpressions="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDGLOBALCOMPONENTID">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Select All">
                            <HeaderStyle Width="30px" />
                            <ItemStyle Wrap="false" HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <telerik:RadCheckBox ID="chkAllSeal" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckAll"
                                    OnPreRender="CheckAll" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" CommandName="SELECT" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSELECTEDYN").ToString().Equals("1"))?true:false %>' EnableViewState="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Global Component No." HeaderStyle-Width="75px" AllowFiltering="TRUE" UniqueName="FLDCOMPONENTNUMBER"
                            DataField="FLDCOMPONENTNUMBER" FilterControlWidth="100%" FilterDelay="2000"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>


                                <telerik:RadLabel ID="lblcomponentno" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER").ToString()) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Global Component" HeaderStyle-Width="75px" AllowFiltering="TRUE" UniqueName="FLDCOMPONENTNAME"
                            DataField="FLDCOMPONENTNAME" FilterControlWidth="100%" FilterDelay="2000"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblgroupid" Visible="false" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDGLOBALCOMPONENTID").ToString()) %>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblcomponent" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME").ToString()) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Applicable Maker" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadComboBox ID="chkRankList" runat="server"
                                    DataSource='<%# PhoenixRegisterDocumentRankGroup.MappedComponentWithMakerList(General.GetNullableGuid(DataBinder.Eval(Container,"DataItem.FLDGLOBALCOMPONENTID").ToString())) %>'
                                    DataTextField="FLDMAKE" DataValueField="FLDMODELID"
                                    EmptyMessage="Type to select maker" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="200px">
                                </telerik:RadComboBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
