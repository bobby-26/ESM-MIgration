<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseReconciledInvoiceList.aspx.cs" Inherits="PurchaseReconciledInvoiceList" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
            
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlOrderForm">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="frmTitle" Text="Reconciliation Statistics"></eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <table id="tblGuidance" runat="server">
                    <tr>
                        <td>
                            <asp:Label ID="lblNote" runat="server" Text="Note: Enter 'From Date' and 'To Date' then click on the find button."
                                ForeColor="Blue" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table id="tblBudgetGroupAllocationSearch" width="100%">
                    <tr>
                        <td>
                           <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:UserControlDate ID="ucFromDate" runat="server" CssClass="input" />
                        </td>
                        <td>
                           <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:UserControlDate ID="ucToDate" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblInvoiceType" runat="server" Text="Invoice Type"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:Hard ID="ddlInvoiceType" runat="server" AppendDataBoundItems="true" AutoPostBack="TRUE"
                                CssClass="input" HardTypeCode="59" Width="300px" />
                        </td>
                        <td>
                           <asp:Literal ID="lblInvoiceCurrency" runat="server" Text="Invoice Currency"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:UserControlCurrency ID="ddlCurrencyCode" runat="server" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtInvoiceNumber" MaxLength="200" CssClass="input"
                                Width="150px"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblInvoiceReference" runat="server" Text="Invoice Reference"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox ID="txtSupplierReference" runat="server" CssClass="input" MaxLength="100" Text=""
                                Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblInvoiceStatus" runat="server" Text="Invoice Status"></asp:Literal>
                            
                        </td>
                        <td>
                            <div id="dvStatus" runat="server" class="input" style="overflow: auto; width: 40%;
                                height: 80px">
                                <asp:CheckBoxList ID="chkInvoiceStatus" runat="server" Height="100%"
                                    RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td>
                           <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            
                        </td>
                        <td>
                            <div id="dvVessel" runat="server" class="input" style="overflow: auto; width: 60%;
                                height: 80px">
                                <asp:CheckBoxList ID="chkVesselList" runat="server" Height="100%"
                                    RepeatColumns="1" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblSupplierCode" runat="server" Text="Supplier Code"></asp:Literal>
                             </td>
                        <td>
                            <span ID="spnPickListMaker">
                            <asp:TextBox ID="txtVendorCode" runat="server" CssClass="input" 
                                ReadOnly="false" Width="60px"></asp:TextBox>
                            <asp:TextBox ID="txtVenderName" runat="server" CssClass="input" 
                                ReadOnly="false" Width="180px"></asp:TextBox>
                            <img ID="ImgSupplierPickList" runat="server" 
                                src="<%$ PhoenixTheme:images/picklist.png %>" 
                                style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                            <asp:TextBox ID="txtVendorId" runat="server" Width="10px"></asp:TextBox>
                            </span></td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvVoucherDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" AllowSorting="true" OnSorting="gvVoucherDetails_Sorting"
                        EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Invoice Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                           <asp:Literal ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></asp:Literal>
                                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInvoiceNumber" runat="server" Visible="TRUE" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                           <asp:Literal ID="lblOrderNumber" runat="server" Text="Order Number"></asp:Literal>
                                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFormNo" runat="server" Visible="TRUE" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vessel">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                           <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Received Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                           <asp:Literal ID="lblReceivedDate" runat="server" Text="Received Date"></asp:Literal>
                                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReceivedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICERECEIVEDDATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reconciled Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                           <asp:Literal ID="lblReconciledDate" runat="server" Text="Reconciled Date"></asp:Literal>
                                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblreconciledDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECONCILEDDATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
