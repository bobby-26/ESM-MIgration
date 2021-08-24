<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFalApprovalLevel.aspx.cs" Inherits="PurchaseFalApprovalLevel" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Fal Approval Level</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvPurchaseFalApprove.ClientID %>"));
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
          <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
              <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false" />
               <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPurchaseFalApprove" runat="server" CellSpacing="0" GridLines="None"
                    AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableHeaderContextMenu="true" EnableViewState="false"
                    GroupingEnabled="false" OnNeedDataSource="gvPurchaseFalApprove_NeedDataSource" OnItemCommand="gvPurchaseFalApprove_ItemCommand"  OnItemDataBound="gvPurchaseFalApprove_ItemDataBound">

                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDQUOTATIONLVLAPPROVALID">

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
                            <telerik:GridTemplateColumn HeaderText="" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="3%" AllowFiltering="false">
                             <ItemStyle  HorizontalAlign="Center" Width="50px"></ItemStyle>
                                <ItemTemplate>
                                <asp:LinkButton runat="server" ID="imgFlag" Enabled="false" Width="15px" Height="15px" Visible="false" >
                                 <span class="icon" id="imgFlagcolor"  ><i class="fas fa-star-red"></i></span>      
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Level" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLevel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVEL") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                               <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                          

                            <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="120px"></ItemStyle>
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                            CommandName="APPROVE" ID="cmdApprove" ToolTip="Approve" Visible="false"></asp:ImageButton>
                                    <telerik:RadLabel runat="server" ID="radlblapprove" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'   Visible="false" />
                                     &nbsp
                                     <asp:ImageButton runat="server" AlternateText="Roll back approval" ImageUrl="<%$ PhoenixTheme:images/28.png %>"
                                            CommandName="ROLLBACK" ID="cmdRollback" ToolTip="Roll back approval" Visible="false"></asp:ImageButton>
                                    &nbsp
                                     <asp:ImageButton runat="server" AlternateText="Rollback reasons" ImageUrl="<%$ PhoenixTheme:images/notepad.png %>"
                                             ID="btnreasons" ToolTip="Rollback reasons" Visible="false"></asp:ImageButton>
                                    &nbsp
                                      <asp:ImageButton runat="server" AlternateText="Approval limit" ImageUrl="<%$ PhoenixTheme:images/final-quotation.png %>"
                                             ID="btnapprovallimit" ToolTip="Approval limit" Visible="false" ></asp:ImageButton>
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
              <table cellpadding="1" cellspacing="1">
                <tr>
                    
                    <td>
                        <span class="icon"><i class="fas fa-star-red"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbldrilloverdue" runat="server" Text="* Additional Level click for details"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
              </telerik:RadAjaxPanel>

    </form>
</body>
</html>
