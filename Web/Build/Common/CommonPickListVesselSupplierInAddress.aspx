<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListVesselSupplierInAddress.aspx.cs"
    Inherits="CommonPickListVesselSupplierInAddress" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Address List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="ucTitle" Text="Vessel Supplier" ShowMenu="false" />
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuAddress" runat="server" OnTabStripCommand="MenuAddress_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlAddressEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="search">
                <table cellpadding="1" cellspacing="1" width="50%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNameSearch" CssClass="input" runat="server" Text=""></asp:TextBox>
                        </td>                        
                        <td>
                            <asp:Literal ID="lblCountry" runat="server" Text="Country"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Country runat="server" ID="ddlCountry" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divGrid" style="position: relative;">
                <asp:GridView ID="gvAddress" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    Font-Size="11px" OnRowCommand="gvAddress_RowCommand" OnRowDataBound="gvAddress_RowDataBound"
                    OnSorting="gvAddress_Sorting" AllowSorting ="true"  ShowFooter="False"
                    ShowHeader="true" Width="100%" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblCodeHeader" runat="server">Code </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblAddressNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                    ForeColor="White">Name&nbsp;</asp:LinkButton>
                                <img id="FLDNAME" runat="server" visible="false" />                              
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSupplierCode" Text='<%# Bind("FLDSUPPLIERCODE") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="lblSupplierAddressCode" Text='<%# Bind("FLDADDRESSCODE") %>'
                                    Visible="false"></asp:Label>
                                <asp:LinkButton ID="lnkSupplierName" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblPhone1Header" runat="server">Phone 1  </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDPHONE1").ToString().Replace('~', ' ') %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblCityHeader" runat="server"> City </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblStateHeader" runat="server"> State </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDSTATENAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblCountryHeader" runat="server"> Country </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblStatusHeader" runat="server"> Status </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
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
            <asp:PostBackTrigger ControlID="gvAddress" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
