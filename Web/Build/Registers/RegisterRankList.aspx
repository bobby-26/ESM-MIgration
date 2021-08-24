<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterRankList.aspx.cs" Inherits="RegisterRankList" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rank List</title>
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
        <%--        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />--%>
        <telerik:RadAjaxPanel ID="radajaxpanel1" runat="server" Height="60%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <eluc:TabStrip ID="MenuRankList" runat="server" OnTabStripCommand="MenuRankList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRankGroup" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvRankGroup_NeedDataSource"
                OnItemCommand="gvRankGroup_ItemCommand"
                OnItemDataBound="gvRankGroup_ItemDataBound"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDGROUPRANKID">
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
                                <telerik:RadCheckBox ID="chkAllSeal" runat="server" Text="Select All" AutoPostBack="true"
                                    OnPreRender="CheckAll" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" CommandName="SELECT" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSELECTEDYN").ToString().Equals("1"))?true:false %>' EnableViewState="true" OnCheckedChanged="SaveCheckedValues" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank Group" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblgroupid" Visible="false" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDGROUPRANKID").ToString()) %>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblExpiry" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDGROUPRANK").ToString()) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Applicable Ranks" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadComboBox ID="chkRankList" runat="server"
                                    DataSource='<%# PhoenixRegisterDocumentRankGroup.MappedRankWithGroupRankList(int.Parse(DataBinder.Eval(Container,"DataItem.FLDGROUPRANKID").ToString())) %>'
                                    DataTextField="FLDRANKNAME" DataValueField="FLDRANKID"
                                    EmptyMessage="Type to select rank" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="200px">
                                </telerik:RadComboBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="New Hand Age" Visible="false" UniqueName="FLDNEWHANDAGE" HeaderTooltip="Greater than" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <eluc:Number ID="ucnewhandage" runat="server"  DecimalPlace="0" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDNEWHANDAGE").ToString()) %>'
                            IsPositive="true" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Ex-Hand Age" Visible="false" UniqueName="FLDEXHANDAGE" HeaderTooltip="Greater than" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmappingid" runat="server" Visible="false" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDMAPPINGID").ToString()) %>'></telerik:RadLabel>
                                 <eluc:Number ID="ucexhandage" runat="server" DecimalPlace="0" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDEXHANDAGE").ToString()) %>'
                            IsPositive="true" />
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
            <%--<table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" Text="Rank Group" runat="server"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadCheckBoxList ID="chkRankGroupList" runat="server" Columns="3" CssClass="input_mandatory">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblrankexclude" Text="Exclude Rank" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="chkRankList" runat="server"
                            EmptyMessage="Type to select rank" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="150px">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
