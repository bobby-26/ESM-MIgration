<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalDMFForms.aspx.cs"
    Inherits="Dashboard_DashboardTechnicalDMFForms" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvForms.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
            function CloseUrlModelWindow() {
                var wnd = $find('<%=RadWindow_NavigateUrl.ClientID %>');
                wnd.close();
                var masterTable = $find('<%=gvForms.ClientID %>').get_masterTableView();
                masterTable.rebind();
            }
            function pageLoad() {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel" LoadingPanelID="RadAjaxLoadingPanel1">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" />
        <eluc:TabStrip ID="ExportForms" runat="server" OnTabStripCommand="MenuExportForms_TabStripCommand" />
       <%-- <table id="tblFiles" style="width: 100%">
            <tr>
                <td style="text-align: center">
                    <telerik:RadUpload ID="RadUpload1" runat="server" MaxFileInputsCount="10" OverwriteExistingFiles="false"
                        ControlObjectsVisibility="None" Skin="Silk">
                    </telerik:RadUpload>
                </td>
            </tr>
        </table>--%>
        <telerik:RadGrid ID="gvForms" runat="server" AutoGenerateColumns="false" AllowSorting="false" AllowCustomPaging="true" AllowPaging="true" 
            CellSpacing="0" GridLines="None"  GroupingEnabled="false" OnItemDataBound="gvForms_ItemDataBound" AllowMultiRowSelection="true" 
            OnItemCommand="gvForms_ItemCommand" EnableHeaderContextMenu="true" OnNeedDataSource="gvForms_NeedDataSource">
            <MasterTableView TableLayout="Fixed">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Date">
                        <ItemTemplate>
                            <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"].ToString())%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Form/Checklist">
                        <ItemTemplate>
                            <%--<asp:HyperLink ID="lnkForm" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDFORMNAME"] %>'></asp:HyperLink>--%>
                            <asp:HyperLink ID="lnkForm" Target="_blank" runat="server" ToolTip="Download Form"
                                Style="text-decoration: underline; cursor: pointer; color: Blue;" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFORMNAME") %>'>
                            </asp:HyperLink>
                            <telerik:RadLabel ID="lblRevisionid" runat="server" Visible="false" Text=' <%#((DataRowView)Container.DataItem)["FLDFORMREVISIONLIST"]%>'>
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblDailyplanactivityid" runat="server" Visible="false" Text=' <%#((DataRowView)Container.DataItem)["FLDDAILYPLANACTIVITYID"]%>'>
                            </telerik:RadLabel>
                            <eluc:ToolTip ID="ucFilenameTT" runat="server" TargetControlId="lnkForm" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>' />
                            <telerik:RadLabel ID="lblReportId" runat="server" Visible="false" Text=' <%#((DataRowView)Container.DataItem)["FLDREPORTID"]%>'></telerik:RadLabel>

                            <%--  <telerik:RadLabel ID="lblformrevisonid" runat="server" Visible="false" Text=' <%#((DataRowView)Container.DataItem)["FLDFORMREVISIONID"]%>'>--%>
                            <%--</telerik:RadLabel>--%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" UniqueName="Name">
                        <ItemTemplate>
                            <%-- <asp:LinkButton ID="lnkForm" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDFORMNAME"] %>'></asp:LinkButton>--%>
                            <telerik:RadLabel ID="lblUsername" runat="server" Text=' <%#((DataRowView)Container.DataItem)["FLDUSERNAME"]%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Type" UniqueName="Type">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblactive" runat="server" Text=' <%#((DataRowView)Container.DataItem)["FLDTYPENAME"]%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <%--<telerik:GridTemplateColumn HeaderText="Date">
                        <ItemTemplate>
                            <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"].ToString())%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                    <telerik:GridBoundColumn UniqueName="FLDSTATUS" HeaderText="Status" DataField="FLDSTATUS">
                    </telerik:GridBoundColumn>
                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="40px"
                        HeaderStyle-HorizontalAlign="Center" EnableHeaderContextMenu="true">
                    </telerik:GridClientSelectColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="" UniqueName="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Lock" ID="cmdLock" ToolTip="Unlock" CommandName="LOCKUNLOCK" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-lock"></i></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="UPLOAD" ID="cmdUpload"
                                CommandName="UPLOAD" ToolTip="Upload Form"><span class="icon"><i class="fas fa-file-upload"></i></span></asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Delete"
                                CommandName="DELETE" ID="cmdDelete" ToolTip="Delete"><span class="icon"><i class="fas fa-trash"></i></asp:LinkButton>

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
            <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="217px" />
                <Resizing AllowColumnResize="true" />
            </ClientSettings>
             <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                   PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    <telerik:RadWindow runat="server" ID="RadWindow_NavigateUrl" Width="900px" Height="365px"
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
