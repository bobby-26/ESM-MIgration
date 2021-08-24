<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonBudgetRemainingBalance.aspx.cs"
    Inherits="CommonBudgetRemainingBalance" %>

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
                    <eluc:Title runat="server" ID="ucTitle" Text="Remaining Budget"></eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBudgetTab" runat="server" OnTabStripCommand="BudgetTab_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table id="tblBudgetGroupAllocationSearch" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblVesselAccount" runat="server" Text="Vessel Account"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ucVesselAccount" CssClass="dropdown_mandatory"
                                AppendDataBoundItems="true">
                                <asp:ListItem Text="--Select--" Value="" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblYeartoDateBudgetRemainingasOn" runat="server" Text="Year to Date Budget Remaining as On"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Quick runat="server" ID="ucFinancialYear" QuickTypeCode="55" CssClass="dropdown_mandatory"
                                AppendDataBoundItems="true" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="dropdown_mandatory">
                                <asp:ListItem Value="" Text="--Select--"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Jan"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Feb"></asp:ListItem>
                                <asp:ListItem Value="3" Text="Mar"></asp:ListItem>
                                <asp:ListItem Value="4" Text="Apr"></asp:ListItem>
                                <asp:ListItem Value="5" Text="May"></asp:ListItem>
                                <asp:ListItem Value="6" Text="Jun"></asp:ListItem>
                                <asp:ListItem Value="7" Text="Jul"></asp:ListItem>
                                <asp:ListItem Value="8" Text="Aug"></asp:ListItem>
                                <asp:ListItem Value="9" Text="Sep"></asp:ListItem>
                                <asp:ListItem Value="10" Text="Oct"></asp:ListItem>
                                <asp:ListItem Value="11" Text="Nov"></asp:ListItem>
                                <asp:ListItem Value="12" Text="Dec"></asp:ListItem>
                            </asp:DropDownList>
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
                <b><asp:Literal ID="lblBudgetAmountisBudgetAgreedWithOwnerlessAllowancegrantedbyManagement" runat="server" Text="Budget Amount is Budget Agreed With Owner less Allowance granted by Management"></asp:Literal></b>
                <br />
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
                                <asp:Literal ID="lblNotYetOrdered" runat="server" Text="Not Yet Ordered"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblNotYetOrdered" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYETTOORDERAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblCommitted" runat="server" Text="Committed"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCommitted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblActual" runat="server" Text="Total Charged"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblActual" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTUALAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
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
                                <asp:Literal ID="lblBudgetRemaining" runat="server" Text="Budget Remaining"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetRemaining" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMAININGBUDGET", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblActionHeader" runat="server">
                                Action
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/72.png %>"
                                    CommandName="INSERTREMARKS" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdRemarks"
                                    ToolTip="Insert Remarks"></asp:ImageButton>
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
                <b><asp:Literal ID="lblApprovedQuotationOrderNotPlaced" runat="server" Text="Approved Quotation Order Not Placed"></asp:Literal></b>
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
                                <asp:Label ID="lblPONumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Committed">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblApprovedAmountUSD" runat="server" Text="Approved Amount(USD)"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBACCOUNT")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblItemDescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>'></asp:Label>
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
                <b><asp:Literal ID="lblOutstandingPOs" runat="server" Text="Outstanding POs"></asp:Literal></b>
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
                                <asp:Literal ID="lblPONumber" runat="server" Text="PO Number"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetGroupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUP") %>'></asp:Label>
                                <asp:Label ID="lblOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></asp:Label>
                                <asp:Label ID="lblPONumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Committed">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblCommittedAmountUSD" runat="server" Text="Committed Amount(USD)"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblApprovedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBACCOUNT")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblItemDescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>'></asp:Label>
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
                <br />
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuActualPO" runat="server" OnTabStripCommand="MenuActualPO_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <b><asp:Literal ID="lblActualExpenses" runat="server" Text="Total Charged"></asp:Literal></b>
                <br />
                <asp:GridView ID="gvActualPO" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvActualPO_RowCommand" OnRowDataBound="gvActualPO_RowDataBound"
                    OnRowEditing="gvActualPO_RowEditing" OnRowCreated="gvActualPO_RowCreated" ShowFooter="false"
                    ShowHeader="true" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Period">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblVoucherNumber" runat="server" Text="Voucher Number"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetGroupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUP") %>'></asp:Label>
                                <%--<asp:Label ID="lblVoucherId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></asp:Label>--%>
                                <asp:Label ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNO") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Committed">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblActualAmountUSD" runat="server" Text="Actual Amount(USD)"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblActualAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBACCOUNT")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblItemDescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div id="div2" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumberActualPO" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPagesActualPO" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecordsActualPO" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPreviousActualPO" runat="server" OnCommand="PagerButtonClickActualPO"
                                    CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNextActualPO" OnCommand="PagerButtonClickActualPO" runat="server"
                                    CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopageActualPO" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGoActualPO" runat="server" Text="Go" OnClick="cmdGo_ClickActualPO"
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
