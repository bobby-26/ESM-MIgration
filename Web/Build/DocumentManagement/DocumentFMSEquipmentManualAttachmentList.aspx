<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DocumentFMSEquipmentManualAttachmentList.aspx.cs"
    Inherits="DocumentFMSEquipmentManualAttachmentList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manuals</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function fnConfirmTelerik(sender, msg) {
                var callBackFn = function (shouldSubmit) {
                    if (shouldSubmit) {
                        //sender.click();
                        //if (Telerik.Web.Browser.ff) {
                        //    sender.get_element().click();
                        //}
                        eval(sender.target.parentElement.parentElement.href);
                    }
                    else {
                        if (e.which)
                            e.stopPropagation();
                        else
                            window.event.cancelBubble = true;
                        return false;
                    }
                }
                var confirm;

                if (msg == null)
                    confirm = radconfirm("Are you sure you want to delete this record?", callBackFn);
                else
                    confirm = radconfirm(msg, callBackFn);

                return false;
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
            Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <telerik:RadGrid ID="gvManual" RenderMode="Lightweight" runat="server" Height="100%" OnItemDataBound="gvManual_ItemDataBound"
                OnNeedDataSource="gvManual_NeedDataSource" OnItemCommand="gvManual_ItemCommand"
                EnableViewState="false" ShowFooter="true" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle Width="85%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkManualName" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDMANUALNAME"]%>' CommandName="View"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" Visible="false">
                            <HeaderStyle Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete" Visible="false">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
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
