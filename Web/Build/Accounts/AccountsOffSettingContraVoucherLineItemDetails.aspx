<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOffSettingContraVoucherLineItemDetails.aspx.cs" Inherits="AccountsOffSettingContraVoucherLineItemDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <form id="frmContraVoucherLineItemDetails" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlStockItemEntry">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Contra Voucher Items "></eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuLineItem_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 300px; width: 100%">
                </iframe>
                <div class="navSelect* " style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOrderLineItem" runat="server" OnTabStripCommand="MenuOrderLineItem_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">
                    <asp:GridView ID="gvLineItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvLineItem_RowCommand" OnRowDataBound="gvLineItem_RowDataBound"
                        OnRowCancelingEdit="gvLineItem_RowCancelingEdit" OnRowDeleting="gvLineItem_RowDeleting"
                        OnRowEditing="gvLineItem_RowEditing" OnRowCreated="gvLineItem_RowCreated" ShowHeader="true"
                        EnableViewState="false" AllowSorting="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVoucherLineItemNoHeader" runat="server">Row Number&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNewVoucherId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWVOUCHERID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lnkVoucherLineItemNo" runat="server" CommandArgument="<%# (Container.DataItemIndex)%>"
                                        CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMNO") %>'></asp:LinkButton>
                                    <asp:Label ID="lblVoucherLineId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMID") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account Id">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAccountCode" runat="server" Text="Account Code"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <span id="spnPickListCompanyAccountEdit">
                                        <asp:TextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'
                                            MaxLength="20" Width="80px"></asp:TextBox>
                                        <asp:TextBox ID="txtAccountDescription" runat="server" CssClass="input_mandatory"
                                            Enabled="False" MaxLength="50" Width="10px"></asp:TextBox>
                                        <asp:ImageButton ID="btnShowAccountEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                            OnClick="txtAccountCode_changed" ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItemIndex %>" />
                                        <asp:TextBox ID="txtAccountId" runat="server" CssClass="input" MaxLength="20" Width="10px"></asp:TextBox>
                                    </span>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account Description">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAccountDescription" runat="server" Text="Account Description"></asp:Literal>&nbsp;
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAccountDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budget Id">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSubAccountCode" runat="server" Text="Sub Account Code"></asp:Literal>&nbsp;
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBudget" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Currency">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblTransactionCurrency" runat="server" Text="Transaction Currency"></asp:Literal>&nbsp;
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPrimeAmount" runat="server" Text="Prime Amount"></asp:Literal>&nbsp;(
                                    <%=strTransactionAmountTotal%>
                                    )
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTranAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRANSACTIONAMOUNT","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblBaseAmount" runat="server" Text="Base Amount"></asp:Literal>&nbsp; (
                                    <%=strBaseAmountTotal%>
                                    )
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBaseAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASEAMOUNT","{0:n2}") %>'></asp:Label>
                                    <asp:Label ID="lblIsPosted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISPOSTED") %>' Visible="false"></asp:Label>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblReportAmount" runat="server" Text="Report Amount"></asp:Literal>&nbsp; (
                                    <%=strReportAmountTotal%>
                                    )
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReportAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTAMOUNT","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:Label ID="lblIsPeriodLocked" runat="server" Text="Period Locked" Visible="false" />
                                    <asp:Label ID="lblAccountActiveYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUTACTIVEYN") %>'
                                        Visible="false" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# (Container.DataItemIndex)%>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="hlnkSplit" runat="Server" Text="View" ImageUrl='<%$ PhoenixTheme:images/annexure.png%>'
                                        ToolTip="Allocation" CommandName="Sort" Visible="false"></asp:ImageButton>
                                    <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Save" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
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
