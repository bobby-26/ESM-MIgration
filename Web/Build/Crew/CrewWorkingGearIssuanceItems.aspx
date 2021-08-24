<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewWorkingGearIssuanceItems.aspx.cs"
    Inherits="CrewWorkingGearIssuanceItems" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkingGearType" Src="~/UserControls/UserControlWorkingGearType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Working Gear Items List</title>
       <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">      
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrew">
        <ContentTemplate>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="" ShowMenu="false"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuWorkingGearItem" runat="server" OnTabStripCommand="MenuWorkingGearItem_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="search">
                    <div style="color: Blue;">
                        1. By default, it will shows all the working gear items.
                        <br />
                        2. The Working Gear Item already issued for this crew change won't show here
                        <br />
                        3. But you can search he items using below filters
                    </div>
                    <br style="clear: both;" />
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblWorkingGearItemName" runat="server" Text="Working Gear Item Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemSearch" runat="server" MaxLength="100" CssClass="input" Width="300px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblGearType" runat="server" Text="Gear Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:WorkingGearType ID="ucWorkingGearType" CssClass="input" AppendDataBoundItems="true"
                                    runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br style="clear: both" />
                <div id="divGrid" runat="server" style="position: relative; overflow: auto; z-index: 0">
                    <asp:GridView ID="gvWorkGear" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        RowStyle-Wrap="false" Width="100%" CellPadding="3" ShowHeader="true" OnRowDataBound="gvWorkGear_RowDataBound"
                        EnableViewState="false" AllowSorting="true" OnSorting="gvWorkGear_Sorting" OnRowEditing="gvWorkGear_RowEditing"
                        OnRowCancelingEdit="gvWorkGear_RowCancelingEdit" OnRowUpdating="gvWorkGear_RowUpdating">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblItemHeader" runat="server">Item</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblItem" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblItemIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblItemEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSizeHeader" runat="server">Size</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSize" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZENAME") %>' ></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblSizeIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZEID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblSizeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZENAME") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRemainingQuantityHeader" runat="server" >Remaining Quantity</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRemainingQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALQUANTITY") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblIssuingQuantityHeader" runat="server">Issuing Quantity</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIssuingQuantity" Visible="true" runat="server" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtIssuingQuantityEdit" runat="server" CssClass="input_mandatory"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">Action </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
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
                </div>
                <div id="divPage" runat="server" style="position: relative;">
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
