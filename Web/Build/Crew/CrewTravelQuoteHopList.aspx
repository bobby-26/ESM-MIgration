<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelQuoteHopList.aspx.cs"
    Inherits="CrewTravelQuoteHopList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="../UserControls/UserControlMaskNumber.ascx" %>
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
                        <eluc:Title runat="server" ID="Title1" Text="Ticket Details"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuRouting" runat="server" OnTabStripCommand="MenuRouting_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <br clear="all" />
                <b>&nbsp;&nbsp;<asp:Literal ID="lblListofTicketCopy" runat="server" Text="List Of Ticket Copy"></asp:Literal></b>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuTicketNoAssign" runat="server"></eluc:TabStrip>
                </div>
                <br />
                <br />
               
                <div id="div2" style="position: relative; z-index: 1">
                    <asp:GridView ID="gvRoutingDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowFooter ="false"  EnableViewState="False" AllowSorting="true" 
                        onrowcommand="gvRoutingDetails_RowCommand" 
                        onrowdatabound="gvRoutingDetails_RowDataBound"                      
                        onrowupdating="gvRoutingDetails_RowUpdating" DataKeyNames="FLDROUTEID" 
                        onrowediting="gvRoutingDetails_RowEditing" OnRowCancelingEdit="gvRoutingDetails_RowCancelingEdit">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
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
                                <FooterTemplate >
                                    <asp:TextBox ID="txtOrginAdd" runat ="server" Width ="120px" CssClass="input_mandatory"></asp:TextBox>
                                </FooterTemplate>                              
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDestinationHeader" runat="server" ForeColor="White">Destination</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDestination" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATION") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate >
                                    <asp:TextBox ID="txtDestinationAdd" runat ="server" Width ="120px" CssClass="input_mandatory"></asp:TextBox>
                                </FooterTemplate>                              
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDepartureDateHeader" runat="server" ForeColor="White">Departure Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDepartureDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MMM/yyyy hh:mm tt}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date runat="server" ID="txtDepartureDateEdit" CssClass="readonlytextbox" Width ="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MMM/yyyy}") %>'>
                                    </eluc:Date>
                                    <asp:TextBox ID="txtDepartureTimeEdit" runat="server" CssClass="readonlytextbox" Width="50px"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:hh:mm tt}") %>' />
                                   
                                </EditItemTemplate>
                                <FooterTemplate>
                                <eluc:Date runat="server" ID="txtDepartureDateAdd" Width ="60px" CssClass="readonlytextbox" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MMM/yyyy}") %>'>
                                    </eluc:Date>
                                    <asp:TextBox ID="txtDepartureTimeAdd" runat="server" CssClass="readonlytextbox" Width="50px"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:hh:mm tt}") %>' />
                                            <ajaxToolkit:MaskedEditExtender ID="txtDepartureTimeMaskEdit" runat="server" TargetControlID="txtDepartureTimeAdd"
                                        Mask="99:99" MaskType="Time" AcceptAMPM="true" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblArrivalDateHeader" runat="server" ForeColor="White">Arrival Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblArrivalDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE","{0:dd/MMM/yyyy hh:mm tt}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Date runat="server" ID="txtArrivalDateEdit" CssClass="readonlytextbox" Width ="60px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MMM/yyyy}") %>'></eluc:Date>
                                    <asp:TextBox ID="txtArrivalTimeEdit" runat="server" CssClass="readonlytextbox" Width="50px"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE","{0:hh:mm tt}") %>' />                                     
                                </EditItemTemplate>
                                <FooterTemplate >
                                    <eluc:Date runat="server" ID="txtArrivalDateAdd" CssClass="readonlytextbox" Width ="60px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MMM/yyyy}") %>'></eluc:Date>
                                    <asp:TextBox ID="txtArrivalTimeAdd" runat="server" CssClass="readonlytextbox" Width="50px"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE","{0:hh:mm tt}") %>' /> 
                                          <ajaxToolkit:MaskedEditExtender ID="txtArrivalTimeAddMaskEdit" runat="server" TargetControlID="txtArrivalTimeAdd"
                                        Mask="99:99" MaskType="Time" AcceptAMPM="true" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAirlineCodeHeader" runat="server" ForeColor="White">Airline Code</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAirlineCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRLINECODE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtAirlineCode" CssClass="input_mandatory" Width ="60px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRLINECODE") %>'></asp:TextBox>                                  
                                </EditItemTemplate>
                                <FooterTemplate>
                                <asp:TextBox ID="txtAirlineCodeAdd" runat ="server" Width ="60px" CssClass="input_mandatory"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblClassHeader" runat="server" ForeColor="White">Class</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASS") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtClass" runat="server" Width ="60px" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASS") %>'></asp:TextBox>                                 
                                </EditItemTemplate>
                                <FooterTemplate>
                                <asp:TextBox ID="txtClassAdd" runat ="server" Width ="60px" CssClass="input_mandatory"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAmountHeader" runat="server" ForeColor="White">Amount</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Decimal ID="txtAmount" CssClass="input_mandatory" Width ="90px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' />                               
                                </EditItemTemplate>
                                <FooterTemplate>
                                 <eluc:Decimal ID="txtAmountAdd" CssClass="input_mandatory" Width ="90px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' />    
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTaxHeader" runat="server" ForeColor="White">Tax</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Decimal ID="txtTax" CssClass="input_mandatory" Width ="90px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>' />                              
                                </EditItemTemplate>
                                 <FooterTemplate>
                                 <eluc:Decimal ID="txtTaxAdd" CssClass="input_mandatory" Width ="90px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>' />    
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTaxHeader" runat="server" ForeColor="White">Ticket Number</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTicket" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtTicket" CssClass="input_mandatory" Width ="90px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>' ></asp:TextBox>                             
                                </EditItemTemplate>
                                 <FooterTemplate>
                                 <asp:TextBox ID="txtTicketAdd" CssClass="input_mandatory" Width ="90px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>' ></asp:TextBox> 
                                </FooterTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblInvoiceHeader" runat="server" ForeColor="White">Is Invoice</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIsInvoice" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDINVOICEDYN").ToString().ToString().Equals("1"))?"Y":"N" %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                 <asp:CheckBox ID="chkIsInvoiced" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDINVOICEDYN").ToString().Equals("1"))?true:false %>' />                                 
                                </EditItemTemplate>                                
                            </asp:TemplateField>
                              <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTaxHeader" runat="server" ForeColor="White">Ticket Number</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICECODE") %>'></asp:Label>
                                </ItemTemplate>                                                            
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
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>                                  
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdUpdate"
                                        ToolTip="Update"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterTemplate>
                                  <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                                    CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                                    ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>                            
                        </Columns>
                    </asp:GridView>
                </div>                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
