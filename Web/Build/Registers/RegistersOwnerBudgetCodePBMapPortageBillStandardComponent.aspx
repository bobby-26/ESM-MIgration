<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersOwnerBudgetCodePBMapPortageBillStandardComponent.aspx.cs"
    Inherits="RegistersOwnerBudgetCodePBMapPortageBillStandardComponent" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Airlines</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersAirlines" autocomplete="off" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPBStandardComponent">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Portage Bill Standard Component" ShowMenu="false" />
                    </div>
                </div>
                <div>
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Address runat="server" ID="ucOwner" CssClass="dropdown_mandatory" AddressType="128"
                                    AutoPostBack="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPBStandardComponent" runat="server" OnTabStripCommand="PBStandardComponent_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <asp:GridView ID="gvPBStdComp" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvPBStdComp_RowCommand" OnRowDataBound="gvPBStdComp_RowDataBound"
                    OnRowCancelingEdit="gvPBStdComp_RowCancelingEdit" OnRowDeleting="gvPBStdComp_RowDeleting"
                    OnRowEditing="gvPBStdComp_RowEditing" ShowFooter="True" Style="margin-bottom: 0px"
                    EnableViewState="false" OnRowUpdating="gvPBStdComp_RowUpdating" DataKeyNames="FLDDTKEY">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <Columns>
                        <asp:TemplateField HeaderText="Component Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDLOGTYPENAME"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlComponentTypeEdit" runat="server" CssClass="input_mandatory"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlComponentTypeEdit_SelectedIndexChanged"
                                    DataTextField="FLDNAME" DataValueField="FLDCOMPONENTID">
                                </asp:DropDownList>
                                <%--<asp:DropDownList ID="ddlComponentTypeEdit" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedIndexChanged="ddlComponentTypeEdit_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Onboard Earnings/Deduction"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Bond Issue"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Cash Advance"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Allotment"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="Radio Log"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="Balance Forward (B.F)"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="Special Allotment"></asp:ListItem>
                                        <asp:ListItem Value="8" Text="Sign Off Allotment"></asp:ListItem>
                                        <asp:ListItem Value="9" Text="Phone Card"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="Final Balance"></asp:ListItem>
                                        <asp:ListItem Value="22" Text="Sign Off Allowance"></asp:ListItem>
                                    </asp:DropDownList>--%>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddlComponentTypeAdd" runat="server" CssClass="input_mandatory"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlComponentTypeEdit_SelectedIndexChanged"
                                    DataTextField="FLDNAME" DataValueField="FLDCOMPONENTID">
                                </asp:DropDownList>
                                <%--<asp:DropDownList ID="ddlComponentTypeAdd" runat="server" CssClass="input_mandatory" AutoPostBack="true" 
                                   OnSelectedIndexChanged="ddlComponentTypeAdd_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Onboard Earnings/Deduction"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Bond Issue"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Cash Advance"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Allotment"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="Radio Log"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="Balance Forward (B.F)"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="Special Allotment"></asp:ListItem>
                                        <asp:ListItem Value="8" Text="Sign Off Allotment"></asp:ListItem>
                                        <asp:ListItem Value="9" Text="Phone Card"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="Final Balance"></asp:ListItem>
                                        <asp:ListItem Value="22" Text="Sign Off Allowance"></asp:ListItem>
                                    </asp:DropDownList>--%>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Component Sub Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDHARDNAME"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ddlHardEdit" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    HardTypeCode="128" ShortNameFilter="BSH,BSU,HRA,BSU,REM,BRF,CWB,CWO,CWP,AOT" HardList='<%#PhoenixRegistersHard.ListHard(1, 128, 0, "BSH,BSU,HRA,BSU,REM,BRF,CWB,CWO,CWP,AOT")%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ddlHardAdd" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    HardTypeCode="128" ShortNameFilter="BSH,BSU,HRA,BSU,REM,BRF,CWB,CWO,CWP,AOT" HardList='<%#PhoenixRegistersHard.ListHard(1, 128, 0, "BSH,BSU,HRA,BSU,REM,BRF,CWB,CWO,CWP,AOT")%>' />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDCODE"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtCodeEdit" runat="server" CssClass="gridinput" MaxLength="5" Text='<%# ((DataRowView)Container.DataItem)["FLDCODE"]%>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtCodeAdd" runat="server" CssClass="gridinput" MaxLength="5"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDDESCRIPTION"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDescEdit" runat="server" CssClass="gridinput_mandatory" MaxLength="200"
                                    Text='<%# ((DataRowView)Container.DataItem)["FLDDESCRIPTION"]%>'></asp:TextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="txtDescAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"></asp:TextBox>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Budget Code">
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDBUDGETCODE"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListBudgetEdit">
                                    <asp:TextBox ID="txtBudgetCodeEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                        MaxLength="20" CssClass="input_mandatory" Width="80%" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="input hidden"
                                        Enabled="False"></asp:TextBox>
                                    <asp:ImageButton ID="btnShowBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItemIndex %>" />
                                    <asp:TextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="input hidden"
                                        Text='<%#DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></asp:TextBox>
                                    <asp:TextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input hidden"></asp:TextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListBudgetAdd">
                                    <asp:TextBox ID="txtBudgetCodeAdd" runat="server" MaxLength="20" CssClass="input_mandatory"
                                        Width="80%" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txtBudgetNameAdd" runat="server" Width="0px" CssClass="input hidden"
                                        Enabled="False"></asp:TextBox>
                                    <asp:ImageButton ID="btnShowBudgetAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataItemIndex %>" />
                                    <asp:TextBox ID="txtBudgetIdAdd" runat="server" Width="0px" CssClass="input hidden"></asp:TextBox>
                                    <asp:TextBox ID="txtBudgetgroupIdAdd" runat="server" Width="0px" CssClass="input hidden"></asp:TextBox>
                                </span>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Owner Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOwnerBudgetCodeEdit">
                                    <asp:TextBox ID="txtOwnerBudgetCodeNameEdit" runat="server" Width="80%" Enabled="False"
                                        CssClass="input_mandatory"></asp:TextBox>
                                    <asp:ImageButton ID="btnShowOwnerBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItemIndex %>" />
                                    <asp:TextBox ID="txtOwnerBudgetCodeIdEdit" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                                    <asp:TextBox ID="txtParentgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                                </span>
                                <asp:Label ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                            </EditItemTemplate>
                            <FooterTemplate>
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
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                &nbsp;<b><asp:Literal ID="lblMappedComponents" runat="server" Text="Mapped Components"></asp:Literal></b>
                <asp:GridView ID="gvOwner" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="gvOwner_RowDataBound" ShowHeader="true"
                    EnableViewState="false" AllowSorting="true" OnSorting="gvOwner_Sorting" OnSelectedIndexChanging="gvOwner_SelectedIndexChanging"
                    OnRowCommand="gvOwner_RowCommand">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblComponentName" runat="server" Text="Component Name"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDPRINCIAPL")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDOWNERBUDGETNAME")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
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
