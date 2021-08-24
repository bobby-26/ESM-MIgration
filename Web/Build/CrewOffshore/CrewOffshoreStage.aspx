<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreStage.aspx.cs"
    Inherits="CrewOffshoreStage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="InspectionCompany" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmOffshoreStage" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlOffshoreStage">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Stage"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOffshoreStage" runat="server" OnTabStripCommand="MenuOffshoreStage_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvOffshoreStage" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter="true" OnRowCommand="gvOffshoreStage_RowCommand"
                        OnRowEditing="gvOffshoreStage_RowEditing" OnRowCancelingEdit="gvOffshoreStage_RowCancelingEdit"
                        OnRowUpdating="gvOffshoreStage_RowUpdating" ShowHeader="true" EnableViewState="false"
                        DataKeyNames="FLDSTAGEID" OnRowDataBound="gvOffshoreStage_RowDataBound" OnRowDeleting="gvOffshoreStage_OnRowDeleting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblShortcodeHeader" runat="server">Short Code</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblShortCode" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtShortCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'
                                        CssClass="gridinput_mandatory"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtShortCodeAdd" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStageHeader" runat="server">Stage</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStage" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtStageEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTAGE") %>'
                                        CssClass="gridinput_mandatory"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtStageAdd" runat="server" CssClass="gridinput_mandatory"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblOrderHeader" runat="server">Order</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDER") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucOrderEdit" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                        MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDER") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucOrderAdd" runat="server" CssClass="gridinput_mandatory" IsInteger="true"
                                        MaxLength="3" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblStatusHeader" runat="server">Plan Status</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'>
                                   </asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblStatusID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>' Visible="false" ></asp:Label>
                                    <eluc:Hard ID="ucStatusEdit" runat="server" CssClass="input_mandatory" HardTypeCode="99" ShortNameFilter="AWA,APV,AFT,AFS,APP" AppendDataBoundItems="true"/>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Hard ID="ucStatusAdd" runat="server" CssClass="input_mandatory" HardTypeCode="99" ShortNameFilter="AWA,APV,AFT,AFS,APP" AppendDataBoundItems="true"/>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
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
                                        CommandName="UPDATE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
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
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
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
