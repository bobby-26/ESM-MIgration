    <%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCostEvaluationQuoteLineItem.aspx.cs"
    Inherits="CrewCostEvaluationQuoteLineItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="../UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Quote Items</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCostQuoteLineItems" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlLineItemEntry">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false">
                </eluc:Error>
                <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
                <b>
                    <asp:Literal ID="lblAgentQuotes" runat="server" Text="Agent Quotes"></asp:Literal></b>
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <div class="navSelect* " style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuQuote" runat="server" OnTabStripCommand="MenuQuote_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="div3" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvQuote" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" EnableViewState="False" AllowSorting="true" OnRowCommand="gvQuote_RowCommand"
                        OnRowDataBound="gvQuote_RowDataBound">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <ItemTemplate>
                                    <img id="imgflag" runat="server" src="<%$ PhoenixTheme:images/14.png %>" visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuoteId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblPortAgentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTAGENTID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblAgentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblAgentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTNAME") %>'
                                        Visible="false"></asp:Label>
                                         <asp:Label ID="lblEvaluationPortId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVALUATIONPORTID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblApprovedYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDYN") %>'
                                        Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lblPortName" CommandName="Select" CommandArgument='<%# Container.DataItemIndex %>'
                                        runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblETA" runat="server" Text="ETA"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblETA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETA") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblETD" runat="server" Text="ETD"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblETD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETD") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblQuotationNo" runat="server" Text="Quotation No."></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQtnNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEREFNO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCurrencyHeader" runat="server" ForeColor="White">Currency</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblTotalAmount" runat="server" Text="Total Amount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblTotalAmountinUSD" runat="server" Text="Total Amount in USD"></asp:Literal><br />
                                    <asp:Literal ID="lblInclFareTax" runat="server" Text="(Incl. Fare + Tax)"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalAmountusd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALUSDAMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <%-- <asp:ImageButton runat="server" AlternateText="Finalize" ImageUrl="<%$ PhoenixTheme:images/final-quotation.png %>"
                                        CommandName="FINALIZE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdFinalize"
                                        ToolTip="Finalize"></asp:ImageButton>--%>
                                    <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/approve.png%>"
                                        CommandName="APPROVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdApprove"
                                        ToolTip="Approve" Visible="false"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Revoke approval" ImageUrl="<%$ PhoenixTheme:images/cancel.png %>"
                                        CommandName="DEAPPROVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDeApprove"
                                        ToolTip="Revoke approval" Visible="false"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Communication" ImageUrl="<%$ PhoenixTheme:images/communication.png %>"
                                        CommandName="COMMUNICATION" CommandArgument='<%# Container.DataItemIndex %>'
                                        ID="cmdCommunication" ToolTip="Chat"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br /><b><asp:Literal ID="lblSectionTotal" runat="server" Text="Section Total"></asp:Literal></b>                
                <div id="divGrid" style="position: relative; z-index: 0; width: 80%;">
                    <asp:GridView ID="gvSectionType" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowCreated="gvSectionType_RowCreated" Width="100%" CellPadding="3" ShowHeader="true"
                        EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle BackColor="#5588bb" ForeColor="#ffffff" Font-Bold="true" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblAgentName" runat="server" Text="Agent Name"> </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAgentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
                <b>&nbsp;<asp:Literal ID="lblQuoteDetails" runat="server" Text="Quote Details"></asp:Literal></b>
                <div class="navSelect* " style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuQuoteLI" runat="server" OnTabStripCommand="MenuQuoteLI_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="div2" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvLineItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" EnableViewState="False" AllowSorting="true" OnRowCommand="gvLineItem_RowCommand"
                        OnRowDataBound="gvLineItem_RowDataBound" ShowFooter="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSectionType" runat="server" Text="Section Type"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLineItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTELINEITEMID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblSectionTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONTYPENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSection" runat="server" Text="Section"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONNAME") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Literal ID="lblTotalAmount" runat="server" Text="Total Amount"></asp:Literal>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAmount" runat="server" Text="Amount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Save" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
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
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
