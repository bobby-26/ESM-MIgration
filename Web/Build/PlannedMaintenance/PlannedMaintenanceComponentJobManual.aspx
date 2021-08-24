<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentJobManual.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceComponentJobManual" %>

<%@ Import Namespace="System.Data" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manual/s</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuPMSManual" runat="server" OnTabStripCommand="MenuPMSManual_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvManual" RenderMode="Lightweight" runat="server" Height="93%" OnItemDataBound="gvManual_ItemDataBound"
                OnNeedDataSource="gvManual_NeedDataSource" OnItemCommand="gvManual_ItemCommand" OnSortCommand="gvManual_SortCommand"
                EnableViewState="false" ShowFooter="true" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table width="99.9%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Path" ItemStyle-Width="500px" HeaderStyle-Width="500px">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDMANUALPATH"]%>
                                
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="fileName" runat="server" Width="345px">
                                    <ClientEvents OnLoad="textBoxLoad" />
                                </telerik:RadTextBox>
                                <telerik:RadButton ID="selectFile" OnClientClicked="OpenFileExplorerDialog" AutoPostBack="false" Text="Open" runat="server">
                                </telerik:RadButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" >
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkManualName" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDMANUALNAME"]%>' CommandName="View"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle />
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
    <script>
        (function (global, undefined) {
            var textBox = null;

            function textBoxLoad(sender) {
                textBox = sender;
            }

            function OpenFileExplorerDialog() {
                var browserWidth = $telerik.$(window).width();
                var browserHeight = $telerik.$(window).height();
                global.radopen("PlannedMaintenaneManualsExplorer.aspx", "PMS Manual", Math.ceil(browserWidth * 95 / 100), Math.ceil(browserHeight * 95 / 100));
            }
            function OpenPDF(filepath, filename) {
                var browserWidth = $telerik.$(window).width();
                var browserHeight = $telerik.$(window).height();
                global.radopen(filepath, filename, Math.ceil(browserWidth * 95 / 100), Math.ceil(browserHeight * 95 / 100));
            }
            //This function is called from a code declared on the Explorer.aspx page
            function OnFileSelected(fileSelected) {
                if (textBox) {
                    textBox.set_value(fileSelected);
                }
            }

            global.OpenFileExplorerDialog = OpenFileExplorerDialog;
            global.OnFileSelected = OnFileSelected;
            global.textBoxLoad = textBoxLoad;
            global.OpenPDF = OpenPDF;
        })(window);
    </script>

</body>
</html>
