<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAdminAssetLicense.aspx.cs"
    Inherits="RegistersAdminAssetLicense" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Asset Licnese</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
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
                        <eluc:Title runat="server" ID="ucTitle" Text="License" />
                    </div>
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuAsset" runat="server" OnTabStripCommand="Asset_TabStripCommand"
                        TabStrip="true" Visible="false"></eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureAdminAsset" width="100%">
                        <tr>
                            <td>
                                Name
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblAssetType" runat="server" Text="Asset Type"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAssetType" runat="server" CssClass="input" Width="120px"
                                    DataValueField="FLDASSETTYPEID" DataTextField="FLDNAME"  AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSerialNo" runat="server" Text='Product Key No'></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSerailNo" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="lblIdentificationNo" runat="server" Text='License No'></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdentificationNo" runat="server" MaxLength="100" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuRegistersAdminAsset" runat="server" OnTabStripCommand="MenuRegistersAdminAsset_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvAdminAsset" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvAdminAsset_RowCommand" OnRowDataBound="gvAdminAsset_RowDataBound"
                        OnRowDeleting="gvAdminAsset_RowDeleting" OnRowEditing="gvAdminAsset_RowEditing"
                        OnRowCancelingEdit="gvAdminAsset_RowCancelingEdit" OnRowUpdating="gvAdminAsset_Rowupdating"
                        ShowFooter="false" ShowHeader="true" OnSorting="gvAdminAsset_Sorting" AllowSorting="true"
                        EnableViewState="false">
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
                                        ForeColor="White">Asset Type&nbsp;</asp:Label>
                                    <img id="Img8" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDASSETTYPENAME")%>
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
                                    <asp:LinkButton ID="lnkAdminAssetName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDNAME").ToString().Length>20 ? DataBinder.Eval(Container, "DataItem.FLDNAME").ToString().Substring(0, 20)+ "..." : DataBinder.Eval(Container, "DataItem.FLDNAME").ToString() %>'></asp:LinkButton>
                                    <eluc:Tooltip ID="ucName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="Asset Serial No">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAssetSerialNoHeader" runat="server" CommandName="Sort" CommandArgument="FLDSERIALNO"
                                        ForeColor="White">Product<br />Key No&nbsp;</asp:Label>
                                    <img id="Img5" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDSERIALNO")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="Asset Identification Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAssetIdentificationNumberHeader" runat="server" CommandName="Sort"
                                        CommandArgument="FLDIDENTIFICATIONNUMBER" ForeColor="White">License<br />No&nbsp;</asp:Label>
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
                                        ForeColor="White">Copies Licensed or <br />Maintained&nbsp;</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDQUANTITY")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="Version">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAssetVersionHeader" runat="server" CommandName="Sort" CommandArgument="FLDVERSION"
                                        ForeColor="White">Version&nbsp;</asp:Label>
                                    <img id="Img7" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDVERSION")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterText="Asset Description" >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAssetDesHeader" runat="server" CommandName="Sort" CommandArgument="FLDDESCRIPTION"
                                        ForeColor="White">Description&nbsp;</asp:Label>
                                    <img id="Img16" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION").ToString().Length>20 ? DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION").ToString().Substring(0, 20)+ "..." : DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION").ToString() %>'></asp:Label>
                                    <eluc:Tooltip ID="ucDescTooltip" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>' />
                                    <%-- <%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>--%>
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
                <div id="divPage" style="position: relative;z-index:0">
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
