<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerBudgetCrewExpense.aspx.cs"
    Inherits="OwnerBudgetCrewExpense" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.OwnerBudget" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OfficerType" Src="~/UserControls/UserControlOfficerType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ExpenseType" Src="~/UserControls/UserControlExpenseType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Expense</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCountry" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Title runat="server" ID="ucTitle" Text="Crew Expense" Visible="false"></eluc:Title>
        <eluc:TabStrip ID="MenuCrewExpense" runat="server" TabStrip="true" OnTabStripCommand="MenuCrewExpense_TabStripCommand" />
        <eluc:TabStrip ID="MenuShowExpenses" runat="server" OnTabStripCommand="MenuShowExpenses_TabStripCommand" />
        <b>
            <br />
            <telerik:RadLabel ID="lblCrewExpense" runat="server" Text="CrewExpense"></telerik:RadLabel>
        </b>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewExpense" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            OnRowCreated="gvCrewExpense_RowCreated" Width="100%" CellPadding="3" OnItemCommand="gvCrewExpense_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnItemDataBound="gvCrewExpense_ItemDataBound" OnNeedDataSource="gvCrewExpense_NeedDataSource"
            OnRowUpdating="gvCrewExpense_RowUpdating"
            OnRowEditing="gvCrewExpense_RowEditing" ShowFooter="false" ShowHeader="true" EnableViewState="false"
            AllowSorting="true" OnSorting="gvCrewExpense_Sorting">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <NoRecordsTemplate>
                    <table width="99.9%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <HeaderStyle Width="102px" />
                <Columns>
                    <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                    <telerik:GridTemplateColumn HeaderText="Expense">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDTkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCategoryId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblExpense" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <FooterTemplate>
                            <eluc:ExpenseType ID="ucCategoryAdd" AppendDataBoundItems="true"
                                runat="server" CssClass="gridinput_mandatory" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Level">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOfficeType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICERTYPE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <FooterTemplate>
                            <eluc:Hard ID="ucOfficerTypeAdd" CssClass="input_mandatory" AppendDataBoundItems="true"
                                runat="server" HardTypeCode="90" ShortNameFilter="SOF,JOF,TRA,RAT" />
                            <%--<asp:DropDownList ID="ucOfficerTypeAdd" runat="server" CssClass="gridinput_mandatory">
                                     <asp:ListItem Text="--Select--" Value="Dummy"></asp:ListItem>
                                        <asp:ListItem Text="Senior Officer" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Junior Officer" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Ratings" Value="3"></asp:ListItem>
                                     </asp:DropDownList>   --%>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Contract Period">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblContractPeriod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTPERIOD") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="ucContractPeriodEdit" DecimalPlace="0" CssClass="input_mandatory"
                                runat="server" IsInteger="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTPERIOD") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="No. Of Crew">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblManRequired" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANREQUIRED") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="ucManRequiredEdit" DecimalPlace="0" IsInteger="true" CssClass="input_mandatory"
                                runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANREQUIRED") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Amount Per Man">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAmountMan" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTPERMAN")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Frequency">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFrequency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCY")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Amount Per Month">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAmountMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTPERMONTH")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Amount Per Year">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblActionHeader1" runat="server">
                                <telerik:RadLabel ID="lblAction" runat="server" Text="Action"></telerik:RadLabel>
                            </telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="DELETE" CommandArgument="<%# Container.DataItem %>" ID="cmdDelete"
                                ToolTip="Delete"></asp:ImageButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                CommandName="Update" CommandArgument="<%# Container.DataItem %>" ID="cmdSave"
                                ToolTip="Save"></asp:ImageButton>
                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="Cancel" CommandArgument="<%# Container.DataItem %>" ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                CommandName="Add" CommandArgument="<%# Container.DataItem %>" ID="cmdAdd"
                                ToolTip="Add New"></asp:ImageButton>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <b>
            <br />
            <telerik:RadLabel ID="lblTravelExpense" runat="server" Text="Travel Expense"></telerik:RadLabel>
        </b>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvTravelExpense" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            OnRowCreated="gvTravelExpense_RowCreated" Width="100%" CellPadding="3" OnItemCommand="gvTravelExpense_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnItemDataBound="gvTravelExpense_ItemDataBound" OnRowCancelingEdit="gvTravelExpense_RowCancelingEdit" OnNeedDataSource="gvTravelExpense_NeedDataSource"
            OnRowUpdating="gvTravelExpense_RowUpdating" ShowFooter="false" ShowHeader="true"
            EnableViewState="false" AllowSorting="true" OnSorting="gvTravelExpense_Sorting">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <NoRecordsTemplate>
                    <table width="99.9%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <HeaderStyle Width="102px" />
                <Columns>
                    <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                    <telerik:GridTemplateColumn HeaderText="Port of Rotation">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDTkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblExpense" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTOFROTATION") %>'></telerik:RadLabel>
                            <%--<telerik:RadLabel ID="lblCategoryId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></telerik:RadLabel>--%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <eluc:ExpenseType ID="ucCategoryAdd" AppendDataBoundItems="true" ExpenseType="2"
                                runat="server" CssClass="gridinput_mandatory" AutoPostBack="true" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Level">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOfficeType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICERTYPE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <FooterTemplate>
                            <eluc:Hard ID="ucOfficerTypeAdd" CssClass="input_mandatory" AppendDataBoundItems="true"
                                runat="server" HardTypeCode="90" ShortNameFilter="SOF,JOF,TRA,RAT" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Contract Period">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblContractPeriod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTPERIOD") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="ucContractPeriodEdit" DecimalPlace="0" CssClass="input_mandatory"
                                runat="server" IsInteger="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTPERIOD") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="No. Of Crew">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblManRequired" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANREQUIRED") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="ucManRequiredEdit" DecimalPlace="0" IsInteger="true" CssClass="input_mandatory"
                                runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANREQUIRED") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Airfare Per Man">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAirfareMan" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRFAREPERMAN")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Agency Fee Per Man">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAgencyMan" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENCYFEEPERMAN")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Airfare Per Month">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAmountMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTPERMONTH")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Airfare Per Year">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Agency Fee Per Month">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAgencyFeeMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENCYFEEPERMONTH")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Agency Fee Per Year">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAgencyFeeYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENCYFEE")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblActionHeader1" runat="server">
                                <telerik:RadLabel ID="lblAction" runat="server" Text="Action"></telerik:RadLabel>
                            </telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="DELETE" CommandArgument="<%# Container.DataItem %>" ID="cmdDelete"
                                ToolTip="Delete" Visible="false"></asp:ImageButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                CommandName="Update" CommandArgument="<%# Container.DataItem %>" ID="cmdSave"
                                ToolTip="Save"></asp:ImageButton>
                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="Cancel" CommandArgument="<%# Container.DataItem %>" ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                CommandName="Add" CommandArgument="<%# Container.DataItem %>" ID="cmdAdd"
                                ToolTip="Add New"></asp:ImageButton>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <b>
            <br />
            <telerik:RadLabel ID="lblOtherExpense" runat="server" Text="Other Expense"></telerik:RadLabel>
        </b>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvOtherCrewExpense" runat="server" AutoGenerateColumns="False"
            Font-Size="11px" OnRowCreated="gvOtherCrewExpense_RowCreated" Width="100%" CellPadding="3" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnItemCommand="gvOtherCrewExpense_ItemCommand" OnItemDataBound="gvOtherCrewExpense_ItemDataBound" OnNeedDataSource="gvOtherCrewExpense_NeedDataSource"
            OnRowUpdating="gvOtherCrewExpense_RowUpdating" OnRowEditing="gvOtherCrewExpense_RowEditing"
            ShowFooter="false" ShowHeader="true" EnableViewState="false" AllowSorting="true"
            OnSorting="gvOtherCrewExpense_Sorting">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <NoRecordsTemplate>
                    <table width="99.9%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <HeaderStyle Width="102px" />
                <Columns>
                    <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                    <telerik:GridTemplateColumn HeaderText="Expense">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDTkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblExpense" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblCategoryId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <FooterTemplate>
                            <eluc:ExpenseType ID="ucCategoryAdd" AppendDataBoundItems="true" ExpenseType="3"
                                runat="server" CssClass="gridinput_mandatory" AutoPostBack="true" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Level">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOfficeType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICERTYPE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <FooterTemplate>
                            <eluc:Hard ID="ucOfficerTypeAdd" CssClass="input_mandatory" AppendDataBoundItems="true"
                                runat="server" HardTypeCode="90" ShortNameFilter="SOF,JOF,TRA,RAT" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Contract Period">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblContractPeriod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTPERIOD") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="ucContractPeriodEdit" DecimalPlace="0" CssClass="input_mandatory"
                                runat="server" IsInteger="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTPERIOD") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="No. Of Crew">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblManRequired" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANREQUIRED") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="ucManRequiredEdit" DecimalPlace="0" IsInteger="true" CssClass="input_mandatory"
                                runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANREQUIRED") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Amount Per Man">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAmountMan" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTPERMAN")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Frequency">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFrequency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCY")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Amount Per Month">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAmountMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTPERMONTH")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Amount Per Year">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblActionHeader1" runat="server">
                                <telerik:RadLabel ID="lblAction" runat="server" Text="Action"></telerik:RadLabel>
                            </telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="DELETE" CommandArgument="<%# Container.DataItem %>" ID="cmdDelete"
                                ToolTip="Delete"></asp:ImageButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                CommandName="Update" CommandArgument="<%# Container.DataItem %>" ID="cmdSave"
                                ToolTip="Save"></asp:ImageButton>
                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="Cancel" CommandArgument="<%# Container.DataItem %>" ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                CommandName="Add" CommandArgument="<%# Container.DataItem %>" ID="cmdAdd"
                                ToolTip="Add New"></asp:ImageButton>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <b>
            <br />
            <telerik:RadLabel ID="lblFlagExpense" runat="server" Text="Flag Expense"></telerik:RadLabel>
        </b>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvOtherExpenses" runat="server" AutoGenerateColumns="False"
            Font-Size="11px" Width="100%" CellPadding="3" GroupingEnabled="false" EnableHeaderContextMenu="true"
           OnItemDataBound="gvOtherExpenses_ItemDataBound" OnNeedDataSource="gvOtherExpenses_NeedDataSource"
            ShowFooter="false" ShowHeader="true" EnableViewState="false" AllowSorting="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <NoRecordsTemplate>
                    <table width="99.9%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <HeaderStyle Width="102px" />
                <Columns>
                    <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                    <telerik:GridTemplateColumn HeaderText="Expense">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblComponent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPENSES") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Level">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOfficeType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVEL") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="No. Of Crew">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblManRequired" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANREQUIRED") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Amount(Per Man Per Month)">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAmountMan" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTPERMAN") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Total Amount Per Month">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAmountMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTPERMONTH") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Total Amount Per Year">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAmountYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTPERYEAR")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
