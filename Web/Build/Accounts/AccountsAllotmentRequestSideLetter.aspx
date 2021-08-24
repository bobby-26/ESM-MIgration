<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRequestSideLetter.aspx.cs"
    Inherits="AccountsAllotmentRequestSideLetter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ucStatus  " Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmSideLetter" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlLineItemEntry">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader">
                    <eluc:Title runat="server" ID="Title1" Text="Side Letter" ShowMenu="false"></eluc:Title>
                </div>
                <table cellpadding="1" cellspacing="1" width="100%" style="border-style: none; color: blue;"
                    runat="server" id="tblNote">
                    <tr>
                        <td>
                            <asp:Literal ID="lblPleaseaddatleast1allotmentrequestforthebelowsidelettersifthereisanybalanceduetothecrew" runat="server" Text="* Please add at least 1 allotment request for the below side letters if there is
                              any balance due to the crew."></asp:Literal>
                        </td>
                    </tr>
                </table>
                <table cellpadding="2" cellspacing="2">
                    <tr>
                        <td>
                            <asp:Literal ID="lblWagesCalculationDate" runat="server" Text="Wages Calculation Date :"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtWageCalculationDate" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvSideLetter" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" EnableViewState="False" AllowSorting="true" OnRowDataBound="gvSideLetter_RowDataBound"
                    ShowFooter="true">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblComponentAgreed" runat="server" Text="Component Agreed"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblComponentId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></asp:Label>
                                <asp:LinkButton ID="lnkComponenetName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblContractAmountUSD" runat="server" Text="Contract Amount (USD)"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblContractAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Literal ID="lblTotalAmount" runat="server" Text="Total Amount"></asp:Literal>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblPayableTillDate" runat="server" Text="Payable Till Date"></asp:Literal>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <asp:Label ID="lblPayableTillDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblTotalPaid" runat="server" Text="Total Paid"></asp:Literal>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <asp:Label ID="lblPaid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAIDAMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblBalanceUSD" runat="server" Text="Balance (USD)"></asp:Literal>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <asp:Label ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALANCEAMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <asp:GridView ID="gvSLRequest" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" EnableViewState="False" AllowSorting="true" OnRowDataBound="gvSLRequest_RowDataBound"
                    ShowFooter="true" OnRowCommand="gvSLRequest_RowCommand">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblRequestDate" runat="server" Text="Request Date"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRequestDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTDATE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblRequestNumber" runat="server" Text="Request Number"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblRequestNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTNUMBER") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblRequestStatus" runat="server" Text="Request Status"></asp:Literal>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <asp:Label ID="lblRequestStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTSTATUSNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblComponent" runat="server" Text="Component"></asp:Literal>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <asp:Label ID="lblComponenet" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddlComponent" AutoPostBack="true" runat="server" CssClass="dropdown_mandatory"
                                    DataTextField="FLDCOMPONENTNAME" DataValueField="FLDCOMPONENTID">
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblBankAccount" runat="server" Text="Bank Account"></asp:Literal>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <asp:Label ID="lblBankAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKACCOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="ddlBankId" runat="server" CssClass="dropdown_mandatory" DataTextField="FLDACCOUNT"
                                    DataValueField="FLDBANKACCOUNTID">
                                </asp:DropDownList>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAmountUSD" runat="server" Text="Amount (USD)"></asp:Literal>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:MaskNumber ID="txtActualAmountAdd" runat="server" DecimalPlace="2" CssClass="input_mandatory"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                            </HeaderTemplate>
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <eluc:ucStatus id="ucStatus" runat="server" visible="false">
            </eluc:ucStatus>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
