<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogBook.aspx.cs" Inherits="Log_ElectricLogBook" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>


<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tank List</title>
    <telerik:radcodeblock id="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvTankDetails.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:radcodeblock>
    <style>
        /*.overflow-none {
            overflow-x: hidden !important;
        }*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:radscriptmanager runat="server" id="RadScriptManager1" />
        <telerik:radskinmanager id="RadSkinManager1" runat="server" />

        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
        
        <div style="height: 100%; margin-left: auto; margin-right: auto; vertical-align: middle;">
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"
                    TabStrip="false"></eluc:TabStrip>
            <%--Height="100%"--%>
       <%--     <telerik:RadGrid RenderMode="Lightweight" ID="gvTankDetails" runat="server" AllowCustomPaging="true" AllowPaging="true" 
                ShowGroupPanel="false" CellSpacing="0" GridLines="None"
                OnItemDataBound="gvTankDetails_ItemDataBound"
                OnItemCommand="gvTankDetails_ItemCommand"
                OnNeedDataSource="gvTankDetails_needdatasource"
           >
                <ExportSettings IgnorePaging="true" OpenInNewWindow="true" FileName="TankList" ExportOnlyData="true">
                    <Pdf PageHeight="210mm" PageWidth="297mm" DefaultFontFamily="Arial Unicode MS" PageTopMargin="45mm"
                        BorderStyle="Medium" BorderColor="#666666">
                    </Pdf>
                </ExportSettings>
                <MasterTableView EditMode="InPlace"
                    InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Tank Name">
                            <HeaderStyle Width="220px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="IOPP NAME">
                            <HeaderStyle Width="118px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIOPPName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIOPPNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="ROB">
                            <HeaderStyle Width="118px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALROB") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="ROB Date">
                            <HeaderStyle Width="118px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRobDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBDATE", "{0:dd-MM-yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Capacity">
                            <HeaderStyle Width="118px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCapacity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPACITY") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Init"
                                    ID="cmdInitializeROB"
                                    ToolTip="Intialize ROB" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-file-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit Tank" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete Tank" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                
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
                <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="false" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>--%>

                 <telerik:RadGrid RenderMode="Lightweight" ID="gvTankDetails" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" ShowFooter="True" Style="margin-bottom: 0px" 
                OnItemDataBound="gvTankDetails_ItemDataBound"
                OnItemCommand="gvTankDetails_ItemCommand"
                OnNeedDataSource="gvTankDetails_needdatasource"
                >
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" 
                    AutoGenerateColumns="false" DataKeyNames="">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <Columns>

                           <telerik:GridTemplateColumn HeaderText="Tank Name">
                            <HeaderStyle Width="220px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="IOPP NAME">
                            <HeaderStyle Width="118px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIOPPName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIOPPNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="ROB">
                            <HeaderStyle Width="118px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALROB") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="ROB Date">
                            <HeaderStyle Width="118px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRobDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBDATE", "{0:dd-MM-yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Capacity">
                            <HeaderStyle Width="118px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCapacity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPACITY") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Init"
                                    ID="cmdInitializeROB"
                                    ToolTip="Intialize ROB" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-file-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit Tank" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete Tank" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        </Columns>
                    </MasterTableView>
                     </telerik:RadGrid>


        </div>
    </form>
</body>
</html>
