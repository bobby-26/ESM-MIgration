<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelQuoteList.aspx.cs" Inherits="CrewTravelQuoteList" %>


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register Src="../UserControls/UserControlCurrency.ascx" TagName="UserControlCurrency"
    TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Travel Request Quotation</title>
          <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
    
</head>
<body>
<div style="margin: 0 auto; width: 1024px; text-align: left;">
    <form id="form1" runat="server">
     <div style="height: 60px;" class="pagebackground">
            <div style="position: absolute; top: 15px;">
                <img id="Img1" runat="server" style="vertical-align: middle" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>"
                    alt="Phoenix" onclick="parent.hideMenu();" />
                <span class="title" style="color: White">
                    <%=Application["softwarename"].ToString() %></span><asp:Label runat="server" ID="lblDatabase"
                        ForeColor="Red" Font-Size="Large" Visible="false" Text="Testing on "></asp:Label><br />
            </div>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false"></ajaxToolkit:ToolkitScriptManager>
            <asp:UpdatePanel runat="server" ID="pnlLineItemEntry">
                <ContentTemplate>
                    <div class="navigation" id="navigation" style="width: 1024px; top: 60px;">
                        <div class="subHeader" style="position: relative">
                            <asp:Button runat="server" ID="cmdHiddenSubmit1" OnClick="cmdHiddenSubmit_Click" />
                            <eluc:Title runat="server" ID="Title1" Text="Travel Request" ShowMenu="false"></eluc:Title>
                        </div>
                        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                            <eluc:TabStrip ID="MenuQuotationLineItem" runat="server" OnTabStripCommand="MenuQuotationLineItem_TabStripCommand"></eluc:TabStrip>
                        </div>
                        <br clear="all" />
                        <div>
                            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                            <eluc:Status runat="server" ID="ucStatus" />
                            <br />
                            <b><asp:Literal ID="lblPassengerList" runat="server" Text="Passenger List"></asp:Literal></b>
                            <div class="navSelect" style="position: relative; width: 15px">
                                <eluc:TabStrip ID="MenuBreakUpAssign" runat="server"></eluc:TabStrip>
                            </div>
                            <asp:GridView ID="gvLineItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" >
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblTravelID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblRequestID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblBreakupID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBREAKUPID") %>'
                                                Visible="false"></asp:Label>
                                        
                                            <asp:Label ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                                CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lnkOriginHeader" runat="server" ForeColor="White">Origin</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrgin" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGIN") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblRouteIDEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROUTEID") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblQuotationIDEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEID") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblTravelIDEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblRequestIDEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblBreakupIDEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBREAKUPID") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label runat="server" ID="txtOrigin"  
                                                Text='<%# DataBinder.Eval(Container, "DataItem.FLDORIGIN")%>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDestinationHeader" runat="server" ForeColor="White">Destination</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDestination" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATION") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label runat="server" ID="txtDestination" 
                                                Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESTINATION")%>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDepartureDateHeader" runat="server" ForeColor="White">Departure Date</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDepartureDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MMM/yyyy hh:mm tt}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                             <asp:Label ID="lblDepartureDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MMM/yyyy }") %>'></asp:Label>
                                            
                                            <asp:TextBox ID="txtDepartureTime" runat="server" CssClass="input_mandatory" Width="50px"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:hh:mm tt}") %>' />
                                            <ajaxToolkit:MaskedEditExtender ID="txtDepartureTimeMaskEdit" runat="server" TargetControlID="txtDepartureTime"
                                                Mask="99:99" MaskType="Time" AcceptAMPM="true" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblArrivalDateHeader" runat="server" ForeColor="White">Arrival Date</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblArrivalDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE","{0:dd/MMM/yyyy hh:mm tt}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                              <asp:Label ID="lblArrivalDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                            <asp:TextBox ID="txtArrivalTime" runat="server" CssClass="input_mandatory" Width="50px"
                                                Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:hh:mm tt}") %>' />
                                            <ajaxToolkit:MaskedEditExtender ID="txtArrivalTimeMaskEdit" runat="server" TargetControlID="txtArrivalTime"
                                                Mask="99:99" MaskType="Time" AcceptAMPM="true" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div id="div3" style="position: relative;">
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
                            <br />
                            <br />
                            <br />
                            <br />
                            <b><asp:Literal ID="lblQuotationList" runat="server" Text="Quotation List"></asp:Literal></b>
                            <div id="div2" style="position: relative; z-index: 0; width: 100%;">
                                <asp:GridView ID="gvAgent" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="3" EnableViewState="False"  AllowSorting="true" OnRowCommand="gvAgent_RowCommand"
                                   >
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <RowStyle Height="10px" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblQuotationNoHeader" CommandArgument="FLDQUOTATIONNO" runat="server"
                                                    CommandName="Sort" ForeColor="White">Quotation Number</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuotationID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEID") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblTravelID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblRequestID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblAgentID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTID") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:LinkButton ID="lblQuotationNo" CommandName="Select" CommandArgument='<%# Container.DataItemIndex %>'
                                                    runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONREFNO") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                           
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblQuotationCurrencyHeader" CommandArgument="FLDCURRENCYID" runat="server"
                                                    CommandName="Sort" ForeColor="White">Currency</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="ucCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></asp:Label>
                                            </ItemTemplate>
                                           
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblStatusHeader" runat="server" ForeColor="White">Status</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONSTATUS") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lnkAmountHeader" CommandArgument="FLDAMOUNT" runat="server" CommandName="Sort"
                                                    ForeColor="White">Amount</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lnkTaxHeader" CommandArgument="FLDTAX" runat="server" CommandName="Sort"
                                                    ForeColor="White">Tax</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblTotalAmountHeader" runat="server" ForeColor="White">Total Amount</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="div4" style="position: relative;">
                                <table width="100%" border="0" class="datagrid_pagestyle">
                                    <tr>
                                        <td nowrap="nowrap" align="center">
                                            <asp:Label ID="lblQuotePagenumber" runat="server">
                                            </asp:Label>
                                            <asp:Label ID="lblQuotePages" runat="server">
                                            </asp:Label>
                                            <asp:Label ID="lblQuoteRecords" runat="server">
                                            </asp:Label>&nbsp;&nbsp;
                                        </td>
                                        <td nowrap="nowrap" align="left" width="50px">
                                            <asp:LinkButton ID="cmdQuotePrevious" runat="server" OnCommand="QuotePagerButtonClick"
                                                CommandName="prev">Prev << </asp:LinkButton>
                                        </td>
                                        <td width="20px">
                                            &nbsp;
                                        </td>
                                        <td nowrap="nowrap" align="right" width="50px">
                                            <asp:LinkButton ID="cmdQuoteNext" OnCommand="QuotePagerButtonClick" runat="server"
                                                CommandName="next">Next >></asp:LinkButton>
                                        </td>
                                        <td nowrap="nowrap" align="center">
                                            <asp:TextBox ID="txtQuotenopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                            </asp:TextBox>
                                            <asp:Button ID="btnQuoteGo" runat="server" Text="Go" OnClick="cmdQuoteGo_Click" CssClass="input"
                                                Width="40px"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divPage" style="position: relative;">
                                <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                                    <tr>
                                        <td width="20px">
                                            <input type="hidden" id="isouterpage" name="isouterpage" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            
            </div>
        </form>
    </div>
</body>
</html>
