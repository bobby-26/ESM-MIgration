<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsUsageSupplier.aspx.cs"
    Inherits="AccountsUsageSupplier" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sub Account Code</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="dvlink">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlBudget">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative;">
                    <div id="divHeading">
                        <asp:Literal ID="lblSubAccount" runat="server" Text="SubAccount"></asp:Literal>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div>
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblAccountCode" runat="server" Text="Account Code"></asp:Literal>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblSubAccountMapId" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtAccountCode" runat="server" MaxLength="10" Width="150px" ReadOnly="true"
                                    CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblAccountDescription" runat="server" Text="Account Description"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAccountDescription" runat="server" MaxLength="10" Width="200px"
                                    ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblAccountUsage" runat="server" Text="Account Usage"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAccountUsage" runat="server" MaxLength="10" Width="150px" ReadOnly="true"
                                    CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblSubAccountCode" runat="server" Text="Sub Account Code"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubAccount" runat="server" MaxLength="10" Width="150px" CssClass="input_mandatory"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="200" Width="400px" CssClass="input_mandatory"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersBudget" runat="server" OnTabStripCommand="RegistersBudgetMenu_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gdBudget" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gdBudget_RowCommand" OnRowDataBound="gdBudget_ItemDataBound"
                        OnRowEditing="gdBudget_RowEditing" OnRowDeleting="gdBudget_RowDeleting" OnSorting="gdBudget_Sorting"
                        AllowSorting="true" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField HeaderText="Sub Account">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblSubAccountHeader" runat="server" CommandName="Sort" CommandArgument="FLDSUBACCOUNT"
                                        ForeColor="White">Code&nbsp;</asp:LinkButton>
                                    <img id="FLDSUBACCOUNT" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkBudget" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblDescriptionHeader" runat="server" CommandName="Sort" CommandArgument="FLDDESCRIPTION"
                                        ForeColor="White">Description&nbsp;</asp:LinkButton>
                                    <img id="FLDDESCRIPTION" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                </ItemTemplate>
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
