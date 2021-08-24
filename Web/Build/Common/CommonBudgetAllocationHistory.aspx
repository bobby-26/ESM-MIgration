<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonBudgetAllocationHistory.aspx.cs" Inherits="CommonBudgetAllocationHistory" %>

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
    <title>Vessel Budget Allocation History</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">    
    
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    
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
                    <eluc:Title runat="server" id="ucTitle" Text="Budget Allocation History" ShowMenu="false"></eluc:Title>
                </div>
                
                <table id="tblBudgetGroupAllocationSearch" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Address runat="server" ID="ucOwner" CssClass="dropdown_mandatory" AddressType="128" AppendDataBoundItems="true" AutoPostBack="true"
                                 Enabled="false" />
                        </td>  
                        <td>
                            <asp:Literal ID="lblFinancialYear" runat="server" Text="Financial Year"></asp:Literal>
                        </td>                          
                        <td>
                            <eluc:Quick runat="server" ID="ucFinancialYear" QuickTypeCode="55" CssClass="dropdown_mandatory" AutoPostBack="true" AppendDataBoundItems="true"
                                 Enabled="false" />
                        </td>
                    </tr>
                </table>
                
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
                                <asp:Label ID="lblVesselHeader" runat="server">Vessel&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVesselAllocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELBUDGETALLOCATIONID") %>'></asp:Label>
                                <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                <asp:Label ID="lblOwnerId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERID") %>'></asp:Label>
                                <asp:Label ID="lblFinancialYearId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALYEAR") %>'></asp:Label>
                                <asp:Label ID="lnkVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField> 
                                                
                        <%--<asp:TemplateField HeaderText="Budget Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblBudgetAmountHeader" runat="server">Amount&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                                                
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
                                                
                        <asp:TemplateField HeaderText="Applied Periods">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblappliedperiodHeader" runat="server">Applied Periods&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblappliedperiod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPLIEDPERIODS" ) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField> 
                                                
                        <%--<asp:TemplateField HeaderText="Utilized Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblUtilizedAmountHeader" runat="server">Utilized Amount&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblUtilizedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUTILIZEDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                                                
                        <asp:TemplateField HeaderText="Over written">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblOverwrittenHeader" runat="server">Over written&nbsp;
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblOverwritten" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERWRITTEN" ) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>            
    </form>
</body>
</html>
