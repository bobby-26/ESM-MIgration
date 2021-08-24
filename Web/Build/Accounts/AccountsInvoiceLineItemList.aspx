<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceLineItemList.aspx.cs"
    Inherits="AccountsInvoiceLineItemList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <style type="text/css">
        .style1
        {
            width: 204px;
        }
        .style2
        {
            width: 190px;
        }
        #tblExchangeRate
        {
            width: 79%;
        }
    </style>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInvoice" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInvoice">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navSelect" style="position: relative; width: 15px">
                <eluc:TabStrip ID="MenuInvoice" runat="server" OnTabStripCommand="Invoice_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <asp:GridView ID="gvInvoice" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    EnableViewState="False" Font-Size="11px" OnRowCommand="gvInvoice_RowCommand"
                    OnRowDataBound="gvInvoice_ItemDataBound" OnRowEditing="gvInvoice_RowEditing"
                    OnRowCancelingEdit="gvInvoice_RowCancelingEdit" ShowFooter="False" Width="100%">
                    <FooterStyle CssClass="datagrid_footerstyle" />
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>
                        <asp:TemplateField HeaderText="PurchaseOrderNumber">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblPurchaseOrderNumberHeader" runat="server">PO Number&nbsp;
                                    <asp:ImageButton ID="cmdInvoiceCodeDesc" runat="server" CommandArgument="1" CommandName="FLDPURCHASEORDERNUMBER"
                                        ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click" />
                                    <asp:ImageButton ID="cmdInvoiceCodeAsc" runat="server" CommandArgument="0" CommandName="FLDPURCHASEORDERNUMBER"
                                        ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>" OnClick="cmdSort_Click" />
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPurchaseOrderNumber" runat="server" CommandArgument='<%# Bind("FLDINVOICELINEITEMCODE") %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEORDERNUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPurchaseOrderNumberEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEORDERNUMBER") %>'
                                    CssClass="gridinput_mandatory" MaxLength="10"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="VesselCode">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblVesselCode" runat="server" Text="Vessel Code"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtVesselCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    CssClass="gridinput_mandatory" MaxLength="10"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="VesselAccount">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblVesselAccount" runat="server" Text="Vessel Account"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVesselAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELACCOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtVesselAccountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELACCOUNT") %>'
                                    CssClass="gridinput_mandatory" MaxLength="10"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PurPayableAmount">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblPurPayableAmount" runat="server" Text="Pur.Payable Amount"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPurPayableAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEPAYABLEAMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPurPayableAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEPAYABLEAMOUNT") %>'
                                    CssClass="gridinput_mandatory" MaxLength="10"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PurAdvanceAmount">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblPurAdvanceAmount" runat="server" Text="Pur.Advance Amount"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblPurAdvanceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEADVANCEAMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtPurAdvanceAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEADVANCEAMOUNT") %>'
                                    CssClass="gridinput_mandatory" MaxLength="10"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="InvPayableAmount">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblInvPayableAmountHeader" runat="server"> Inv.Payable Amount
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblInvPayableAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEPAYABLEAMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtInvPayableAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEPAYABLEAMOUNT") %>'
                                    CssClass="gridinput_mandatory" MaxLength="10"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField FooterStyle-HorizontalAlign="Center" HeaderText="IsIncludedinSOA">
                            <FooterStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <HeaderTemplate>
                                <asp:Label ID="lblIsIncludedinSOAHeader" runat="server"> Is Included in SOA
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblIsIncludedinSOAYN" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDISINCLUDEDINSOA").ToString().Equals("1"))?"Yes":"No" %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkIncludedinSOAYNEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDISINCLUDEDINSOA").ToString().Equals("1"))?true:false %>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle />
                            <HeaderTemplate>
                                <asp:Label ID="lblActionHeader" runat="server"> Action </asp:Label>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                            <ItemTemplate>
                                <asp:ImageButton ID="cmdEdit" runat="server" AlternateText="Edit" CommandArgument="<%# Container.DataItemIndex %>"
                                    CommandName="EDIT" ImageUrl="<%$ PhoenixTheme:images/te_edit.png%>" ToolTip="Edit" />
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton ID="cmdSave" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataItemIndex %>"
                                    CommandName="Save" ImageUrl="<%$ PhoenixTheme:images/save.png%>" ToolTip="Save" />
                                <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                            </EditItemTemplate>
                        </asp:TemplateField>
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
        </ContentTemplate>        
    </asp:UpdatePanel>
    </form>
</body>
</html>
