<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardBroadcast.aspx.cs" Inherits="Dashboard_PhoenixBroadcast" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <asp:UpdatePanel ID="pnlBroadcast" runat="server">
        <ContentTemplate>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

    <div>
                        <asp:GridView ID="gvPhoenixNews" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" BorderColor="Transparent">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle HorizontalAlign="Center" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblMessage" runat="server">Messages
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table width="100%" cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td width="10%">
                                            <asp:Label runat="server" ID="lblSubject" Font-Size="Larger" Font-Bold="true" ForeColor="Navy" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBJECT") %>'></asp:Label>
                                            </td>
                                            <td>
                                            [ <asp:Label runat="server" ID="lblUser" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBY") %>'></asp:Label> ]
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label runat="server" ID="lblDate" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE") + " (GMT)" %>'></asp:Label></td>                                            
                                            <td><asp:Label runat="server" ID="lblMessage" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMESSAGE") %>'></asp:Label></td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

    </div>
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
                                <asp:TextBox ID="txtnopage" MaxLength="5" Width="20px" runat="server" CssClass="input">
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
