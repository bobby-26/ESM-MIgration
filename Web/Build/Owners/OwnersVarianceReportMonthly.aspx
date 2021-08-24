<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersVarianceReportMonthly.aspx.cs"
    Inherits="OwnersVarianceReportMonthly" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Budget Group Allocation</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="divlink">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCommonBudgetGroupAllocation" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCommonBudgetGroupAllocation">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="ucTitle" Text="Variance Report"></eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBudgetTab" runat="server" OnTabStripCommand="BudgetTab_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table id="tblBudgetGroupAllocationSearch" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblAccountcode" runat="server" Text="Account Code/Description"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccountCode" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblStatementRef" runat="server" Text="Statement Reference"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStatementRef" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    <eluc:TabStrip ID="MenuCommonBudgetGroupAllocation" runat="server" OnTabStripCommand="CommonBudgetGroupAllocation_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <asp:GridView ID="gvBudgetGroupAllocation" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvBudgetGroupAllocation_RowCommand"
                    OnRowDataBound="gvBudgetGroupAllocation_RowDataBound" OnRowDeleting="gvBudgetGroupAllocation_RowDeleting"
                    OnSorting="gvBudgetGroupAllocation_Sorting" AllowSorting="true" OnRowEditing="gvBudgetGroupAllocation_RowEditing"
                    ShowFooter="false" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDBUDGETGROUPID">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Budget Group">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblBudgetGroupHeader" runat="server">Budget Group&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetGroupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPID") %>'></asp:Label>
                                <asp:LinkButton ID="lnkBudgetGroup" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUP") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblCommitted" runat="server" Text="Committed Amount"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCommitted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblBilledAmount" runat="server" Text="Billed Amount"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBilledAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBILLEDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblBudgetAmount" runat="server" Text="Budget Amount"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETPERMONTH", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblVarianceAmount" runat="server" Text="Variance"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVarianceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCE", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblVariancePercentage" runat="server" Text="Variance %"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVariancePercentage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCEPERCENTAGE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblRemarks" runat="server" Text="YTD Remarks"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYTDREMARKS") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuApprovedPO" runat="server" OnTabStripCommand="MenuApprovedPO_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <b><asp:Literal ID="lblOutStandingPO" runat="server" Text="Outstanding PO"></asp:Literal></b>
                <br />
                <asp:GridView ID="gvBudgetPeriodAllocation" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvBudgetPeriodAllocation_RowCommand"
                    OnRowDataBound="gvBudgetPeriodAllocation_RowDataBound" OnRowEditing="gvBudgetPeriodAllocation_RowEditing"
                    OnRowCreated="gvBudgetPeriodAllocation_RowCreated" ShowFooter="false" ShowHeader="true"
                    EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Period">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblPONumber" runat="server" Text="PO Number"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetGroupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUP") %>'></asp:Label>
                                <asp:Label ID="lblOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></asp:Label>
                                <asp:Label ID="lblPONumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPONUMBER") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Period">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblSupplierName" runat="server" Text="Supplier Name"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Committed">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAmountUSD" runat="server" Text="Amount(USD)"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblCommittedDate" runat="server" Text="Committed Date"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCommittedDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMMITTEDDATE")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reversed Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblReversedDate" runat="server" Text="Reversed Date"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblReversedDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDATEOFREVERSAL")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Voucher Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblVoucherNumber" runat="server" Text="Voucher Number"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVOUCHERNUMBER")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Statement Reference">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblStatementRef" runat="server" Text="Statement Reference"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStatementRef" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSTATEMENTREF")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClickApprovedPO"
                                    CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClickApprovedPO" runat="server"
                                    CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_ClickApprovedPO" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuCommittedPO" runat="server" OnTabStripCommand="MenuCommittedPO_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <b><asp:Literal ID="lblBilledVouchers" runat="server" Text="Billed Vouchers"></asp:Literal></b>
                <br />
                <asp:GridView ID="gvCommittedPO" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvCommittedPO_RowCommand" OnRowDataBound="gvCommittedPO_RowDataBound"
                    OnRowEditing="gvCommittedPO_RowEditing" OnRowCreated="gvCommittedPO_RowCreated"
                    ShowFooter="false" ShowHeader="true" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Period">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblStatementRef" runat="server" Text="Statement Reference"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetGroupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUP") %>'></asp:Label>
                                <asp:Label ID="lblStatementRef" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATEMENTREFERENCE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Committed">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblVoucherRow" runat="server" Text="Voucher Row"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVoucherRow" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERROW") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Committed">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAmountUSD" runat="server" Text="Amount(USD)"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBUDGETCODE")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblBudgetGroup" runat="server" Text="Budget Group"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetGroup" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBUDGETGROUP")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblParticulars" runat="server" Text="Particulars"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblParticulars" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPARTICULARS")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div id="div1" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumberCommittedPO" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPagesCommittedPO" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecordsCommittedPO" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPreviousCommittedPO" runat="server" OnCommand="PagerButtonClickCommittedPO"
                                    CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNextCommittedPO" OnCommand="PagerButtonClickCommittedPO" runat="server"
                                    CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopageCommittedPO" MaxLength="3" Width="20px" runat="server"
                                    CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGoCommittedPO" runat="server" Text="Go" OnClick="cmdGo_ClickCommittedPO"
                                    CssClass="input" Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
