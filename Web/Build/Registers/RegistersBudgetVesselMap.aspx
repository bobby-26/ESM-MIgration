<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersBudgetVesselMap.aspx.cs"
    Inherits="Registers_RegistersBudgetVesselMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Budget Vessel Map</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBudgetGroup" runat="server">
      <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
    <telerik:RadAjaxPanel runat="server" ID="pnlBudgetGroup" Height="100%">
   
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <eluc:TabStrip ID="MenuBudget" runat="server" OnTabStripCommand="Budget_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
                <div>
                    <table>
                        <tr>
                            <td>
                                &nbsp;&nbsp;<asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td>
                                &nbsp;&nbsp;<eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" Width="90%"
                                    CssClass="dropdown_mandatory" VesselsOnly="true" AutoPostBack="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <eluc:TabStrip ID="MenuBudgetGroup" runat="server" OnTabStripCommand="BudgetGroup_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvBudgetGroup" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="77%"
                        Width="100%" CellPadding="3" OnItemCommand="gvBudgetGroup_ItemCommand" OnItemDataBound="gvBudgetGroup_ItemDataBound"
                         OnDeleteCommand="gvBudgetGroup_DeleteCommand"   ShowFooter="false" ShowHeader="true" EnableViewState="false"
                        OnSortCommand="gvBudgetGroup_SortCommand"  OnRowCreated="gvBudgetGroup_RowCreated" OnNeedDataSource="gvBudgetGroup_NeedDataSource"
                        OnSelectedIndexChanging="gvBudgetGroup_SelectedIndexChanging" OnUpdateCommand="gvBudgetGroup_UpdateCommand" >
                         <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="true"
                    AutoGenerateColumns="false"  TableLayout="Fixed">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                          <Columns>
                            <telerik:gridtemplatecolumn HeaderStyle-Width="150px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCode" runat="server" Text="Code"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetGroupCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblBudgetGroupCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></asp:Label>
                                </EditItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn HeaderStyle-Width="495px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                  <asp:LinkButton ID="lblBudgetNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDHARDNAME"
                                        >Name&nbsp;</asp:LinkButton>
                                    <img id="FLDHARDNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBudgetGroupMapId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPVESSELMAPID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblHardCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDCODE") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkBudgetGroupName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItem %>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblBudgetGroupMapIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPVESSELMAPID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblHardCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDCODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblBudgetGroupName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                </EditItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn HeaderStyle-Width="435px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblBudgetedExpense" runat="server" Text="Budgeted Expense"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDISBUDGETED"].ToString() == "0" ? "No" : "Yes"%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadCheckBox ID="ckbBudgeted" runat="server" />
                                </EditItemTemplate>
                            </telerik:gridtemplatecolumn>
                           <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle  />
                            <ItemStyle  Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        </Columns>
                       <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" ScrollHeight="425px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
                   
            
       
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
