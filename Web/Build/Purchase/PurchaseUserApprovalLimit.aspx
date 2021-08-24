<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseUserApprovalLimit.aspx.cs" Inherits="PurchaseUserApprovalLimit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }
        </script>
         <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvUserApprovalLimit.ClientID %>"));
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
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:TabStrip ID="MenuPurchaseConfig" runat="server" OnTabStripCommand="MenuPurchaseConfig_TabStripCommand" TabStrip="true" />
          <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblSearch">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblstocktye" runat="server" Text="Stock Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="ddlStockType" Width="180px">
                            <Items>
                                <telerik:DropDownListItem Text="--Select--" Value="" />
                                <telerik:DropDownListItem Text="Spares" Value="SPARE" />
                                <telerik:DropDownListItem Text="Stores" Value="STORE" />
                                <telerik:DropDownListItem Text="Service" Value="SERVICE" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuUserApprovalLimit" runat="server" OnTabStripCommand="MenuUserApprovalLimit_TabStripCommand" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvUserApprovalLimit" runat="server" CellSpacing="0" GridLines="None" 
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableHeaderContextMenu="true" EnableViewState="false"
                GroupingEnabled="false" OnNeedDataSource="gvUserApprovalLimit_NeedDataSource" OnItemCommand="gvUserApprovalLimit_ItemCommand"
                 OnItemDataBound="gvUserApprovalLimit_ItemDataBound" >

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="Stock Type" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="120px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDUSERAPPROVALLIMITID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblstocktype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Exceed Budget Minimum" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="120px"></ItemStyle>
                            <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="txtExcMinimum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCBUDGETMINIMUM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Exceed Budget Maximum" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="120px"></ItemStyle>
                            <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="txtExcMaximum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCBUDGETMAXIMUM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="In Budget Minimum" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="120px"></ItemStyle>
                            <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="txtInMinimum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINBUDGETMINIMUM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="In Budget Maximum" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="120px"></ItemStyle>
                            <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="txtInMaximum" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINBUDGETMAXIMUM","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="120px"></ItemStyle>
                            <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="txtVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="User" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="120px"></ItemStyle>
                            <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="txtUser" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.NAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="120px"></ItemStyle>
                            <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />

                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder">

                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>

            </telerik:RadGrid>

               <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>




