<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsPaymentVoucherVesselVisitTravelClaim.aspx.cs" Inherits="AccountsPaymentVoucherVesselVisitTravelClaim" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="~/UserControls/UserControlDate.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payment Voucher</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPVGenerate" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPVGenerate">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Payment Voucher"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblVoucherNo" runat="server" Text="Voucher Number"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVoucherNo" runat="server" CssClass="readonlytextbox" Width="200px">
                                </asp:TextBox>
                                <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                    ID="cmdApprove" OnClick="cmdApprove_OnClientClick" ToolTip="Approve"></asp:ImageButton>
                            </td>
                            <td>
                                <asp:Literal ID="lblBankName" runat="server" Text="Bank Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankName" runat="server" CssClass="readonlytextbox" Width="200px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblVoucherDate" runat="server" Text="Voucher Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:UserControlDate ID="ucVoucherDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"/>
                            </td>
                            <td>
                                <asp:Literal ID="lblBeneficiaryName" runat="server" Text="Beneficiary Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBeneficiaryName" runat="server" CssClass="readonlytextbox" Width="200px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblEmployeeName" runat="server" Text="Payee"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmployeeCode" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                        MaxLength="50" Width="60px"></asp:TextBox>
                                <asp:TextBox ID="txtEmployeeName" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                        MaxLength="200" Width="150px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblBankAccountNo" runat="server" Text="Bank Account Number"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankAccountNo" runat="server" CssClass="readonlytextbox" Width="200px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCurrency" runat="server" Text="Payable Currency"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCurrency" runat="server" CssClass="readonlytextbox" Width="200px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblPaymentAmount" runat="server" Text="Payable Amount"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtpaymentAmount" runat="server" CssClass="readonlytextbox" Width="200px">
                                </asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskNumber" runat="server" TargetControlID="txtpaymentAmount"
                                    Mask="999,999,999.99" MaskType="Number" InputDirection="RightToLeft">
                                </ajaxToolkit:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblPurpose" runat="server" Text="Purpose of Visit"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPurpose" runat="server" CssClass="readonlytextbox" Width="200px">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblDate" runat="server" Text="Date of Travel"></asp:Literal>
                            </td>
                            <td>
                                <eluc:UserControlDate ID="ucFromDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"/> - 
                                <eluc:UserControlDate ID="ucToDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtStatus" runat="server" CssClass="readonlytextbox" Width="200px">
                                </asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuTravelClaim" runat="server" OnTabStripCommand="MenuTravelClaim_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="div2" style="position: relative; z-index: 1; width: 100%;">
                    <asp:GridView ID="gvLineItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvLineItem_ItemDataBound" 
                        ShowHeader="true" ShowFooter="true"
                        EnableViewState="false" AllowSorting="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblVesselVisitIdHdr" runat="server" Text="Vessel Visit Id"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselVisitId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNUMBER") %>'></asp:Label>                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblVoucherNumberHdr" runat="server" Text="Voucher Number"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblVoucherDateHdr" runat="server" Text="Voucher Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVoucherDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblVesselNameHdr" runat="server" Text="Vessel Name"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPurposeHdr" runat="server" Text="Purpose"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPurpose" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCurrencyHdr" runat="server" Text="Payable Currency"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>                                    
                                    <b>
                                        Total
                                    </b>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblReimAmountHdr" runat="server" Text="Reimbursement Amount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReimAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:###,###,###,##0.00}") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>                                    
                                    <b>
                                        <%=strAmountTotal%>
                                    </b>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <table id="Table2" width="100%" style="color: Blue">
                    <tr>
                        <td>
                            &nbsp;Less: Cash Paid by Vessel/ Travel Advance 
                        </td>
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuTravelAdvance" runat="server" OnTabStripCommand="MenuTravelAdvance_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index:0; width: 100%;">
                    <asp:GridView ID="gvTravelAdvance" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvTravelAdvance_ItemDataBound" ShowFooter="true"
                        ShowHeader="true" EnableViewState="false" AllowSorting="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblTravelAdvanceNoHdr" runat="server" Text="Travel Advance Id"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTravelAdvanceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELADVANCEID") %>'></asp:Label>
                                    <asp:Label ID="lnkTravelAdvanceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELADVANCENUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblVesselHdr" runat="server" Text="Vessel"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblDateHdr" runat="server" Text="Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAdvanceAmountHdr" runat="server" Text="Advance Amount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAdvanceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTAMOUNT","{0:###,###,###,##0.00}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAmountAllocatedHdr" runat="server" Text="Amount Allocated"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountAllocated" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLAIMAMOUNT","{0:###,###,###,##0.00}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAdvanceRemarksHdr" runat="server" Text="Advance Remarks"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAdvanceRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumberadv" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPagesadv" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecordsadv" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPreviousadv" runat="server" OnCommand="PagerButtonClickadv"
                                    CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNextadv" OnCommand="PagerButtonClickadv" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopageadv" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGoadv" runat="server" Text="Go" OnClick="cmdGoAdv_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>                
                <table id="Table1" width="100%" style="color: Blue">
                    <tr>
                        <td>
                            &nbsp;List of other Travel Advance
                        </td>
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuTravelAdvanceOther" runat="server" OnTabStripCommand="MenuTravelAdvanceOther_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid1" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvTravelAdvanceOther" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvTravelAdvanceOther_ItemDataBound"
                        ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" /><Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblTravelAdvanceNoHdr" runat="server" Text="Travel Advance Id"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTravelAdvanceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELADVANCEID") %>'></asp:Label>
                                    <asp:Label ID="lnkTravelAdvanceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELADVANCENUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblVesselHdr" runat="server" Text="Vessel"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblDateHdr" runat="server" Text="Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREQUESTEDDATE")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAdvanceAmountHdr" runat="server" Text="Advance Amount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAdvanceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTAMOUNT","{0:###,###,###,##0.00}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAmountAllocatedHdr" runat="server" Text="Amount Allocated"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountAllocated" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELCLAIMAMOUNT","{0:###,###,###,##0.00}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblBalanceAmountHdr" runat="server" Text="Balance Amount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBalanceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALANCEAMOUNT","{0:###,###,###,##0.00}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAdvanceRemarksHdr" runat="server" Text="Advance Remarks"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAdvanceRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumberOtheradv" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPagesOtheradv" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecordsOtheradv" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPreviousOtheradv" runat="server" OnCommand="PagerButtonClickOtheradv"
                                    CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNextOtheradv" OnCommand="PagerButtonClickOtheradv" runat="server"
                                    CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopageOtheradv" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGoOtheradv" runat="server" Text="Go" OnClick="cmdGoOtherAdv_Click"
                                    CssClass="input" Width="40px"></asp:Button>
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
