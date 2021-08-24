<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersInventoryPurchase.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="OwnersInventoryPurchase" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/OwnersReport.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <%: Scripts.Render("~/bundles/js") %>
        <script type="text/javascript">
            //Dummy function to ignore javascript page error
            function OnHeaderMenuShowing(sender, args) {
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .panelHeight {
            height: 440px;
        }

        .panelfont {
            overflow: auto;
            font-size: 11px;
        }

        .RadGrid .rgHeader, .RadGrid th.rgResizeCol, .RadGrid .rgRow td, .RadGrid .rgAltRow td {
            padding-left: 2px !important;
            padding-right: 2px !important;
        }

        .higherZIndex {
            z-index: 2;
        }

        .RadGrid_Windows7 .rgHeader {
            color: black !important;
        }

        .rdTitleWrapper, .rdTitleBar {
            background-color: rgb(194, 220, 252) !important;
            background-image: linear-gradient(rgb(244, 248, 250), rgb(233, 242, 251) 50%, rgb(221, 231, 245) 50%, rgb(228, 237, 248)) !important;
            color: black !important;
        }

        .rgGroupCol {
            padding-left: 0 !important;
            padding-right: 0 !important;
            font-size: 1px !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="93%" EnableAJAX="false">
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table style="width: 100%" runat="server">
                <tr>
                    <td style="width: 35%; vertical-align: top;">
                        <telerik:RadDockZone ID="RadDockZone1" runat="server" Orientation="Vertical" Docks="RadDock1,RadDock2" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock1" RenderMode="Lightweight" runat="server" Height="585px" Title="Purchase Summary"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                        <ContentTemplate>
                                            <table>
                                                <tr valign="middle">
                                                    <td>
                                                        <telerik:RadLabel runat="server" ID="lblPurchase" Text="Purchase Summary"></telerik:RadLabel>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkPurchaseSummaryInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                    </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;  
                                                        <asp:LinkButton ID="lnkPurchaseSummaryComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                                        </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </TitlebarTemplate>
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="GVPUR" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None" Height="545px"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="GVPUR_NeedDataSource"
                                        OnItemDataBound="GVPUR_ItemDataBound">
                                        <MasterTableView TableLayout="Fixed">
                                            <NoRecordsTemplate>
                                                <table runat="server" width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Item" HeaderStyle-Width="60%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Count" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUECOUNT") %>'></asp:LinkButton>
                                                        <telerik:RadLabel ID="lblCount" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUEURL") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                            <Resizing AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                    <td style="width: 65%; vertical-align: top;">
                        <telerik:RadDockZone ID="RadDockZone2" runat="server" Orientation="Vertical" Docks="RadDock1,RadDock2" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock2" RenderMode="Lightweight" runat="server" Height="585px"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton
                                                            ID="lnkInventory"
                                                            runat="server"
                                                            Style="text-decoration: underline; color: black;"
                                                            Text="Inventory - List of Major Parts onboard">
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkListofMajorPartsInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                    </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp; 
                                                        <asp:LinkButton ID="lnkListofMajorPartsComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                                        </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>


                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </TitlebarTemplate>
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="GVR" runat="server" AutoGenerateColumns="False" Width="100%" Height="545px"
                                        AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" CellSpacing="0" GridLines="None" EnableViewState="false"
                                        OnNeedDataSource="GVR_NeedDataSource" OnItemDataBound="GVR_ItemDataBound" OnColumnCreated="GVR_ColumnCreated" OnItemCreated="GVR_ItemCreated">
                                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="false" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false">
                                            <NoRecordsTemplate>
                                                <table runat="server" width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="SHORT FALL NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>

                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Component No." HeaderStyle-Width="15%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblcompno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Component Name" HeaderStyle-Width="30%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblcompname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Item" HeaderStyle-Width="30%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="ROB" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQTY") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Min Qty" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" HeaderTooltip="Stock Minimum Qty">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblSMQ" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINIMUMQTY") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblshortfallyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISREQUIREDYN") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings>
                                            <Resizing AllowColumnResize="true" />
                                            <Selecting AllowRowSelect="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadDockZone ID="RadDockZone3" runat="server" Orientation="Vertical" Docks="RadDock1,RadDock2" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock3" RenderMode="Lightweight" runat="server" Height="585px"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <table width="24%">
                                                <tr valign="middle">
                                                    <td>
                                                        <asp:LinkButton
                                                            ID="lnkspareitem"
                                                            runat="server"
                                                            Style="text-decoration: underline; color: black;"
                                                            Text="Inventory - Spare Item Transaction">
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkSpareItemTransactionInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                    </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkSpareItemTransactioncomment" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                                    </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </TitlebarTemplate>
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="GVINV" runat="server" AutoGenerateColumns="False" Width="100%" Height="545px"
                                        AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" CellSpacing="0" GridLines="None" EnableViewState="false"
                                        OnNeedDataSource="GVINV_NeedDataSource" OnItemDataBound="GVINV_ItemDataBound" OnColumnCreated="GVINV_ColumnCreated" OnItemCreated="GVINV_ItemCreated">
                                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="false" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false">
                                            <NoRecordsTemplate>
                                                <table runat="server" width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>

                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Component No." HeaderStyle-Width="15%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblcompno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Component Name" HeaderStyle-Width="30%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblcompname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="Item No." HeaderStyle-Width="15%" Visible="false">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblcompno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Item" HeaderStyle-Width="30%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="Purchased" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEQTY") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Consumed" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblSMQ" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSUMEDQTY") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings>
                                            <Resizing AllowColumnResize="true" />
                                            <Selecting AllowRowSelect="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
            <%--            </telerik:RadDockLayout>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
