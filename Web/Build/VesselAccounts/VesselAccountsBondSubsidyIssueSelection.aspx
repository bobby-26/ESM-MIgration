<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsBondSubsidyIssueSelection.aspx.cs"
    Inherits="VesselAccountsBondSubsidyIssueSelection" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Store Item List</title>
  <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
   </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStoreItemList" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="ucTitle" Text="Store Item" ShowMenu="false" />
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuStockItem" runat="server" OnTabStripCommand="MenuStockItem_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlStockItem">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="search">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNumberSearch" CssClass="input_mandatory" MaxLength="20"
                                Width="80px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblStoreItemName" runat="server" Text="Store Item Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtStockItemNameSearch" CssClass="input" Width="240px"
                                Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblStoreType" runat="server" Text="Store Type"></asp:Literal>
                        </td>
                        <td colspan="3">
                            <eluc:UserControlHard ID="ddlStockClass" runat="server" Visible="true" CssClass="input"
                                AppendDataBoundItems="true" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:GridView ID="gvStoreItem" runat="server" AutoGenerateColumns="False" CellPadding="3"
                GridLines="None" Font-Size="11px" OnRowDataBound="gvStoreItem_RowDataBound" ShowFooter="false"
                OnRowEditing="gvStoreItem_RowEditing" OnRowCancelingEdit="gvStoreItem_RowCancelingEdit"
                OnRowUpdating="gvStoreItem_RowUpdating" ShowHeader="true" Width="100%" EnableViewState="false"
                AllowSorting="true" OnSorting="gvStoreItem_Sorting" DataKeyNames="FLDSTOREITEMID">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                    <asp:TemplateField HeaderText="Number">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lblnumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDNUMBER">Number&nbsp;</asp:LinkButton>
                            <img id="FLDNUMBER" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblStockItemNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="StoreItem Name">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lblStoreItemNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME">Store Item Name&nbsp;</asp:LinkButton>
                            <img id="FLDNAME" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Maker">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lblMakerHeader" runat="server" CommandName="Sort" CommandArgument="FLDMAKER">Maker&nbsp;</asp:LinkButton>
                            <img id="FLDMAKER" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDMAKER") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ROB">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDROB").ToString().TrimEnd('0').TrimEnd('.') %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDROB").ToString().TrimEnd('0').TrimEnd('.') %>
                            <asp:Label ID="lblRob" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB").ToString() %>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtQuantity" runat="server" CssClass="input" Width="80px" IsInteger="true"
                                IsPositive="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Issue Date">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Date ID="txtIssueDate" runat="server" CssClass="input" Text='<%# DateTime.Now.ToString("dd/MM/yyyy") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="No of Cans">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtNoofCans" runat="server" CssClass="input" Width="80px" IsInteger="true"
                                IsPositive="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remarks">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRemarks" runat="server"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdUpdate"
                                ToolTip="Update"></asp:ImageButton>
                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <table width="100%" border="0" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red">Red Line item is not in Market. </asp:Label>
                    </td>
                </tr>
            </table>
            <div id="divPage" style="position: relative;">
                <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
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
                            <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev"><< Prev </asp:LinkButton>
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
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvStoreItem" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
