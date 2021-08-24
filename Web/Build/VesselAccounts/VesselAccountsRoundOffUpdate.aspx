<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsRoundOffUpdate.aspx.cs"
    Inherits="VesselAccountsRoundOffUpdate" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
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
    <form id="frmBondReq" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlComponent">
            <ContentTemplate>
                <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
                    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                        <div class="subHeader" style="position: relative">
                            <eluc:Title runat="server" ID="Title1" Text="Requisition Bond and Provisions" ShowMenu="false"></eluc:Title>
                        </div>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="OrderForm_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <div style="position: relative; overflow: hidden; clear: right;">
                    </div>
                    <div class="navSelect" style="position: relative; clear: both; width: 15px">
                        <eluc:TabStrip ID="MenuBondReq" runat="server" OnTabStripCommand="MenuBondReq_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvBondReq" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            GridLines="None" Width="100%" CellPadding="3" OnRowDataBound="gvBondReq_RowDataBound"
                            ShowHeader="true" OnRowCommand="gvBondReq_RowCommand" EnableViewState="false"
                            AllowSorting="true" OnSorting="gvBondReq_Sorting" OnSelectedIndexChanging="gvBondReq_SelectedIndexChanging"
                            OnRowEditing="gvBondReq_RowEditing" OnRowCancelingEdit="gvBondReq_RowCancelingEdit"
                            DataKeyNames="FLDORDERID">
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
                                        <%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDORDERDATE"]) %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtOrderDate" runat="server" CssClass="input_mandatory" Text='<%# string.Format("{0:dd/MM/yyyy}", ((DataRowView)Container.DataItem)["FLDORDERDATE"]) %>'>
                                        </asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="txtOrderDateMask" runat="server" TargetControlID="txtOrderDate"
                                            ClearTextOnInvalid="false" Mask="99/99/9999" MaskType="Date" ClearMaskOnLostFocus="false"
                                            InputDirection="RightToLeft" UserDateFormat="DayMonthYear">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblStockType" runat="server" CommandName="Sort" CommandArgument="FLDSTOCKTYPE">Stock Type</asp:LinkButton>
                                        <img id="FLDSTOCKTYPE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDSTOCKTYPE"] %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblReceiveDate" runat="server" CommandName="Sort" CommandArgument="FLDRECEIVEDDATE">Received Date</asp:LinkButton>
                                        <img id="FLDRECEIVEDDATE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDRECEIVEDDATE"])%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtReceivedDate" runat="server" CssClass="input_mandatory" Text='<%# string.Format("{0:dd/MM/yyyy}", ((DataRowView)Container.DataItem)["FLDRECEIVEDDATE"]) %>'>
                                        </asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="txtReceivedDateMask" runat="server" TargetControlID="txtReceivedDate"
                                            ClearTextOnInvalid="false" Mask="99/99/9999" MaskType="Date" ClearMaskOnLostFocus="false"
                                            InputDirection="RightToLeft" UserDateFormat="DayMonthYear">
                                        </ajaxToolkit:MaskedEditExtender>
                                    </EditItemTemplate>
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
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblDiscount" runat="server" Text="Discount"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDDISCOUNT"]%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDiscount" CssClass="input_mandatory" DecimalPlace="2" Text='<%# ((DataRowView)Container.DataItem)["FLDDISCOUNT"]%>'
                                            runat="server" MaxLength="19" Width="60px"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblRoundoff" runat="server" Text="Actual Rount Off"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDROUNDOFFAMOUNT"]%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtUnitPrice" CssClass="input_mandatory" DecimalPlace="2" Text='<%# ((DataRowView)Container.DataItem)["FLDROUNDOFFAMOUNT"]%>'
                                            runat="server" MaxLength="19" Width="60px"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblcurrency" runat="server" Text="Currency"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDCURRENCYNAME"]%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Currency ID="ddlCurrency" AppendDataBoundItems="true" CssClass="input_mandatory"
                                            runat="server" CurrencyList='<%#SouthNests.Phoenix.Registers.PhoenixRegistersCurrency.ListCurrency(null, null) %>'
                                            SelectedCurrency='<%#((DataRowView)Container.DataItem)["FLDCURRENCYID"]%>' />
                                    </EditItemTemplate>
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
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Round Off Update" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="CORRECT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCorrect"
                                            ToolTip="Round Off Update"></asp:ImageButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divPage" style="position: relative;">
                        <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                            <tr>
                                <td nowrap align="center">
                                    <asp:Label ID="lblPagenumber" runat="server"> </asp:Label>
                                    <asp:Label ID="lblPages" runat="server"> </asp:Label>
                                    <asp:Label ID="lblRecords" runat="server"> </asp:Label>&nbsp;&nbsp;
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
                                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input"> </asp:TextBox>
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
