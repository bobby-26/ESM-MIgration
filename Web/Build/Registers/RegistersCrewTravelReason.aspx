<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCrewTravelReason.aspx.cs" Inherits="RegistersCrewTravelReason" %>


<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Travel Reason</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersTravelreason" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTitle" runat="server" Title="Travel Reason"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblConfigureCity" width="50%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTravelReason" runat="server" Text="Travel Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="200" CssClass="input" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersCity" runat="server" OnTabStripCommand="RegistersCity_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTravelreason" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvTravelreason_ItemCommand" OnNeedDataSource="gvTravelreason_NeedDataSource" Height="85%"
                OnItemDataBound="gvTravelreason_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
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
                        <telerik:GridTemplateColumn HeaderStyle-Width="500px" HeaderText="Travel Reason" AllowSorting="true" SortExpression="FLDREASON">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblreasonid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELREASONID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkreason" runat="server" CommandName="EDIT" 
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblreasonidedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELREASONID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtreasonedit" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'
                                    CssClass="gridinput_mandatory" MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtreasonadd" runat="server" CssClass="gridinput_mandatory" Width="98%" MaxLength="200"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="250px" HeaderText="Off-signers Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListBudgetEdit">
                                    <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                        MaxLength="20" CssClass="input_mandatory" Width="80%">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".."
                                        OnClientClick="return showPickList('spnPickListBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudget.aspx', true);" />
                                    <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="hidden" Enabled="False"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListBudget">
                                    <telerik:RadTextBox ID="txtBudgetCode" runat="server"
                                        MaxLength="20" CssClass="input_mandatory" Width="80%">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".."
                                        OnClientClick="return showPickList('spnPickListBudget', 'codehelp1', '', '../Common/CommonPickListBudget.aspx', true);" />
                                    <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="0px" CssClass="hidden" Enabled="False"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="250px" HeaderText="On-signers Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOnSignerBudgetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERSUBACCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOnBudgetEdit">
                                    <telerik:RadTextBox ID="txtOnBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERSUBACCOUNT") %>'
                                        MaxLength="20" CssClass="input_mandatory" Width="80%">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnOnShowBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".."
                                        OnClientClick="return showPickList('spnPickListOnBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudget.aspx', true);" />
                                    <telerik:RadTextBox ID="txtOnBudgetNameEdit" runat="server" Width="0px" CssClass="hidden" Enabled="False"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOnBudgetIdEdit" runat="server" Width="0px" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERBUDGETID") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOnBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListOnBudget">
                                    <telerik:RadTextBox ID="txtOnBudgetCode" runat="server" MaxLength="20" CssClass="input_mandatory"
                                        Width="80%">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnOnShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." 
                                        OnClientClick="return showPickList('spnPickListOnBudget', 'codehelp1', '', '../Common/CommonPickListBudget.aspx', true);" />
                                    <telerik:RadTextBox ID="txtOnBudgetName" runat="server" Width="0px" CssClass="hidden" Enabled="False"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOnBudgetId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOnBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" HeaderText="Action">                         
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" ToolTip="Add New" Width="20PX" Height="20PX"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>

</html>

