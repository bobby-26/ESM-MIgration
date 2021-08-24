<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenancePropertyValueList.aspx.cs" Inherits="PlannedMaintenancePropertyValueList" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Property Values</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvLuboillist.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
                fade('statusmessage');
            }
        </script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvElementList.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
                fade('statusmessage');
            }
        </script>
        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }
        </script>
   
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
           <eluc:TabStrip ID="MenuRankList" runat="server" OnTabStripCommand="MenuRankList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvLuboillist" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnItemCommand="gvLuboillist_ItemCommand" OnItemDataBound="gvLuboillist_ItemDataBound" OnNeedDataSource="gvLuboillist_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="">
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
                        <telerik:GridTemplateColumn HeaderText="Sample Reference">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPERTYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblpropertyname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPERTYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Value">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                               <telerik:RadLabel ID="lblvalueid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblvalue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                 <telerik:RadLabel ID="lblvalueidedit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUEID") %>'></telerik:RadLabel>
                                 <telerik:RadLabel ID="lblpropertyidedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPERTYID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtvalue" CssClass="gridinput_mandatory" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
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

             <telerik:RadGrid RenderMode="Lightweight" ID="gvElementList" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnItemCommand="gvElementList_ItemCommand" OnItemDataBound="gvElementList_ItemDataBound" OnNeedDataSource="gvElementList_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="">
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
                        <telerik:GridTemplateColumn HeaderText="Elements (ppm)">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPERTYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblpropertyname" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPERTYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Value">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvalueid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblvalue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                 <telerik:RadLabel ID="lblvalueidedit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblpropertyidedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPERTYID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtvalue" CssClass="gridinput_mandatory" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
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
        <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />

    </form>
</body>
</html>
