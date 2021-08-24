<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseCommittedCostBreakUpRevoked.aspx.cs" Inherits="PurchaseCommittedCostBreakUpRevoked" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Committed Cost Breakup</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">    
    
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCostBreakup" runat="server" submitdisabledcontrols="true">

    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>    
    <asp:UpdatePanel runat="server" ID="pnlCostBreakup">
        <ContentTemplate>                
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align:top; width:100%; position:absolute; z-index:+1">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <asp:Literal ID="lblCostBreakup" runat="server" Text="Revoked"></asp:Literal>&nbsp;&nbsp;&nbsp;
                        (<asp:Literal ID="lblVesselHdr"  runat="server"></asp:Literal>&nbsp;&nbsp;
                        <asp:Literal ID="lblMonthHdr"  runat="server"></asp:Literal>&nbsp;-&nbsp;
                        <asp:Literal ID="lblBudgetGroupHdr"  runat="server"></asp:Literal>)
                    </div>
                </div>
                <br />
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblPeriod" runat="server" Text="Period"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPeriodName" runat="server" CssClass="readonlytextbox" ReadOnly="true" ></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <div id="divGrid" style="position: relative; z-index:+1">               
                    <asp:GridView ID="gvCostBreakup" runat="server" AutoGenerateColumns="False" Font-Size="11px" 
                        Width="100%" CellPadding="3" AllowSorting="false" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        
                        <Columns>                              
                            <asp:TemplateField Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <%--<asp:LinkButton ID="lblVesselNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME" ForeColor="White">Vessel Name&nbsp;</asp:LinkButton>
                                    <img id="FLDVESSELNAME" runat="server" visible="false" />--%>
                                    <asp:Label ID="lblVesselHeader" runat="server">Vessel&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                    <asp:Label ID="lnkVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>                                
                            </asp:TemplateField> 
                             
                            <asp:TemplateField HeaderText="PO Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblIMONumberHeader" runat="server">PO Number&nbsp;</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPONumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></asp:Label>
                                </ItemTemplate>                          
                            </asp:TemplateField> 
                                              
                            <asp:TemplateField HeaderText="Vendor">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVendorHeader" runat="server">Vendor&nbsp;</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                </ItemTemplate>                          
                            </asp:TemplateField> 
                            
                            <asp:TemplateField HeaderText="Description">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDescriptionHeader" runat="server">Description&nbsp;</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:Label>
                                </ItemTemplate>                          
                            </asp:TemplateField> 
                                
                            <asp:TemplateField HeaderText="Budget Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblBudgetCodeHeader" runat="server">Budget Code&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></asp:Label>
                                </ItemTemplate>       
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAmountHeader" runat="server">Amount (USD)&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTINUSD") %>'></asp:Label>
                                </ItemTemplate>                                
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblApprovalDateHeader" runat="server">Order Date&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblApprovalDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFAPPROVAL", "{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>                                
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVoucherNumberHeader" runat="server">Voucher Number&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></asp:Label>
                                </ItemTemplate>                                
                            </asp:TemplateField>
                            
                            <%--<asp:TemplateField HeaderText="Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTypeHeader" runat="server">Type&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                </ItemTemplate>                                
                            </asp:TemplateField>--%>                            
                        </Columns>
                    </asp:GridView>
                </div>
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
                <%--<b><asp:Literal ID="lblReversedCommitted" runat="server" Text="Reversed Committed"></asp:Literal></b>--%>
                <%--<div style="position: relative; width:15px">
                    <eluc:TabStrip ID="MenuCostBreakup" runat="server" OnTabStripCommand="CostBreakup_TabStripCommand">
                    </eluc:TabStrip>
                </div>--%>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvCostBreakup" />
        </Triggers>
    </asp:UpdatePanel>            
    </form>
</body>
</html>
