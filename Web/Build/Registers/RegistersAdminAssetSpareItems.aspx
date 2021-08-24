<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAdminAssetSpareItems.aspx.cs" Inherits="Registers_RegistersAdminAssetSpareItems" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Spare Items</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersAdminAsset" autocomplete="off" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAdminAssetEntry">
        <ContentTemplate>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Spare" />
                    </div>
                </div>
                <dev>
                    <table width="100%">
                        <tr>
                            <td width= "6.6%">
                                <asp:Literal ID="lblLocation" runat="server" Text="Zone"></asp:Literal> 
                            </td>
                            <td width= "26.6%">
                                <eluc:Zone ID="ddlLocation" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="180px"  AutoPostBack="true"/>
                            </td>
                            <td width= "6.6%">Item </td>
                            <td width= "26.6%">
                                <asp:DropDownList ID="ddlItemType" runat="server" AutoPostBack="true" CssClass="input" Width="180px">
                                </asp:DropDownList>
                            </td>
                            <td width= "6.6%">Serial Number </td>
                            <td width= "26.6%">
                                <asp:TextBox ID="txtSerialNo" runat="server" CssClass="input" Width="180px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </dev>
                <div style="position: relative; overflow: hidden; clear:right; top: 0px; left: 0px;">
                    <eluc:TabStrip ID="MenuRegistersAdminAsset" runat="server" OnTabStripCommand="MenuRegistersAdminAsset_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvAdminAssetItems" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvAdminAssetItems_RowCommand" OnRowDataBound="gvAdminAssetItems_RowDataBound"
                        OnRowDeleting="gvAdminAssetItems_RowDeleting" OnRowEditing="gvAdminAssetItems_RowEditing"
                        OnRowCancelingEdit="gvAdminAssetItems_RowCancelingEdit" OnRowUpdating="gvAdminAssetItems_Rowupdating"
                        ShowFooter="false" ShowHeader="true" OnSorting="gvAdminAssetItems_Sorting" AllowSorting="true" EnableViewState="false" DataKeyNames="FLDASSETTYPEID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField FooterText="Asset Type">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAssetTypeHeader" runat="server" CommandName="Sort" CommandArgument="FLDASSETTYPEID"
                                        ForeColor="White">Item&nbsp;</asp:Label>
                                    <img id="Img8" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID ="lblAssetTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDASSETTYPEID")%>'></asp:Label>
                                    <asp:Label ID ="lblAssemblyParentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDASSEMBLYPARENTID")%>'></asp:Label>
                                    <asp:Label ID ="lblItemType" runat="server" visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDASSETTYPENAME")%>'></asp:Label>
                                    <asp:Label ID ="lblLocationId" runat="server" visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPANYID")%>'></asp:Label>
                                    <asp:LinkButton ID ="lnkItemType" runat="server" CommandName="SELECT" Text='<%# DataBinder.Eval(Container, "DataItem.FLDASSETTYPENAME")%>' CommandArgument="<%# Container.DataItemIndex %>"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="Serial No">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSerialNoHeader" runat="server" CommandName="Sort" CommandArgument="FLDSERIALNO"
                                        ForeColor="White">Serial Number&nbsp;</asp:Label>
                                    <img id="Img19" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSERIALNO")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="New Team Member">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAdminAssetNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                        ForeColor="White">Name&nbsp;</asp:Label>
                                    <img id="FLDNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAdminAssetID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSETID") %>'></asp:Label>
                                    <asp:Label ID="lblAdminAssetName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="Asset Maker">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="12.5%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAssetMakerHeader" runat="server" CommandName="Sort" CommandArgument="FLDMAKER"
                                        ForeColor="White">Maker&nbsp;</asp:Label>
                                    <img id="Img5" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDMAKER")%>
                                </ItemTemplate>
                            </asp:TemplateField>   
                            <asp:TemplateField FooterText="Asset Model">
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="12.5%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAssetModelHeader" runat="server" CommandName="Sort"
                                        CommandArgument="FLDIDENTIFICATIONNUMBER" ForeColor="White">Model&nbsp;</asp:Label>
                                    <img id="Img18" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDIDENTIFICATIONNUMBER")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblStatus" runat="server" CommandName="Sort"
                                        CommandArgument="FLDSTATUSNAME" ForeColor="White">Status&nbsp;</asp:Label>
                                    <img id="Img18" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSTATUSNAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>                      
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"  Width="10%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;z-index: 0">
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
