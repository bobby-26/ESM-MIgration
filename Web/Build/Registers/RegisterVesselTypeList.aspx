<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterVesselTypeList.aspx.cs" Inherits="RegisterVesselTypeList" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Type List</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvvesseltype.ClientID %>"));
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
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="radajaxpanel1" runat="server" Height="80%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <eluc:TabStrip ID="MenuRankList" runat="server" OnTabStripCommand="MenuRankList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvvesseltype" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvvesseltype_NeedDataSource"
                OnItemDataBound="gvRankGroup_ItemDataBound" OnItemCommand="gvvesseltype_ItemCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDVESSELTYPEID">
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
                                <telerik:RadCheckBox ID="chkSelect" CommandName="SELECT" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSELECTEDYN").ToString().Equals("1"))?true:false %>' EnableViewState="true"  />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Type Group" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblgroupid" Visible="false" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPEID").ToString()) %>'></telerik:RadLabel>

                                <telerik:RadLabel ID="lblExpiry" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVESSELTYPENAME").ToString()) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Applicable Vessel Type" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadComboBox ID="chkRankList" runat="server"
                                    DataSource='<%#  PhoenixRegisterDocumentRankGroup.MappedVesselSubTypeWithVesselType(int.Parse(DataBinder.Eval(Container,"DataItem.FLDVESSELTYPEID").ToString())) %>'
                                    DataTextField="FLDTYPEDESCRIPTION" DataValueField="FLDVESSELTYPEID"
                                    EmptyMessage="Type to select vessel type" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="200px">
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
            <%--<table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" Text="Rank Group" runat="server"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadCheckBoxList ID="chkVesselTypeList" runat="server" Columns="3" CssClass="input_mandatory">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblrankexclude" Text="Exclude Vessel Sub Type" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="chkVesselsubTypeList" runat="server"
                            EmptyMessage="Type to select rank" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="150px">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>--%>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
