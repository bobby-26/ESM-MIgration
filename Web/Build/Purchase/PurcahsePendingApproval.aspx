<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurcahsePendingApproval.aspx.cs"
    Inherits="PurcahsePendingApproval" %>

<!DOCTYPE html>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OwnerBudgetCode" Src="~/UserControls/UserControlOwnerBudgetCode.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pending Approval</title>
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var browserHeight = $telerik.$(window).height();
            var grid = $find("gvPendingApproval");
            grid._gridDataDiv.style.height = (browserHeight - 190) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body>
    <form id="frmPendingApproval" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden"/>
        <br />
        <table style="width:50%;" >
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true"  />
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblorder" runat="server" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtnumber" runat="server"  Width="200px"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <br />
        <eluc:TabStrip ID="MenuPendingApproval" runat="server" OnTabStripCommand="MenuPendingApproval_TabStripCommand"></eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvPendingApproval" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvPendingApproval" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvPendingApproval_NeedDataSource" OnUpdateCommand="gvPendingApproval_UpdateCommand"
            OnItemDataBound="gvPendingApproval_ItemDataBound" OnItemCommand="gvPendingApproval_ItemCommand" OnSortCommand="gvPendingApproval_SortCommand"
            GroupingEnabled="false" EnableHeaderContextMenu="true" Width="100%">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDORDERID">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Vessel Name" SortExpression="FLDVESSELNAME">
                        <HeaderStyle Width="100px" />
                        <ItemTemplate>
                            <asp:Label ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                            <asp:Label ID="lblvesselname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Number" SortExpression="FLDFORMNO">
                        <HeaderStyle Width="85px" />
                        <ItemTemplate>
                            <asp:Label ID="lblorderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></asp:Label>
                            <asp:Label ID="lblFormNumberName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:Label>
                            <asp:Label ID="lblStockId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKCLASSID") %>'></asp:Label>
                            <asp:Label ID="lblStockType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></asp:Label>
                            <asp:Label ID="lblApprovalType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONAPPROVAL") %>'></asp:Label>
                            <asp:Label ID="lblQuotationid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONID") %>'></asp:Label>
                            <asp:Label ID="lblQuotation" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONAPPROVAL") %>'></asp:Label>
                            <asp:Label ID="lblTechdirector" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTECHDIRECTOR") %>'></asp:Label>
                            <asp:Label ID="lblFleetManager" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLEETMANAGER") %>'></asp:Label>
                            <asp:Label ID="lblSupdt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPT") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Title" SortExpression="FLDTITLE">
                        <ItemTemplate>
                            <asp:Label ID="lbltitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vendor" SortExpression="FLDNAME">
                         <ItemTemplate>
                            <asp:Label ID="lblVendorName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Budget Code" SortExpression="FLDBUDGETCODE">
                         <ItemTemplate>
                            <asp:Label ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:BudgetCode ID="ucBudgetCodeEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucBudgetCode_TextChangedEvent" />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Owner Budget Code" SortExpression="FLDOWNERBUDGETGROUP">
                        <ItemTemplate>
                            <asp:Label ID="lblOwnerBudgetCodeItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>'></asp:Label>
                            <asp:Label ID="lblOwnerBudgetMapID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODEMAPID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblOwnerBudgetCodeID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUPID") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:OwnerBudgetCode ID="ucOwnerBudgetCodeEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="100px" />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Quoted Price" SortExpression="FLDQUOTEDPRICE">
                        <HeaderStyle Width="70px" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label ID="lblQuotedPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Currency" SortExpression="FLDCURRENCY">
                        <HeaderStyle Width="70px" />
                        <ItemTemplate>
                            <asp:Label ID="lblCurrencycode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCY") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Assigned By" SortExpression="FLDASSIGNEDBY">
                        <ItemTemplate>
                            <asp:Label ID="lblAssignedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSIGNEDBY") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                          <ItemTemplate>
                            <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                              <asp:LinkButton runat="server" CommandName="APPROVE" ID="cmdApprove" ToolTip="Approve">
                                <span class="icon"><i class="fas fa-award"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="VIEW" ID="cmdView" ToolTip="View Quotation">
                                <span class="icon"><i class="fas fa-binoculars"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                <span class="icon"><i class="fas fa-times"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                    PageSizeLabelText="Records per page:" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
