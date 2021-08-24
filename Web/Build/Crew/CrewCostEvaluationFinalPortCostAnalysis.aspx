<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCostEvaluationFinalPortCostAnalysis.aspx.cs"
    Inherits="CrewCostEvaluationFinalPortCostAnalysis" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCostQuoteLineItems" runat="server">
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmCrewTravelVisaLineItem" DecoratedControls="All" />
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuCrewCost" runat="server" OnTabStripCommand="MenuCrewCost_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPONumber" runat="server" Text="Request No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRequestNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCrewChangeVessel" runat="server" Text="Crew Change Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCrewChangeDate" runat="server" Text="Crew Change Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtCrewChangeDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPortName" runat="server" Text="Port Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPortName" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblETA" runat="server" Text="ETA"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtETA" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblETD" runat="server" Text="ETD"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtETD" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblQuotationNo" runat="server" Text="Quotation No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtQtnNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNumberofOnSigners" runat="server" Text="Number of On-Signers"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOnSigners" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNumberofOffSigners" runat="server" Text="Number of Off-Signers"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOffSigners" runat="server" CssClass="readonlytextbox" Width="80%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAnalysis" runat="server" EnableViewState="false" Height="85%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvAnalysis_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvAnalysis_ItemDataBound"
                OnItemCommand="gvAnalysis_ItemCommand" OnUpdateCommand="gvAnalysis_UpdateCommand" ShowFooter="true"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
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
                        <telerik:GridTemplateColumn HeaderText="Section Type" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLineItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTELINEITEMID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblQuoteId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblSectionTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Section" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Right" Wrap="False" />
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblTotal" runat="server" Font-Bold="true" Text="Total Amount"></telerik:RadLabel>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" UniqueName="AMOUNT" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterStyle HorizontalAlign="Right" Wrap="False" />
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblTotalA" runat="server" Font-Bold="true" Text=""></telerik:RadLabel>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Actual Amount" UniqueName="ACTUAL" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="10%">
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActualAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblLineItemIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTELINEITEMID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <eluc:MaskNumber ID="txtActualAmountEdit" runat="server" DecimalPlace="2" CssClass="input_mandatory"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALAMOUNT") %>' />
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Right" Wrap="False" />
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblTotalActual" runat="server" Font-Bold="true" Text=""></telerik:RadLabel>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="9%">
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListTaxBudgetEdit">
                                    <telerik:RadTextBox ID="txtBudgetCodeEdit" ReadOnly="true" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                        MaxLength="20" CssClass="input_mandatory" Width="80%">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="0px" Enabled="False"></telerik:RadTextBox>
                                    <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top" CommandArgument="<%# Container.DataSetIndex %>"
                                        ID="btnShowBudgetEdit" ToolTip="Select Budget Code">                                       
                                    <span class="icon"><i class="fas fa-list-alt"></i></span>
                                    </asp:LinkButton>

                                    <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Width="0px" Text='<%#DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Check" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCheckedYN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCHECKEDYN").ToString() == "1" ? "Yes" : "No"%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkCheckEdit" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDCHECKEDYN").ToString() == "1" ? true : false%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCheckingRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHECKINGREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtCheckingRemarksEdit" TextMode="MultiLine" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHECKINGREMARKS") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="" HeaderStyle-Width="7%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
