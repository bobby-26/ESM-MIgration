<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceSubWorkOrderWaiverDetail.aspx.cs"
    Inherits="PlannedMaintenanceSubWorkOrderWaiverDetail" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manual" Src="~/UserControls/UserControlPMSManual.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Waive</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">function CloseUrlModelWindow() {
                var wnd = $find('<%=RadWindow_NavigateUrl.ClientID %>');
                wnd.close();
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
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="true">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadNotification ID="RadNotification1" RenderMode="Lightweight" runat="server" AutoCloseDelay="2000" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"
                EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="Center" Font-Bold="true" Font-Size="X-Large" Animation="Fade" BackColor="#99ccff" ShowTitleMenu="false">
            </telerik:RadNotification>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="menuWaive" runat="server" TabStrip="false" Title="Waive Detail" />
            <telerik:RadGrid ID="gvWaiver" runat="server" AutoGenerateColumns="false" OnNeedDataSource="gvWaiver_NeedDataSource" OnItemDataBound="gvWaiver_ItemDataBound"
                EnableHeaderContextMenu="true" AllowMultiRowSelection="true" Height="35%" ClientSettings-Scrolling-AllowScroll="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <Columns>
                        <telerik:GridBoundColumn DataField="FLDWAIVENAME"></telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Required" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWaiveType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVETYPE")%>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequired" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUIRED").ToString()=="1" ? "Yes": "No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Requirement Met?" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequiredMet" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUIREDMET").ToString()=="1" ? "Yes": "No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Waived?" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWaived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWAIVED").ToString()=="1" ? "Yes": "No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton ID="lnkPtwWaive" runat="server" ImageUrl="<%$ PhoenixTheme:images/45.png %>" CommandName="WAIVE" ToolTip="Want to Waive?" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="99%" border="0">
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
            <telerik:RadGrid ID="gvPTWSummary" runat="server" AutoGenerateColumns="false" OnNeedDataSource="gvPTWSummary_NeedDataSource"
                OnItemDataBound="gvPTWSummary_ItemDataBound" OnItemCommand="gvPTWSummary_ItemCommand"
                EnableHeaderContextMenu="true" AllowMultiRowSelection="true" Height="35%">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDFORMID" ClientDataKeyNames="FLDFORMID" CommandItemDisplay="Top">
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="true" AddNewRecordText="Add PTW" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Hazard Number" UniqueName="NUMBER" HeaderStyle-Width="250px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHazardID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJHAID") %>'></telerik:RadLabel>

                                <asp:LinkButton ID="lnkNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItem %>"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnHazardEdit">
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtHazardNumberEdit" runat="server" Enabled="false"
                                        MaxLength="50" Width="30%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNUMBER") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtHazardEdit" runat="server" Enabled="false"
                                        Width="60%">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowHazardEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtHazardIdEdit" runat="server" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJHAID") %>'></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Form No." HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFormRevisionid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMREVISION") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReportId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTID") %>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkFormNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" AllowFiltering="false" HeaderStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReport" CommandName="REPORT" ToolTip="PTW Report" Visible="false">
                                 <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="99%" border="0">
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
            <eluc:TabStrip ID="MenuParts" runat="server" TabStrip="false" Title="Required Spares" OnTabStripCommand="MenuParts_TabStripCommand" />
            <telerik:RadGrid ID="gvPartSummary" runat="server" AutoGenerateColumns="false" OnNeedDataSource="gvPartSummary_NeedDataSource"
                OnItemDataBound="gvPartSummary_ItemDataBound" OnItemCommand="gvPartSummary_ItemCommand"
                EnableHeaderContextMenu="true" AllowMultiRowSelection="true" Height="35%">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSPAREITEMID" ClientDataKeyNames="FLDSPAREITEMID">
                    <Columns>
                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" EnableHeaderContextMenu="true"
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="40px">
                        </telerik:GridClientSelectColumn>
                        <telerik:GridTemplateColumn HeaderText="Number" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSpareItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMID") %>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn HeaderText="Name" DataField="FLDNAME"></telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="ROB" DataField="FLDROB" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px"
                            ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Required" DataField="FLDREQUIREDQUANTITY" HeaderStyle-HorizontalAlign="Center"
                            HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Shortage" HeaderStyle-HorizontalAlign="Center" 
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShortage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="120px"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="99%" border="0">
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
