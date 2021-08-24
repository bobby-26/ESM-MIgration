<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewJoiningLettersReports.aspx.cs" Inherits="Crew_CrewJoiningLettersReports" %>

<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="~/UserControls/UserControlDocuments.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Seaport" Src="~/UserControls/UserControlSeaport.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Letters And Forms</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvJoiningPapers.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvSeparateJoining.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvOtherReports.ClientID %>"));
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
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="ReportNotRelievedOnTime"
            runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>

            <eluc:TabStrip ID="MenuLettersAndForms" runat="server" OnTabStripCommand="MenuLettersAndForms_TabStripCommand"></eluc:TabStrip>

            <table width="100%">
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblJoiningPapers" runat="server" Text="Joining Papers"></telerik:RadLabel>
                        </b>

                    </td>

                </tr>
                <tr>
                    <td colspan="4">
                        <div>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvJoiningPapers" runat="server"
                                EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                CellSpacing="0" OnNeedDataSource="gvJoiningPapers_NeedDataSource" OnItemCommand="gvJoiningPapers_ItemCommand"
                                GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true" OnSortCommand="gvJoiningPapers_SortCommand"
                                ShowFooter="False" AutoGenerateColumns="false">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" TableLayout="Fixed">
                                    <HeaderStyle Width="102px" />
                                    <NoRecordsTemplate>
                                        <table width="100%" border="0">
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                                        Font-Bold="true">
                                                    </telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>

                                    <Columns>
                                        <telerik:GridTemplateColumn AllowSorting="false" HeaderStyle-Width="45px"
                                            ShowSortIcon="true">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadCheckBox ID="chkSelection" runat="server" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Letters" AllowSorting="false" HeaderStyle-Width="45px"
                                            ShowSortIcon="true">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblHardCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDCODE") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblHardCodeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn AllowSorting="false" HeaderStyle-Width="45px"
                                            ShowSortIcon="true">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkClick" runat="server" Text='Click here' CommandName="Join"></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvSeparateJoining" runat="server"
                            EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" OnNeedDataSource="gvSeparateJoining_NeedDataSource" OnItemCommand="gvSeparateJoining_ItemCommand"
                            GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true" OnSortCommand="gvSeparateJoining_SortCommand"
                            ShowFooter="False" AutoGenerateColumns="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed">
                                <HeaderStyle Width="102px" />
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                                    Font-Bold="true">
                                                </telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>

                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Letters" AllowSorting="false" HeaderStyle-Width="45px"
                                        ShowSortIcon="true">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblHardCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDCODE") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblHardCodeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn AllowSorting="false" HeaderStyle-Width="45px"
                                        ShowSortIcon="true">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkClick" runat="server" Text='Click here' CommandName="Select"></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvOtherReports" runat="server"
                            EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" OnNeedDataSource="gvOtherReports_NeedDataSource" OnItemCommand="gvOtherReports_ItemCommand"
                            GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true" OnSortCommand="gvOtherReports_SortCommand"
                            ShowFooter="False" AutoGenerateColumns="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed">
                                <HeaderStyle Width="102px" />
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                                    Font-Bold="true">
                                                </telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>

                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Letters" AllowSorting="false" HeaderStyle-Width="45px"
                                        ShowSortIcon="true">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblHardCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDCODE") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblHardCodeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblShortName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>' Visible="false"></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn AllowSorting="false" HeaderStyle-Width="45px"
                                        ShowSortIcon="true">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkClick" runat="server" Text='Click here' CommandName="Select"></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
