<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonBudgetGroupAllocationViewOnly.aspx.cs" Inherits="CommonBudgetGroupAllocationViewOnly" %>

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
                    <eluc:Title runat="server" id="ucTitle" Text="Budget Allocation" ShowMenu="false"></eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuBudget" runat="server" OnTabStripCommand="MenuBudget_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                
                <table id="tblBudgetGroupAllocationSearch" width="100%">
                    <tr>  
                        <td>
                            <asp:Literal ID="lblFinancialYear" runat="server" Text="Financial Year"></asp:Literal>
                        </td>                          
                        <td>
                            <eluc:Quick runat="server" ID="ucFinancialYear" QuickTypeCode="55" CssClass="dropdown_mandatory" AutoPostBack="true" AppendDataBoundItems="true"
                                 OnTextChangedEvent="FinancialYear_Changed" />
                        </td> 
                    </tr>
                </table>
                
                <br />
                <br />
                
                <asp:GridView ID="gvVesselAllocation" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvVesselAllocation_RowCommand" OnRowDataBound="gvVesselAllocation_RowDataBound"
                    OnRowEditing="gvVesselAllocation_RowEditing" OnRowCancelingEdit="gvVesselAllocation_RowCancelingEdit" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                    
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />
                    
                    <Columns>  
                        <asp:TemplateField HeaderText="Vessel">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDBUDGETGROUPNAME"
                                    ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                <asp:Label ID="lblVesselHeader" runat="server">Vessel&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVesselAllocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELBUDGETALLOCATIONID") %>'></asp:Label>
                                <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                <asp:Label ID="lblOwnerId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERID") %>'></asp:Label>
                                <asp:Label ID="lblFinancialYearId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALYEAR") %>'></asp:Label>
                                <asp:LinkButton ID="lnkVessel" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField> 
                                                
                        <asp:TemplateField HeaderText="Account">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblAccountHeader" runat="server">Account&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></asp:Label>
                                <asp:Label ID="lblAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                                
                        <asp:TemplateField HeaderText="Budget Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblBudgetAmountHeader" runat="server">Amount&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                                                
                        <asp:TemplateField HeaderText="Period">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblPeriodHeader" runat="server">Effective Period&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPeriod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEPERIODNAME" ) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <%--<asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblActionHeader" runat="server">
                                Action
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="History" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png %>"
                                    CommandName="History" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID")  %>' ID="cmdHistory"
                                    ToolTip="View History"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        
                    </Columns>
                </asp:GridView>
                
                <br />
                <br />                
                
                <div class="navSelect" style="position: relative; width: 15px">
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    <eluc:TabStrip ID="MenuCommonBudgetGroupAllocation" runat="server" OnTabStripCommand="CommonBudgetGroupAllocation_TabStripCommand">
                    </eluc:TabStrip>
                </div>    
                     
                <asp:GridView ID="gvBudgetGroupAllocation" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvBudgetGroupAllocation_RowCommand" OnRowDataBound="gvBudgetGroupAllocation_RowDataBound"
                    OnRowDeleting="gvBudgetGroupAllocation_RowDeleting" OnSorting="gvBudgetGroupAllocation_Sorting" AllowSorting="true"
                    OnRowEditing="gvBudgetGroupAllocation_RowEditing" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                    
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
                                <asp:Label ID="lblBudgetGroupAllocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPALLOCATIONID") %>'></asp:Label>
                                <asp:Label ID="lblBudgetGroupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPID") %>'></asp:Label>
                                <asp:LinkButton ID="lnkBudgetGroup" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField> 
                                                
                        <asp:TemplateField HeaderText="Budget Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblBudgetAmountHeader" runat="server">Budget Amount&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Access">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblAccessHeader" runat="server">
                                    Access
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAccess" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCESSNAME") %>'></asp:Label>
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
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                
                <br />
                <br />
                
                <asp:GridView ID="gvBudgetPeriodAllocation" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvBudgetPeriodAllocation_RowCommand" 
                    OnRowEditing="gvBudgetPeriodAllocation_RowEditing" OnRowCreated="gvBudgetPeriodAllocation_RowCreated"
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
                                <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDBUDGETGROUPNAME"
                                    ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                <asp:Label ID="lblPeriodHeader" runat="server">Period&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPeriod" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERIOD") %>'></asp:Label>
                                <asp:Label ID="lblPeriodName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERIODNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField> 
                         
                        <asp:TemplateField HeaderText="Committed">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblCommittedHeader" runat="server">Committed
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCommittedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField> 
                         
                        <asp:TemplateField HeaderText="Paid">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblPaidHeader" runat="server">Paid&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPaidAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAIDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField> 
                         
                        <asp:TemplateField HeaderText="Total">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblTotalHeader" runat="server">Total&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALEXPENDITURE", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField> 
                                                
                        <asp:TemplateField HeaderText="Budgeted">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblBudgetAmountHeader" runat="server">Budgeted &nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>  
                         
                        <asp:TemplateField HeaderText="Total">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblAccumulatedTotalHeader" runat="server">Total&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAccumulatedTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDTOTAL", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField> 
                                                
                        <asp:TemplateField HeaderText="Budget">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblBudgetedTotalHeader" runat="server">Budget&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetedTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDBUDGET", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField> 
                                                
                        <asp:TemplateField HeaderText="Variance">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblVarianceHeader" runat="server">Variance&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVariance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCE", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        
                    </Columns>
                </asp:GridView>
                
                <br />
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>            
    </form>
</body>
</html>
