<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSupplierBankInformation.aspx.cs" Inherits="AccountsSupplierBankInformation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagName="Currency" TagPrefix="eluc" Src="~/UserControls/UserControlCurrency.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bank Information</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div runat="server" id="DivHeader">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmBankInformation" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManagerbank" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <%-- <asp:UpdatePanel runat="server" ID="pnlBankInformation">
        <ContentTemplate>--%>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader" style="position: relative">
            <div id="div1" style="vertical-align: top">
                <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Address" Width="360px"></asp:Label>
            </div>
        </div>
        <%--<div class="navSelectHeader" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuBankInformation" runat="server" OnTabStripCommand="AddressMain_TabStripCommand">
            </eluc:TabStrip>
        </div>--%>
       <%-- <div class="navSelectHeader" style="top: 28px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuBankMain" runat="server" OnTabStripCommand="BankMain_TabStripCommand">
            </eluc:TabStrip>
        </div>--%>
       <%-- <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuRegistersBankInformation" runat="server" OnTabStripCommand="RegistersBankInformation_TabStripCommand">
            </eluc:TabStrip>
        </div>--%>
        <div id="divGrid" style="position: relative; z-index: 0">
            <asp:GridView ID="gvBankInformation" runat="server" AutoGenerateColumns="False" CellPadding="3"
                Font-Size="11px" OnRowCommand="gvBankInformation_RowCommand" OnRowDataBound="gvBankInformation_RowDataBound"
                ShowHeader="true" Width="100%"
                ShowFooter="true" EnableViewState="false">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <HeaderTemplate>
                            <asp:Literal ID="lblBankInformation" runat="server" Text="Bank Information"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <font size="2px"><b><u><asp:Literal ID="lblBank" runat="server" Text="Bank"></asp:Literal></u></b></font>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td width="10%">
                                        <b><asp:Literal ID="lblBankBankName" runat="server" Text="Bank Name:"></asp:Literal></b>
                                    </td>
                                    <td width="19%">
                                        <asp:Label ID="lblBankid" Visible="false" Text='<%#Bind("FLDBANKID") %>' runat="server"></asp:Label>
                                        <asp:Label ID="lblBankName" Text='<%#Bind("FLDBANKNAME") %>' runat="server"></asp:Label>
                                    </td>
                                    <td width="10%">
                                        <b><asp:Literal ID="lblBankCode" runat="server" Text="Bank Code:"></asp:Literal></b>
                                    </td>
                                    <td width="20%">
                                        <asp:Label ID="lblBcode" runat="server" Text='<%#Bind("FLDBANKCODE") %>'></asp:Label>
                                    </td>
                                    <td width="10%">
                                        <b><asp:Literal ID="lblBranchCode" runat="server" Text="Branch Code:"></asp:Literal></b>
                                    </td>
                                    <td width="21%">
                                        <asp:Label ID="lblBrcode" runat="server" Text='<%#Bind("FLDBRANCHCODE") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b><asp:Literal ID="lblBankSwiftCode" runat="server" Text="Swift Code:"></asp:Literal></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSwiftcode" runat="server" Text='<%#Bind("FLDSWIFTCODE") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b><asp:Literal ID="lblBankIBANNumber" runat="server" Text="IBAN Number:"></asp:Literal></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblIBANNumber" runat="server" Text='<%#Bind("FLDIBANNUMBER") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b><asp:Literal ID="lblBankAccountNumber" runat="server" Text="Account Number:"></asp:Literal></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblAccountNumber" runat="server" Text='<%#Bind("FLDACCOUNTNUMBER") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b><asp:Literal ID="lblBankCurrency" runat="server" Text="Currency:"></asp:Literal></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCurrency" runat="server" Text='<%#Bind("FLDCURRENCYNAME") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b><asp:Literal ID="lblBankBeneficiaryName" runat="server" Text="Beneficiary Name:"></asp:Literal> </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBeneficiaryName" runat="server" Text='<%#Bind("FLDBENEFICIARYNAME") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b> <asp:Literal ID="lblContactPersonName" runat="server" Text="Contact Person Name:"></asp:Literal></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblContactName" runat="server" Text='<%#Bind("FLDCONTACTNAME") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b><asp:Literal ID="lblEMail" runat="server" Text="E-Mail:"></asp:Literal> </b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEmailid" runat="server" Text='<%#Bind("FLDEMAILID") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b> <asp:Literal ID="lblBankPhoneNumber" runat="server" Text="Phone Number:"></asp:Literal></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPhoneNumber" runat="server" Text='<%#Bind("FLDPHONENUMBER") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <font size="2px"><b><u><asp:Literal ID="lblIntermediateBank" runat="server" Text="Intermediate Bank"></asp:Literal></u></b></font>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b><asp:Literal ID="lblIntermediateBankName" runat="server" Text="Bank Name:"></asp:Literal></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbliBankid" Visible="false" Text='<%#Bind("FLDIBANKID") %>' runat="server"></asp:Label>
                                        <asp:Label ID="txtibankname" runat="server" Text='<%#Bind("FLDIBANKNAME") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b><asp:Literal ID="lblIntermediateBankCode" runat="server" Text="Bank Code:"></asp:Literal></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblibankcode" runat="server" Text='<%#Bind("FLDIBANKCODE") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b><asp:Literal ID="lblIntermediateBranchCode" runat="server" Text="Branch Code:"></asp:Literal></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblibranchcode" runat="server" Text='<%#Bind("FLDIBRANCHCODE") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b><asp:Literal ID="lblIntermediateSwiftCode" runat="server" Text="Swift Code:"></asp:Literal></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbliswiftcode" runat="server" Text='<%#Bind("FLDISWIFTCODE") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b><asp:Literal ID="lblIntermediateIBANNumber" runat="server" Text="IBAN Number:"></asp:Literal></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbliinanumber" runat="server" Text='<%#Bind("FLDIIBANNUMBER") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <b><asp:Literal ID="lblIntermediateAccountNumber" runat="server" Text="Account Number:"></asp:Literal></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbliAccountNumber" runat="server" Text='<%#Bind("FLDIACCOUNTNUMBER") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b><asp:Literal ID="lblIntermediateCurrency" runat="server" Text="Currency:"></asp:Literal></b>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbliCurrency" runat="server" Text='<%#Bind("FLDICURRENCYNAME") %>'></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>                                              
                    </asp:TemplateField>                    
                </Columns>
            </asp:GridView>
        </div>
        <div id="divPage" style="position: relative;">
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
    </div>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    </form>
</body>
</html>

