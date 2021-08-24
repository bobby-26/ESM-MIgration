﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterAdminAsset.aspx.cs" Inherits="RegisterAdminAsset" MaintainScrollPositionOnPostback="true"%>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Asset Hardware</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
                        <eluc:Title runat="server" ID="ucTitle" Text="Hardware" />
                    </div>
                </div>
               <div style="position: relative; overflow: hidden; clear:right;">
                    <eluc:TabStrip ID="MenuRegistersAdminAsset" runat="server" OnTabStripCommand="MenuRegistersAdminAsset_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvAdminAsset" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvAdminAsset_RowCommand" OnRowDataBound="gvAdminAsset_RowDataBound"
                        OnRowDeleting="gvAdminAsset_RowDeleting" OnRowEditing="gvAdminAsset_RowEditing"
                        OnRowCancelingEdit="gvAdminAsset_RowCancelingEdit" OnRowUpdating="gvAdminAsset_Rowupdating"
                        ShowFooter="false" ShowHeader="true" OnSorting="gvAdminAsset_Sorting" AllowSorting="true" EnableViewState="false" DataKeyNames="FLDASSETID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField FooterText="Asset Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAssetTypeHeader" runat="server" CommandName="Sort" CommandArgument="FLDASSETTYPEID"
                                        ForeColor="White">Assembly&nbsp;</asp:Label>
                                    <img id="Img8" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID ="lblAssetTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDASSETTYPEID")%>'></asp:Label>
                                    <asp:Label ID ="lblAssemblyParentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDASSEMBLYPARENTID")%>'></asp:Label>
                                    <asp:Label ID ="lblItemType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDITEMTYPE")%>'></asp:Label>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDASSETTYPENAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="Tag Number" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTagNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDTAGNUMBER"
                                        ForeColor="White">Assembly&nbsp;</asp:Label>
                                    <img id="Img20" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDTAGNUMBER")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="Serial No">
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
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                                        CommandName="BindData" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                    <asp:LinkButton ID="lblAdminAssetNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                        ForeColor="White">Name&nbsp;</asp:LinkButton>
                                    <img id="FLDNAME" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAdminAssetID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSETID") %>'></asp:Label>
                                    <asp:Label ID="lblTypeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkAdminAssetName" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDNAME").ToString().Length>20 ? DataBinder.Eval(Container, "DataItem.FLDNAME").ToString().Substring(0, 20)+ "..." : DataBinder.Eval(Container, "DataItem.FLDNAME").ToString() %>' ></asp:LinkButton>
                                     <eluc:ToolTip ID="ucName" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'  />      
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="Asset Maker">
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
                            <asp:TemplateField FooterText="Asset Quantity">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAssetQuantityHeader" runat="server" CommandName="Sort" CommandArgument="FLDQUANTITY"
                                        ForeColor="White">Quantity&nbsp;</asp:Label>
                                    <img id="Img7" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>
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
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Software Mapping" ImageUrl="<%$ PhoenixTheme:images/checklist.png %>"
                                                CommandName="SOFTWAREMAPPING" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSoftwareMapping"
                                                ToolTip="Map Software"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
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
