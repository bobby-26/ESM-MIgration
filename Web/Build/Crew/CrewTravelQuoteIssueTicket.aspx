<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelQuoteIssueTicket.aspx.cs"
    Inherits="CrewTravelQuoteIssueTicket" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Issue Ticket</title>
         <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

   </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlLineItemEntry">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="Title1" Text="Ticket Details" ShowMenu="false"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuRouting" runat="server" OnTabStripCommand="MenuRouting_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuTicketNoAssign" runat="server"></eluc:TabStrip>
                </div>
                <br />
                <br />
                <div id="div2" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvRoutingDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" EnableViewState="False" AllowSorting="true" OnRowDataBound="gvRoutingDetails_RowDatabound"
                        OnRowEditing="gvRoutingDetails_RowEditing" OnRowCancelingEdit="gvAgentLineItem_RowCancelingEdit"
                        OnRowUpdating="gvAgentLineItem_RowUpdating" OnRowCommand="gvRoutingDetails_RowCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <%-- <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDestinationHeader" runat="server" ForeColor="White">Name</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDestination" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                </ItemTemplate>                               
                            </asp:TemplateField>--%>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lnkOriginHeader" runat="server" ForeColor="White">Origin</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
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
                                    <asp:TextBox runat="server" ID="txtOriginOld" CssClass="readonlytextbox" ReadOnly="true"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDORIGIN")%>'></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:TextBox runat="server" ID="txtOrigin" CssClass="input_mandatory"></asp:TextBox>                                  
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
                                    <asp:TextBox runat="server" ID="txtDestinationOld" CssClass="readonlytextbox"></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:TextBox runat="server" ID="txtDestination" CssClass="input_mandatory" ReadOnly="true"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESTINATION")%>'></asp:TextBox>
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
                                    <eluc:Date runat="server" ID="txtDepartureDateOld" Width="60px" CssClass="readonlytextbox"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MMM/yyyy}") %>'>
                                    </eluc:Date>
                                    <asp:TextBox ID="txtDepartureTimeOld" runat="server" CssClass="readonlytextbox" Width="50px"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:hh:mm tt}") %>' />
                                    <ajaxToolkit:MaskedEditExtender ID="txtDepartureTimeMaskEditold" runat="server" TargetControlID="txtDepartureTimeOld"
                                        Mask="99:99" MaskType="Time" AcceptAMPM="false" ClearMaskOnLostFocus="false"
                                        ClearTextOnInvalid="false" />
                                    <br />
                                    <br />
                                    <eluc:Date runat="server" ID="txtDepartureDate" Width="60px" CssClass="input_mandatory">
                                    </eluc:Date>
                                    <asp:TextBox ID="txtDepartureTime" runat="server" CssClass="input_mandatory" Width="50px"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:hh:mm tt}") %>' />
                                    <ajaxToolkit:MaskedEditExtender ID="DEPTMaskedEditExtender1" runat="server" TargetControlID="txtDepartureTime"
                                        Mask="99:99" MaskType="Time" AcceptAMPM="false" ClearMaskOnLostFocus="false"
                                        ClearTextOnInvalid="false" />
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
                                    <eluc:Date runat="server" ID="txtArrivalDateOld" Width="60px" CssClass="readonlytextbox">
                                    </eluc:Date>
                                    <asp:TextBox ID="txtArrivalTimeOld" runat="server" CssClass="readonlytextbox" Width="50px"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE","{0:hh:mm tt}") %>' />
                                    <ajaxToolkit:MaskedEditExtender ID="ARRIVALOLDMaskedEditExtender1" runat="server" TargetControlID="txtArrivalTimeOld"
                                        Mask="99:99" MaskType="Time" AcceptAMPM="false" ClearMaskOnLostFocus="false"
                                        ClearTextOnInvalid="false" />
                                    <br />
                                    <br />
                                    <eluc:Date runat="server" ID="txtArrivalDate" Width="60px" CssClass="input_mandatory"
                                        Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MMM/yyyy}") %>'>
                                    </eluc:Date>
                                    <asp:TextBox ID="txtArrivalTime" runat="server" CssClass="input_mandatory" Width="50px"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE","{0:hh:mm tt}") %>' />
                                    <ajaxToolkit:MaskedEditExtender ID="ARRIVALMaskedEditExtender2" runat="server" TargetControlID="txtArrivalTime"
                                        Mask="99:99" MaskType="Time" AcceptAMPM="false" ClearMaskOnLostFocus="false"
                                        ClearTextOnInvalid="false" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAirlineCodeHeader" runat="server" ForeColor="White">Airline Code</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAirlineCode" runat="server" Width="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRLINECODE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtAirlineCodeOld" CssClass="input_mandatory" Width="60px" runat="server"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRLINECODE") %>'></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:TextBox ID="txtAirlineCode" CssClass="input_mandatory" Width="60px" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblClassHeader" runat="server" ForeColor="White">Class</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASS") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtClassOld" runat="server" Width="60px" CssClass="input_mandatory"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASS") %>'></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:TextBox ID="txtClass" runat="server" Width="60px" CssClass="input_mandatory"></asp:TextBox>
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
                                    <eluc:Decimal ID="txtAmountOld" CssClass="input_mandatory" Width="60px" runat="server"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' />
                                    <br />
                                    <br />
                                    <eluc:Decimal ID="txtAmount" CssClass="input_mandatory" Width="60px" runat="server" />
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
                                    <eluc:Decimal ID="txtTaxOld" CssClass="input_mandatory" Width="60px" runat="server"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>' />
                                    <br />
                                    <br />
                                    <eluc:Decimal ID="txtTax" CssClass="input_mandatory" Width="60px" runat="server" />
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
                                    <asp:ImageButton runat="server" AlternateText="Select" ImageUrl="<%$ PhoenixTheme:images/travel-breakup.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdTravelBreakUp"
                                        ToolTip="Add Break Jaurney"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save Break Up"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Save" Visible="false" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
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
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
