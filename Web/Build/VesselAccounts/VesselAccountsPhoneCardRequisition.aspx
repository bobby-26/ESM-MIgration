<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsPhoneCardRequisition.aspx.cs"
    Inherits="VesselAccountsPhoneCardRequisition" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Phone Card Requisition</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBondReq" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlPhonReq">
            <ContentTemplate>
                <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
                    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                        <div class="subHeader" style="position: relative; right: 0px">
                            <eluc:Title runat="server" ID="Title1" Text="Phone Card Requisition" ShowMenu="<%# Title1.ShowMenu %>"></eluc:Title>
                            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="OrderForm_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <div style="position: relative; overflow: hidden; clear: right;">
                        <iframe runat="server" id="ifMoreInfo" style="min-height: 275px; width: 100%; overflow-x: hidden"></iframe>
                    </div>
                    <div class="navSelect" style="position: relative; clear: both; width: 15px">
                        <eluc:TabStrip ID="MenuBondReq" runat="server" OnTabStripCommand="MenuBondReq_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvPhonReq" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            GridLines="None" Width="100%" CellPadding="3" OnRowDataBound="gvPhonReq_RowDataBound"
                            ShowHeader="true" OnRowUpdating="gvPhonReq_RowUpdating" EnableViewState="false"
                            AllowSorting="true" OnSorting="gvPhonReq_Sorting" OnSelectedIndexChanging="gvPhonReq_SelectedIndexChanging"
                            DataKeyNames="FLDREQUESTID" OnRowCommand="gvPhonReq_RowCommand">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkRefNoHeader" runat="server" CommandName="Sort" CommandArgument="FLDREFERENCENO">Request No</asp:LinkButton>
                                        <img id="FLDREFERENCENO" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTID"] %>'></asp:Label>
                                        <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="select"><%# ((DataRowView)Container.DataItem)["FLDREFERENCENO"] %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblOrderDate" runat="server" CommandName="Sort" CommandArgument="FLDREQUESTDATE">Order Date</asp:LinkButton>
                                        <img id="FLDREQUESTDATE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDREQUESTDATE"]) %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblOrderStatus" runat="server" CommandName="Sort" CommandArgument="FLDREQUESTSTATUS">Request Status</asp:LinkButton>
                                        <img id="FLDREQUESTSTATUS" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%--<asp:Label ID="LblReceiveDate" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDRECEIVEDDATE"]%>'></asp:Label>--%>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTSTATUS"]%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                            Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Confirm" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                            CommandName="APPROVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdApprove"
                                            ToolTip="Approve Requset"></asp:ImageButton>
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="ORDERCANCEL" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                            ToolTip="Cancel"></asp:ImageButton>
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
                                <td width="20px">&nbsp;
                                </td>
                                <td nowrap align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap align="center">
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
