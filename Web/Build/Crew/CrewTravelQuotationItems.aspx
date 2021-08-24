<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelQuotationItems.aspx.cs"
    Inherits="CrewTravelQuotationItems" %>

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
<body style="margin: 0; padding: 0px; text-align: center;">
    <div style="margin: 0 auto; width: 1024px; text-align: left;">
        <form id="frmCrewTravelQuotationItems" runat="server">
        <div style="height: 60px;" class="pagebackground">
            <div style="position: absolute; top: 15px;">
                <img id="Img1" runat="server" style="vertical-align: middle" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>"
                    alt="Phoenix" onclick="parent.hideMenu();" />
                <span class="title" style="color: White">
                    <%=Application["softwarename"].ToString() %></span><asp:Label runat="server" ID="lblDatabase"
                        ForeColor="Red" Font-Size="Large" Visible="false" Text="Testing on "></asp:Label><br />
            </div>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
            </ajaxToolkit:ToolkitScriptManager>
            <asp:UpdatePanel runat="server" ID="pnlLineItemEntry">
                <ContentTemplate>
                    <div class="navigation" id="navigation" style="width: 1024px; top: 60px;">
                        <div class="subHeader" style="position: relative">
                            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                            <eluc:Title runat="server" ID="Title1" Text="Travel Request" ShowMenu="false"></eluc:Title>
                        </div>
                        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                            <eluc:TabStrip ID="MenuQuotationLineItem" runat="server" OnTabStripCommand="MenuQuotationLineItem_TabStripCommand">
                            </eluc:TabStrip>
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
                                Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowCommand="gvLineItem_RowCommand"
                                OnRowCancelingEdit="gvLineItem_RowCancelingEdit" OnRowUpdating="gvLineItem_RowUpdating"
                                OnRowEditing="gvLineItem_RowEditing" OnRowDataBound="gvLineItem_RowDataBound"
                                OnPreRender="gvLineItem_PreRender">
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
                                            <asp:Label ID="lblRouteID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROUTEID") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblQuotationID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEID") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblTravelID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblRequestID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblBreakupID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBREAKUPID") %>'
                                                Visible="false"></asp:Label>
                                            <asp:Label ID="lblDTKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
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
                                            <asp:Label runat="server" ID="txtOrigin" Text='<%# DataBinder.Eval(Container, "DataItem.FLDORIGIN")%>'></asp:Label>
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
                                            <asp:Label runat="server" ID="txtDestination" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESTINATION")%>'></asp:Label>
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
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblNoOfStopsHeader" runat="server" ForeColor="White">Stops</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNoOfStops" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFSTOPS") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtNoOfStops" CssClass="input_mandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFSTOPS") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblDurationHeader" runat="server" ForeColor="White">Duration</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDuration" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDuration" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblAmountHeader" runat="server" ForeColor="White">Amount</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Decimal ID="txtAmount" CssClass="input_mandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblTaxHeader" runat="server" ForeColor="White">Tax</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Decimal ID="txtTax" CssClass="input_mandatory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>' />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblRouting" runat="server" ForeColor="White">Route</asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                ID="cmdShowReason" ImageUrl="<%$ PhoenixTheme:images/reschedule-remark.png %>"
                                                ImageAlign="AbsMiddle" Text=".." ToolTip="Ticket Details" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <span id="spnPickReason">
                                                <asp:TextBox ID="txtRouting" runat="server" Width="1px"></asp:TextBox>
                                                <asp:ImageButton runat="server" CommandArgument="<%# Container.DataItemIndex %>"
                                                    ID="cmdShowReasonEdit" ImageUrl="<%$ PhoenixTheme:images/reschedule-remark.png %>"
                                                    ImageAlign="AbsMiddle" Text=".." ToolTip="Ticket Details" />
                                            </span>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblActionHeader" runat="server">
                                                            Action
                                            </asp:Label>
                                        </HeaderTemplate>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Select" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                CommandName="EDITROW" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                                ToolTip="Edit"></asp:ImageButton>
                                            <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Instruction" ImageUrl="<%$ PhoenixTheme:images/add-instruction.png %>"
                                                CommandName="INSTRUCTION" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAddInstruction"
                                                ToolTip="Add Instruction"></asp:ImageButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Save" Visible="true" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                CommandName="Save" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdRowSave"
                                                ToolTip="Save"></asp:ImageButton>
                                            <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                                ToolTip="Cancel"></asp:ImageButton>
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
                            <b><asp:Literal ID="lblQuotationSummary" runat="server" Text="Quotation Summary"></asp:Literal></b>
                            <div id="div2" style="position: relative; z-index: 0; width: 100%;">
                                <asp:GridView ID="gvAgent" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="3" EnableViewState="False" ShowFooter="true" AllowSorting="true"
                                    OnRowDataBound="gvAgent_RowDataBound" OnRowCommand="gvAgent_RowCommand">
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
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtQuotationNo" runat="server" CssClass="input_mandatory" Width="120px"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblQuotationCurrencyHeader" CommandArgument="FLDCURRENCYID" runat="server"
                                                    CommandName="Sort" ForeColor="White">Currency</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="ucCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <eluc:UserControlCurrency ID="ucCurrency" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                                    runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"></eluc:UserControlCurrency>
                                            </FooterTemplate>
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
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:Label ID="lblActionHeader" runat="server">
                                                             Action
                                                </asp:Label>
                                            </HeaderTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" AlternateText="Confirm" ImageUrl="<%$ PhoenixTheme:images/select.png %>"
                                                    CommandName="CONFIRM" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdConfirm"
                                                    ToolTip="Confirm"></asp:ImageButton>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                                    CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                                    ToolTip="Add New"></asp:ImageButton>
                                            </FooterTemplate>
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
