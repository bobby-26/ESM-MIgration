<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAdminAssetMapping.aspx.cs" Inherits="Registers_RegistersAdminAssetMapping" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Asset Software Mapping</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="AssetSoftwareMapping" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmsoftwaremapping" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlsoftwaremapping">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Installed Software" ShowMenu="False"></eluc:Title>
                    </div>
                </div>
                <table width="100%" cellpadding="2" cellspacing="2">
                    <tr>
                        <td>
                            <asp:Literal ID="lblAssetType" runat="server">Asset</asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAssetType" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="SoftwareMenuList" runat="server" OnTabStripCommand="SoftwareMenuList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvSoftwareMapping" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvSoftwareMapping_RowCommand" OnRowDataBound="gvSoftwareMapping_ItemDataBound" 
                        OnRowCancelingEdit="gvSoftwareMapping_RowCancelingEdit" OnRowDeleting="gvSoftwareMapping_RowDeleting" 
                        OnRowUpdating="gvSoftwareMapping_RowUpdating" OnRowEditing="gvSoftwareMapping_RowEditing"
                        ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true" 
                        DataKeyNames="FLDADMINASSETSOFTWAREMAPPINGID" OnSorting="gvSoftwareMapping_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="30%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSoftwareTypeHeader" runat="server" CommandName="Sort" CommandArgument="FLDASSETTYPEID"
                                        ForeColor="White">Software&nbsp;</asp:Label>
                                    <img id="FLDSOFTWARETYPEID" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSoftwareType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblAssetId" runat="server" Visible="false"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSETID") %>'></asp:Label>
                                    <asp:Label ID="lblSoftwareTypeEdit" runat="server" Visible="false"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSETTYPEID") %>'></asp:Label>
                                    <asp:DropDownList ID="ddlSoftwareTypeEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" DataTextField="FLDNAME" DataValueField="FLDASSETTYPEID" Width="100%"></asp:DropDownList>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlSoftwareTypeAdd" runat="server" CssClass="input_mandatory" Width="100%"></asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="25%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblInstalledDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDINSTALLEDDATE"
                                        ForeColor="White" Width="40%">Installed Date&nbsp;</asp:Label>
                                    <img id="FLDINSTALLEDDATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInstalledDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDINSTALLEDDATE")) %>' Width="40%"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date ID="ucInstalledDateEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDINSTALLEDDATE")%>'
                                     CssClass="input" Width="21.5%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="txtInstalledDateAdd" Visible="false" runat="server" CssClass="input_mandatory"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="25%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblUninstalledDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDUNINSTALLEDDATE"
                                        ForeColor="White">Uninstalled Date&nbsp;</asp:Label>
                                    <img id="FLDUNINSTALLEDDATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUninstalledDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDUNINSTALLEDDATE")) %>' Width="40%"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblUninstalledDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDUNINSTALLEDDATE")) %>' Width="40%"></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="txtUninstalledDateAdd" Visible="false" runat="server" CssClass="input_mandatory"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Uninstall"></asp:ImageButton>
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
                        <eluc:Status runat="server" ID="ucStatus" />
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
