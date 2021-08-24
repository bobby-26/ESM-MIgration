<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockBudget.aspx.cs" Inherits="DryDockBudget"
    MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Budget</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Budgetlink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmDryDockBudget" runat="server" autocomplete="off" >
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDryDockBudget">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Status runat="server" ID="ucStatus" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                       <asp:Literal ID="lblBudget" runat="server" Text="Budget"></asp:Literal>
                    </div>
                </div>
                <div id="divFind" style="position: relative;">
                    <table id="tblBudget" width="100%">
                        <tr>
                            <td>
                       <asp:Literal ID="lblBudgetGroup" runat="server" Text="Budget Group"></asp:Literal>
                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtBudgetGroup" runat="server" MaxLength="50" ReadOnly="true" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                       <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                                
                            </td>
                            <td>
                                <asp:TextBox ID="txtBudgetCode" runat="server" MaxLength="20" ReadOnly="true" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="position: relative;">
                    <eluc:TabStrip ID="MenuDryDockBudget" runat="server" OnTabStripCommand="DryDockBudget_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: +1;">
                    <asp:GridView ID="gvBudget" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvBudget_RowCommand" OnRowDataBound="gvBudget_ItemDataBound"
                        OnRowCancelingEdit="gvBudget_RowCancelingEdit" OnRowDeleting="gvBudget_RowDeleting"
                        OnRowEditing="gvBudget_RowEditing" ShowFooter="true" ShowHeader="true" EnableViewState="false"
                        OnSorting="gvBudget_Sorting" AllowSorting="true" OnRowCreated="gvBudget_RowCreated"
                        OnSelectedIndexChanging="gvBudget_SelectedIndexChanging" OnRowUpdating="gvBudget_RowUpdating">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Principal">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <HeaderTemplate>
                                     <asp:LinkButton ID="lblPrincipalHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                        ForeColor="White">Principal&nbsp;</asp:LinkButton>
                                    <img id="FLDNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPrinicipal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'>
                                    </asp:Label>
                                    <asp:Label ID="lblBudgetid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                <asp:Label ID="lblBudgetidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'>
                                    </asp:Label>
                                    <eluc:AddrType ID="ucPrinicipalEdit" runat="server" AddressType="128"
                                        AddressList='<%# PhoenixRegistersAddress.ListAddress("128") %>' AppendDataBoundItems="true"
                                        CssClass="dropdown_mandatory" SelectedAddress='<%# DataBinder.Eval(Container,"DataItem.FLDPRINCIPALID") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:AddrType ID="ucPrinicipalAdd" runat="server" AddressType="128"
                                        AddressList='<%# PhoenixRegistersAddress.ListAddress("128") %>' AppendDataBoundItems="true"
                                        CssClass="dropdown_mandatory" />
                                </FooterTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Budget Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblBudgetCodeHeader" runat="server" CommandName="Sort" CommandArgument="FLDBUDGETCODE"
                                        ForeColor="White">Budget Code&nbsp;</asp:LinkButton>
                                    <img id="FLDBUDGETCODE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'
                                        CssClass="gridinput_mandatory" MaxLength="20"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtBudgetCodeAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="20" ToolTip="Budget Code"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Budget Group">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblBudgetGroupHeader" runat="server" CommandName="Sort" CommandArgument="FLDBUDGETGROUP"
                                        ForeColor="White">Budget Group&nbsp;</asp:LinkButton>
                                    <img id="FLDBUDGETGROUP" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUP") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtBudgetGroupEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUP") %>'
                                        CssClass="gridinput_mandatory" MaxLength="50"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtBudgetGroupAdd" runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="50" ToolTip="Budget Group"></asp:TextBox>
                                </FooterTemplate>
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
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
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
