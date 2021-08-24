<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceJobPtwList.aspx.cs"
    Inherits="PlannedMaintenanceJobPtwList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PTW List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvJhaList.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
            function CloseUrlModelWindow() {
                var wnd = $find('<%=RadWindow_NavigateUrl.ClientID %>');
                wnd.close();
                document.getElementById("cmdHiddenSubmit").click();
            }
            function refresh() {
                if (typeof parent.CloseUrlModelWindow === "function")
                    parent.CloseUrlModelWindow();                
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>

    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="true" Width="100%">
            <telerik:RadNotification ID="RadNotification1" runat="server" AutoCloseDelay="2000" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"
                EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="Center" Font-Bold="true" Font-Size="X-Large" Animation="Fade" BackColor="#99ccff" ShowTitleMenu="false">
            </telerik:RadNotification>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <telerik:RadGrid ID="gvJhaList" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false" OnItemCommand="gvJhaList_ItemCommand"
                CellSpacing="0" GridLines="None" EnableViewState="true" AllowMultiRowEdit="true" OnNeedDataSource="gvJhaList_NeedDataSource"
                OnItemDataBound="gvJhaList_ItemDataBound" Width="100%">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="PTW Name" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPtwid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRevisionID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMREVISION") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReportId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblJHAMapId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJHAMAPID") %>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkPtwName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>' CommandName="PTW"></asp:LinkButton>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" UniqueName="Type">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDTYPENAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Status"  UniqueName="FLDSTATUS">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDSTATUS"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="" UniqueName="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="UPLOAD" ID="cmdUpload"
                                    CommandName="UPLOAD" ToolTip="Upload Form"><span class="icon"><i class="fas fa-file-upload"></i></span></asp:LinkButton>     
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" ID="cmdDelete" ToolTip="Delete"><span class="icon"><i class="fas fa-trash"></i></asp:LinkButton>
                                <asp:ImageButton runat="server" ID="cmdAtt" ToolTip="Attachment" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
        <telerik:RadWindow runat="server" ID="RadWindow_NavigateUrl" Width="500px" Height="300px"
            Modal="true" OffsetElementID="main" VisibleStatusbar="false" KeepInScreenBounds="true" ReloadOnShow="true" ShowContentDuringLoad="false">
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=RadWindow_NavigateUrl.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
