<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelInvoiceLineItem.aspx.cs"
    Inherits="CrewTravelInvoiceLineItem" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice</title>
      <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

   </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewTravelInvoiceLineItem" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewTravelInvoiceLineItem">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading" style="vertical-align: top">
                            <asp:Literal ID="lblPurchaseOrderDetails" runat="server" Text="Purchase Order Details"></asp:Literal>
                        </div>
                    </div>
                </div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuHopItem" runat="server" OnTabStripCommand="MenuHopItem_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div>
                    <table cellpadding="2" cellspacing="1" style="width: 100%">
                        <tr>
                            <td width="15%">
                                <asp:Literal ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></asp:Literal>
                            </td>
                            <td width="35%">
                                <asp:TextBox ID="txtInvoiceNumber" runat="server" MaxLength="25" ReadOnly="true"
                                    CssClass="readonlytextbox" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <asp:Literal ID="lblSupplierCode" runat="server" Text="Supplier Code"></asp:Literal>
                            </td>
                            <td>
                                <span id="spnPickListMaker">
                                    <asp:TextBox ID="txtVendorCode" runat="server" Width="60px" CssClass="readonlytextbox"
                                        ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txtVenderName" runat="server" Width="180px" CssClass="readonlytextbox"
                                        ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txtVendorId" runat="server" Width="10px"></asp:TextBox>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblInvoiceReference" runat="server" Text="Invoice Reference"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSupplierRefEdit" runat="server" CssClass="readonlytextbox" MaxLength="25"
                                    ReadOnly="true" Width="240px"></asp:TextBox>
                                <asp:TextBox ID="txtCurrencyId" runat="server" CssClass="readonlytextbox" MaxLength="25"
                                    ReadOnly="true" Width="240px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCurrencyCode" runat="server" Text="Currency Code"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCurrencyName" runat="server" CssClass="readonlytextbox" MaxLength="25"
                                    ReadOnly="true" Width="240px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <hr />
                <br />
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvHopList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvHopList_RowCommand" ShowFooter="false"
                        ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="SELECT" Visible="false" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCheckedLineItem" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lnkOriginHeader" runat="server" ForeColor="White">Origin</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblRouteID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROUTEID") %>'
                                        Visible="false"></asp:Label>--%>
                                    <asp:Label ID="lblRowID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblRouteID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROUTEID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblTravelID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblRequestID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblBreakupID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBREAKUPID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblOrgin" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGIN") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDestinationHeader" runat="server" ForeColor="White">Destination</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDestination" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATION") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDepartureDateHeader" runat="server" ForeColor="White">Departure Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDepartureDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MMM/yyyy hh:mm tt}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblArrivalDateHeader" runat="server" ForeColor="White">Arrival Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblArrivalDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE","{0:dd/MMM/yyyy hh:mm tt}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAirlineCodeHeader" runat="server" ForeColor="White">Airline Code</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAirlineCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRLINECODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblClassHeader" runat="server" ForeColor="White">Class</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAmountHeader" runat="server" ForeColor="White">Amount</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTaxHeader" runat="server" ForeColor="White">Ticket Number</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTicket" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
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
        <Triggers>
            <asp:PostBackTrigger ControlID="gvHopList" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
