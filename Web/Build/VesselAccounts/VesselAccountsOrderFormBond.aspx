<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsOrderFormBond.aspx.cs"
    Inherits="VesselAccountsOrderFormBond" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmBondReq" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlComponent">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative; right: 0px">
                        <eluc:Title runat="server" ID="Title1" Text="" ShowMenu="<%# Title1.ShowMenu %>"></eluc:Title>
                    </div>
                    <div id="divMain" runat="server" style="width: 99%; padding: 5px 5px 5px 5px;">
                        <%-- <fieldset>
                        <legend>
                            <asp:Label ID="lblhead" runat="server" Text='Filter'></asp:Label>
                        </legend>--%>
                        <table width="90%">
                            <tr>
                                <td colspan="6">
                                    <font color="blue"><b>
                                        <asp:Literal ID="lblNote" runat="server" Text="Note:"></asp:Literal>
                                    </b>
                                        <asp:Literal ID="lblForembedded" runat="server" Text="For embedded search, use '%' symbol. (Eg. Order No.: %xxxx)"></asp:Literal></font>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblRequestNo" runat="server" Text="Order No."></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtRefNo" MaxLength="50" CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblFromDate" runat="server" Text="Order Between"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                                    <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblOrderStatus" runat="server" Text="Order Status"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Hard ID="ddlStatus" runat="server" AppendDataBoundItems="true" CssClass="input"
                                        HardTypeCode="41" ShortNameFilter="PEN,RCD,CNL" />
                                </td>
                            </tr>
                        </table>
                        <%-- </fieldset>--%>
                    </div>
                    <div class="navSelect" style="position: relative; clear: both; width: 15px">
                        <eluc:TabStrip ID="MenuBondReq" runat="server" OnTabStripCommand="MenuBondReq_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvBondReq" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            GridLines="None" Width="100%" CellPadding="3" OnRowDataBound="gvBondReq_RowDataBound"
                            ShowHeader="true" OnRowCommand="gvBondReq_RowCommand" EnableViewState="false" OnRowDeleting="gvBondReq_RowDeleting"
                            AllowSorting="true" OnSorting="gvBondReq_Sorting" DataKeyNames="FLDORDERID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkRefNoHeader" runat="server" CommandName="Sort" CommandArgument="FLDREFERENCENO">Order No</asp:LinkButton>
                                        <img id="FLDREFERENCENO" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDORDERID"] %>'></asp:Label>
                                        <asp:Label ID="lbldtkey" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDDTKEY"] %>'></asp:Label>
                                        <asp:LinkButton ID="lnkRefNo" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>"><%# ((DataRowView)Container.DataItem)["FLDREFERENCENO"] %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblOrderDate" runat="server" CommandName="Sort" CommandArgument="FLDORDERDATE">Order Date</asp:LinkButton>
                                        <img id="FLDORDERDATE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDORDERDATE", "{0:dd/MMM/yyyy}"))%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblOrderStatus" runat="server" CommandName="Sort" CommandArgument="FLDORDERSTATUS">Order Status</asp:LinkButton>
                                        <img id="FLDORDERSTATUS" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDORDERSTATUS"]%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblReceiveDate" runat="server" CommandName="Sort" CommandArgument="FLDRECEIVEDDATE">Received On</asp:LinkButton>
                                        <img id="FLDRECEIVEDDATE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRECEIVEDDATE", "{0:dd/MMM/yyyy}"))%>
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

                                        <asp:ImageButton runat="server" AlternateText="Details" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png %>"
                                            CommandName="SELECT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDetails"
                                            ToolTip="Requisition of Bond"></asp:ImageButton>
                                        <img id="Img3" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/copy.png %>"
                                            CommandName="COPY" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCopy"
                                            ToolTip="Copy Requisition"></asp:ImageButton>
                                        <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                            ToolTip="Attachment"></asp:ImageButton>

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
                    <eluc:Status runat="server" ID="ucStatus" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
