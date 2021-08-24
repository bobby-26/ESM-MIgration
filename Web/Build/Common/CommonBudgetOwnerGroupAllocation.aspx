<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonBudgetOwnerGroupAllocation.aspx.cs" Inherits="CommonBudgetOwnerGroupAllocation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Budget Group Allocation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCommonBudgetGroupAllocation" runat="server" submitdisabledcontrols="true">
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
       <telerik:RadAjaxPanel runat="server" ID="pnlCommonBudgetGroupAllocation" EnableAJAX="false">
       
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden"/>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Title runat="server" ID="ucTitle" Text="Budget Allocation" Visible="false"></eluc:Title>
        <eluc:TabStrip ID="MenuBudgetTab" runat="server" TabStrip="true" OnTabStripCommand="BudgetTab_TabStripCommand"></eluc:TabStrip>
        <table id="tblBudgetGroupAllocationSearch" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFinancialYear" runat="server" Text="Financial Year"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick runat="server" ID="ucFinancialYear" QuickTypeCode="55" CssClass="dropdown_mandatory"
                        AutoPostBack="true" AppendDataBoundItems="true" OnTextChangedEvent="FinancialYear_Changed" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtFromDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtToDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                </td>
            </tr>
        </table>

        <br />
        <br />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselAllocation" runat="server" AutoGenerateColumns="False" OnDeleteCommand="gvVesselAllocation_DeleteCommand"
            Font-Size="11px" GridLines="None" Width="100%" CellPadding="3" OnItemCommand="gvVesselAllocation_ItemCommand" OnNeedDataSource="gvVesselAllocation_NeedDataSource"
            OnItemDataBound="gvVesselAllocation_ItemDataBound" EnableHeaderContextMenu="true" GroupingEnabled="false" AllowSorting="true" Height="150px"
            ShowFooter="false" ShowHeader="true" OnSelectedIndexChanging="gvVesselAllocation_SelectedIndexChanging" EnableViewState="false" DataKeyNames="FLDVESSELBUDGETALLOCATIONID">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
              <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

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
                    <telerik:GridTemplateColumn HeaderText="Rev.No" HeaderStyle-Width="60px">
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <%#(Container.ItemIndex + 1) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselAllocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELBUDGETALLOCATIONID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblOwnerId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblFinancialYearId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALYEAR") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkVessel" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItem %>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                            <telerik:RadLabel ID="lbllatest" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLATEST") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblVesselAllocationIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELBUDGETALLOCATIONID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblOwnerIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblFinancialYear" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALYEAR") %>'></telerik:RadLabel>
                            <telerik:RadLabel runat="server" ID="lblVesselNameEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAccountIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Account" HeaderStyle-Width="235px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%--<telerik:GridTemplateColumn HeaderText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAmountHeader" runat="server">Amount&nbsp;
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox runat="server" ID="txtBudgetAmountEdit" style="text-align: right" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' ></telerik:RadTextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskNumber" runat="server" TargetControlID="txtBudgetAmountEdit"
                                    Mask="9,999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                </ajaxToolkit:MaskedEditExtender>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                    <telerik:GridTemplateColumn HeaderText="Budget Amount">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Effective Date" HeaderStyle-Width="150px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE", "{0:dd/MM/yyyy}" ) %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Date runat="server" ID="ucDate" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE" ) %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Applied Period">
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblappliedperiod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPLIEDPERIODS") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Over written" HeaderStyle-Width="98px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOverwritten" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERWRITTEN" ) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText=" Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" Visible="true" ID="cmdEdit" ToolTip="Edit">
                                 <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" Visible="true" ID="cmdDelete" ToolTip="Delete Revision">
                                  <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="cmdBudgetBreakDown" runat="server" AlternateText="Budget Breakdown" CommandName="BUDGETBREAKDOWN" ToolTip="Budget Breakdown">
                                                            <span class="icon"><i class="fas fa-list-alt"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAttachment" ToolTip="View Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="SAVE" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
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
        <br />
        <br />
        <br />
        <table id="tblBudgetGroupLevel" width="50%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblGroupLevel" runat="server" Text="Group Level"></telerik:RadLabel>
                </td>
                <td>
                     <telerik:RadDropDownList ID="ddlOwnerBudgetGroupLevel" runat="server" CssClass="input" Width="120px" OnSelectedIndexChanged = "ddlOwnerBudgetGroupLevel_Changed" AutoPostBack="true">
                           
                        </telerik:RadDropDownList>
                    <%--<telerik:RadComboBox ID="ddlOwnerBudgetGroupLevel" runat="server" Width="120px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"
                        OnSelectedIndexChanged="ddlOwnerBudgetGroupLevel_Changed" AutoPostBack="true">
                    </telerik:RadComboBox>--%>
                </td>
                <td>
                     <telerik:RadLabel ID="txtlevelcount" runat="server" Width="250px" Enabled="true"></telerik:RadLabel>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuCommonBudgetGroupAllocation" runat="server" OnTabStripCommand="CommonBudgetGroupAllocation_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvBudgetGroupAllocation" runat="server" AutoGenerateColumns="False" OnNeedDataSource="gvBudgetGroupAllocation_NeedDataSource"
            Font-Size="11px" GridLines="None" Width="100%" CellPadding="3" OnItemCommand="gvBudgetGroupAllocation_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnItemDataBound="gvBudgetGroupAllocation_ItemDataBound" AllowPaging="true" AllowCustomPaging="true" Height="250px"
            OnSorting="gvBudgetGroupAllocation_Sorting" AllowSorting="true"
            ShowFooter="false" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDOWNERBUDGETGROUPID">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
              <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

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
                    <telerik:GridTemplateColumn HeaderText="Immediate Parent">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblParentBudgetGroupAllocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPALLOCATIONID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblParentBudgetGroupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLPARENTDOWNERBUDGETGROUPID") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkParentBudgetGroup" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItem %>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLPARENTDOWNERBUDGETGROUP") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>


                    <telerik:GridTemplateColumn HeaderText="Owner Budget Code">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBudgetGroupAllocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPALLOCATIONID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselbudgetAllocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELBUDGETALLOCATIONID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblBudgetGroupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUPID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblBudgetGroup" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkBudgetGroup" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItem %>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Owner Code Description">
                        <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbldescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Budget Amount">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Allowance">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAllowance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOWANCE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Access">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAccess" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCESSNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Apportionment">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblApportionment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPORTIONMENTMETHODNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="80px">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                 <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete Revision">
                                 <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
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
        <%--        <div id="divPage" style="position: relative;">
            <table width="100%" border="0" class="datagrid_pagestyle">
                <tr>
                    <td nowrap align="center">
                        <telerik:RadLabel ID="lblPagenumber" runat="server">
                        </telerik:RadLabel>
                        <telerik:RadLabel ID="lblPages" runat="server">
                        </telerik:RadLabel>
                        <telerik:RadLabel ID="lblRecords" runat="server">
                        </telerik:RadLabel>&nbsp;&nbsp;
                    </td>
                    <td nowrap align="left" width="50px">
                        <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                    </td>
                    <td width="20px">&nbsp;
                    </td>
                    <td nowrap align="right" width="50px">
                        <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                    </td>
                    <td nowrap align="center">
                        <telerik:RadTextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                        </telerik:RadTextBox>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                            Width="40px"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>--%>
        <br />
        <br />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvBudgetPeriodAllocation" runat="server" AutoGenerateColumns="False" GroupingEnabled="false" EnableHeaderContextMenu="true"
            Font-Size="11px" Width="100%" CellPadding="3" OnNeedDataSource="gvBudgetPeriodAllocation_NeedDataSource"
            OnItemDataBound="gvBudgetPeriodAllocation_ItemDataBound" AllowSorting="true"
            OnRowCreated="gvBudgetPeriodAllocation_RowCreated" ShowFooter="false" ShowHeader="true" EnableViewState="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
           <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Monthly Totals" HeaderStyle-HorizontalAlign="Center" Name="Monthlytotals">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Accumulated" HeaderStyle-HorizontalAlign="Center" Name="Accumulated">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>

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
                    <telerik:GridTemplateColumn HeaderText="Period" ColumnGroupName="Monthlytotals">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDBUDGETGROUPNAME"
                                ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                            <telerik:RadLabel ID="lblPeriodHeader" runat="server">
                                Period&nbsp;
                            </telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPeriod" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERIOD") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblYear" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblBudgetGroupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPID") %>'></telerik:RadLabel>

                            <telerik:RadLabel ID="lblPeriodName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERIODNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Committed" ColumnGroupName="Monthlytotals">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lblCommittedAmount" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItem %>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:LinkButton>
                            <telerik:RadLabel ID="lblCommitmentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCOMMITMENTID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Charged" ColumnGroupName="Monthlytotals">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lblPaidAmount" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItem %>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAIDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Total" ColumnGroupName="Monthlytotals">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTotalAmount" runat="server"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALEXPENDITURE", "{0:##,###,###,##0.00}") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Budgeted" ColumnGroupName="Monthlytotals">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Monthly Variance" ColumnGroupName="Monthlytotals">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMonthlyVariance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTHLYVARIANCE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Total" ColumnGroupName="Accumulated">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAccumulatedTotalAmount" runat="server" Text=''></telerik:RadLabel>
                            <%--   <%=accumulatedtotal %> --%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Budget" ColumnGroupName="Accumulated">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBudgetedTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDBUDGET", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Allowance" ColumnGroupName="Accumulated">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAllowanceTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOWANCETOTAL", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Variance" ColumnGroupName="Accumulated">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVariance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Mngt Variance" ColumnGroupName="Accumulated">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMngtVariance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANAGEMENTVARIANCE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
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
        <br />
        <br />
        <br />
    </telerik:RadAjaxPanel>
    </form>
 
</body>
</html>
