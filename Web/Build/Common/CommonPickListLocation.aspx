<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListLocation.aspx.cs"
    Inherits="CommonPickListLocation" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
            <eluc:Title runat="server" ID="ucTitle" Text="Location" ShowMenu="false" />
        </div>
    </div>
    
     <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuComponent" runat="server" OnTabStripCommand="MenuComponent_TabStripCommand">
        </eluc:TabStrip>
    </div>
    
     <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlComponet">
        <ContentTemplate>
         <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="search">
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblLocationName" runat="server" Text="Location Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLoctionNameSearch" CssClass="input" runat="server" Text=""></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblLocationCode" runat="server" Text="Location Code"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLocationCodeSearch" CssClass="input" runat="server"   Text=""></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divGrid" style="position: relative;">
                <asp:GridView ID="gvLocation" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    Font-Size="11px" OnRowCommand="gvLocation_RowCommand" OnRowDataBound="gvLocation_ItemDataBound" 
                    OnRowEditing="gvLocation_RowEditing" ShowFooter="true" ShowHeader="true" Width="100%"
                    EnableViewState="false"  AllowSorting="true" OnSorting="gvLocation_Sorting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                            
                             <asp:LinkButton ID="lblLocationCodeHeader" runat="server" CommandName="Sort" CommandArgument="FLDLOCATIONCODE"
                                    ForeColor="White">Location Code&nbsp;</asp:LinkButton>
                                <img id="FLDLOCATIONCODE" runat="server" visible="false" />
                            
                            
                           <%--     <asp:Label ID="lblLocationCodeHeader" runat="server">Location Code
                                    <asp:ImageButton runat="server" ID="ibtnLocationCodeDes" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>"
                                        OnClick="cmdSort_Click" CommandName="FLDLOCATIONCODE" CommandArgument="1" />
                                    <asp:ImageButton runat="server" ID="ibtLocationCodeAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>" OnClick="cmdSort_Click"
                                        CommandName="FLDLOCATIONCODE" CommandArgument="0" />
                                </asp:Label>--%>
                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblLocationId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDLOCATIONID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="lblLocationCode" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDLOCATIONCODE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblLocationNameHeader" runat="server">Location Name</asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblLocationName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'
                                    Visible="false"></asp:Label>
                                <asp:LinkButton ID="lnkLocationName" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'></asp:LinkButton>
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
            <asp:PostBackTrigger ControlID="gvLocation" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
