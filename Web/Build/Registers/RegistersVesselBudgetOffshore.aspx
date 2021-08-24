<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselBudgetOffshore.aspx.cs"
    Inherits="RegistersVesselBudgetOffshore" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Budget</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersBudget" runat="server" submitdisabledcontrols="true">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        Vessel Budget

                    </div>
                </div>
                <div id="divFind" style="position: relative; z-index: +1;">
                    <eluc:TabStrip ID="MenuVesselList" runat="server" OnTabStripCommand="MenuVesselList_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    <table id="tblConfigureBudget" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtVessel" runat="server" MaxLength="100" ReadOnly="true" CssClass="readonlytextbox"
                                    Width="260px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <b>
                    <telerik:RadLabel ID="lblRev" runat="server" Text="Revision"></telerik:RadLabel>
                </b>

                <eluc:TabStrip ID="MenuRegistersBudget" runat="server" OnTabStripCommand="RegistersBudget_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: 2">
                    <%-- <asp:GridView ID="gvBudgetRevision" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="true" OnRowCommand="gvBudgetRevision_RowCommand"
                        OnRowEditing="gvBudgetRevision_RowEditing" OnRowCancelingEdit="gvBudgetRevision_RowCancelingEdit"
                        OnRowUpdating="gvBudgetRevision_RowUpdating" ShowHeader="true" EnableViewState="false"
                        DataKeyNames="FLDREVISIONID" OnRowDataBound="gvBudgetRevision_RowDataBound" OnRowDeleting="gvBudgetRevision_OnRowDeleting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvBudgetRevision" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvBudgetRevision_NeedDataSource"
                        OnItemCommand="gvBudgetRevision_ItemCommand"
                        OnItemDataBound="gvBudgetRevision_ItemDataBound"
                        OnPreRender="gvBudgetRevision_PreRender"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false" ShowFooter="true">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDREVISIONID">
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <HeaderStyle Width="102px" />
                            <Columns>

                                <telerik:GridTemplateColumn HeaderText="Effective Date" HeaderStyle-Width="100px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEffectiveDate" CommandName="SELECTREVISION" CommandArgument="<%# Container.DataSetIndex %>"
                                            runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE")) %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date ID="ucEffectiveDateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE")) %>'
                                            CssClass="input_mandatory" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Date ID="ucEffectiveDateAdd" runat="server" CssClass="input_mandatory" Width="100%" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Currency" HeaderStyle-Width="60px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE")) %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Currency Width="100%" ID="ucCurrencyEdit" runat="server" SelectedCurrency='<%# (DataBinder.Eval(Container,"DataItem.FLDCURRENCY")) %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Currency ID="ucCurrencyAdd" runat="server" Width="100%" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Overlap Wage" HeaderStyle-Width="60px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <FooterStyle HorizontalAlign="Right" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblOverlap" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERLAPWAGE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="ucOverlapWageEdit" runat="server" DecimalPlace="2"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERLAPWAGE") %>' Width="100%" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Number ID="ucOverlapWageAdd" runat="server" Width="100%" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Tank Clean Allowance" HeaderStyle-Width="60px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <FooterStyle HorizontalAlign="Right" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTankCleanAllowance" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKCLEANALLOWANCE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="ucTankCleanAllowanceEdit" runat="server" DecimalPlace="2"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKCLEANALLOWANCE") %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Number ID="ucTankCleanAllowanceAdd" runat="server" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="DP Allowance" HeaderStyle-Width="60px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <FooterStyle HorizontalAlign="Right" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDPAllowance" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDPALLOWANCE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="ucDPAllowanceEdit" runat="server" DecimalPlace="2"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDDPALLOWANCE") %>' Width="100%" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Number ID="ucDPAllowanceAdd" runat="server" Width="100%" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Other Allowance" HeaderStyle-Width="60px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <FooterStyle HorizontalAlign="Right" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblOtherAllowance" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERALLOWANCE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="ucOtherAllowanceEdit" runat="server" DecimalPlace="2"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERALLOWANCE") %>' Width="100%" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Number ID="ucOtherAllowanceAdd" runat="server" Width="100%" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="250px">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRemrks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtRemarksEdit" runat="server" TextMode="MultiLine" Rows="2" Width="100%"
                                            CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'>
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtRemarksAdd" runat="server" TextMode="MultiLine" Rows="2" Width="100%"
                                            CssClass="gridinput">
                                        </telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Revision No" HeaderStyle-Width="50px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRevNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISIONNO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="100px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                            ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Copy Budget from previous revision"
                                            CommandName="COPY" CommandArgument="<%# Container.DataSetIndex %>"
                                            ID="cmdCopy" ToolTip="Copy Budget from previous revision">
                                             <span class="icon"><i class="fas fa-copy"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="UPDATE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                            ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                            ToolTip="Add New">
                                            <span class="icon"><i class="fa fa-plus-circle"></i></i></span>

                                        </asp:LinkButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>

                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>

                <br />
                <b>
                    <telerik:RadLabel ID="lblBudget" runat="server" Text="Budget"></telerik:RadLabel>
                </b>

                <eluc:TabStrip ID="MenuRegistersVesslBudget" runat="server" OnTabStripCommand="RegistersVesselBudget_TabStripCommand"></eluc:TabStrip>

                <div id="divGridBudget" style="position: relative; z-index: 0">
                    <%-- <asp:GridView ID="gvBudget" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="true" OnRowCommand="gvBudget_RowCommand"
                        ShowHeader="true" EnableViewState="false" DataKeyNames="FLDBUDGETID" OnRowDataBound="gvBudget_RowDataBound">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvBudget" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvBudget_NeedDataSource"
                        OnItemCommand="gvBudget_ItemCommand"
                        OnItemDataBound="gvBudget_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDBUDGETID">
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <HeaderStyle Width="102px" />
                            <Columns>

                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblRankHeader" runat="server" Text="Rank"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRank" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblCurrencyHeader" runat="server" Text="Currency"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE")) %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblWageHeader" runat="server" Text="Budgeted Wage/day"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblWage" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETEDWAGE") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblOverlapWageHeader" runat="server" Text="Overlap Wage/day"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblOverlap" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERLAPWAGE") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblOtherAllowanceHeader" runat="server" Text="Other allowance/day"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblOtherAllowance" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERALLOWANCE") %>'></telerik:RadLabel>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="400px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblNationalityHeader" runat="server" Text="Preferred Nationality"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblNationality" Width="400px" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDPREFERREDNATIONALITYNAME").ToString().Length>50 ? DataBinder.Eval(Container, "DataItem.FLDPREFERREDNATIONALITYNAME").ToString().Substring(0, 50)+ "..." : DataBinder.Eval(Container, "DataItem.FLDPREFERREDNATIONALITYNAME").ToString()) %>'></telerik:RadLabel>
                                        <eluc:Tooltip ID="ucNationalityToolTip" runat="server" Width="350px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREFERREDNATIONALITYNAME") %>' />
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit" 
                                            CommandName="EDITBUDGET" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit">
                                             <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                      
                                        <asp:LinkButton runat="server" AlternateText="Delete" 
                                            CommandName="DELETEBUDGET" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                            ToolTip="Delete">
                                             <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>

                                </telerik:GridTemplateColumn>
                            </Columns>

                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>

            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
