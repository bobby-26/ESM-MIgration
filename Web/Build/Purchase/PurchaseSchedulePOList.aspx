<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseSchedulePOList.aspx.cs" Inherits="PurchaseSchedulePOList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersBudgetBillingList" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlBudgetBillingListEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
             <eluc:Status runat="server" ID="ucStatus" Visible="false" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute; z-index: +1">
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                    <div class="subHeader" style="position: relative; right: 0px">
                        <eluc:Title runat="server" ID="Title1" Text="Schedule PO" ShowMenu="<%# Title1.ShowMenu %>">
                        </eluc:Title>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuSchedulePO" runat="server" OnTabStripCommand="MenuSchedulePO_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 220px; width: 100%; z-index: 104">
                </iframe>
                <div style="position: relative; width: 15px; z-index: 1">
                    <eluc:TabStrip ID="MenuMainSchedulePO" runat="server" OnTabStripCommand="MenuMainSchedulePO_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="gvSchedulePO" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnSorting="gvBulkPO_Sorting" Width="100%" CellPadding="3" OnRowCommand="gvBulkPO_RowCommand"
                        OnRowDataBound="gvBulkPO_ItemDataBound" OnRowCancelingEdit="gvBulkPO_RowCancelingEdit"
                        AllowSorting="true" OnRowEditing="gvBulkPO_RowEditing" ShowFooter="false" ShowHeader="true"
                        EnableViewState="false" DataKeyNames="FLDSCHEDULEPOID" OnSelectedIndexChanging="gvBulkPO_SelectedIndexChanging">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                             <asp:TemplateField HeaderText="Form Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblFormNoHeader" runat="server">Form <br /> Number
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblFormNo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkFormNo" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                             <asp:TemplateField HeaderText="Vendor">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVesselHeader" runat="server">Vessel
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Form Title">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblFormTitleHeader" runat="server">Form Title&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblScheduleId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCHEDULEPOID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblFormTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTITLE") %>'></asp:Label>
                                    <asp:Label ID="lblCreateFirstTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTTIMEYN") %>'
                                        Visible="false"></asp:Label>
                                     <asp:Label ID="lblApprovedYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDYN") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> 
                                                      
                            <asp:TemplateField HeaderText="Vendor">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVendorHeader" runat="server">Vendor
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Budget Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblBudgetCodeHeader" runat="server">Budget Code
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Currency">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCurrencyHeader" runat="server">Currency
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Start Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStartDateHeader" runat="server">Start Date
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStartDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTDATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="End Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblEndDateHeader" runat="server">End Date
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENDDATE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStatusHeader" runat="server">Status
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblApprovalStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDYN") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblApprovalStatusName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALSTATUS") %>'></asp:Label>
                                    <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%#HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDAPPROVALSTATUSNAME").ToString()) %>' />
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
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton ID="cmdPOApprove" runat="server" AlternateText="POApprove" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="POAPPROVE" ImageUrl="<%$ PhoenixTheme:images/approve.png %>" ToolTip="PO Approve"
                                        Enabled="true" />
                                    <img id="Img7" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Revoke approval" ImageUrl="<%$ PhoenixTheme:images/cancel.png %>"
                                        CommandName="DEAPPROVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDeApprove"
                                        ToolTip="Revoke approval"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton ID="cmdCreatePO" runat="server" AlternateText="CREATEPO" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="CREATEPO" ImageUrl="<%$ PhoenixTheme:images/task-list.png %>" ToolTip="Create PO"
                                        Enabled="true" />
                                    <%-- <asp:ImageButton ID="cmdCopy" runat="server" AlternateText="POCOPYTOORDERFORM" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="POCOPYTOORDERFORM" ImageUrl="<%$ PhoenixTheme:images/copy.png %>"
                                        ToolTip="Create PO" Enabled="true" Visible="false" />
                                    <img id="Img3" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton ID="cmdPost" runat="server" AlternateText="POPOST" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="POPOST" ImageUrl="<%$ PhoenixTheme:images/pr.png %>" ToolTip="Create Invoice"
                                        Enabled="true" Visible="false" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server"> </asp:Label>
                                <asp:Label ID="lblPages" runat="server"> </asp:Label>
                                <asp:Label ID="lblRecords" runat="server"> </asp:Label>&nbsp;&nbsp;
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
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input"> </asp:TextBox>
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
