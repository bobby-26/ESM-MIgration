<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceRequestsPayment.aspx.cs"
    Inherits="Crew_CrewLicenceRequestsPayment" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Licence Requests Payment</title>
       <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlOrderForm">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader">
                    <div class="divFloatLeft">
                        <eluc:Title runat="server" ID="frmTitle" Text="Licence Payment Requests"></eluc:Title>
                    </div>
                    <div class="divFloat">
                        <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="subHeader">
                    <div class="divFloat" style="clear: right">
                        <eluc:TabStrip ID="MenuOrderFormSub" runat="server" OnTabStripCommand="MenuOrderFormSub_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                </div>
                <div id="find">
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="lblConsulate" runat="server" Text="Consulate"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Address ID="ucAddress" runat="server" AppendDataBoundItems="true" AddressType="334"
                                    CssClass="input" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblReceivingInvoicesFromConsulate" runat="server" Text="Receiving Invoices From Consulate"></asp:Literal>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkPayment" runat="server"/>
                            </td>
                            <td>
                                <asp:Literal ID="lblShowAll" runat="server" Text="Show All"></asp:Literal>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkShowAll" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvAdvancePayment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvAdvancePayment_RowCommand" OnRowDataBound="gvAdvancePayment_ItemDataBound"
                        AllowSorting="true" EnableViewState="true" OnRowCreated="gvAdvancePayment_RowCreated" OnSelectedIndexChanged="gvAdvancePayment_OnSelectedIndexChanged"
                        OnSorting="gvAdvancePayment_Sorting"
                        DataKeyNames="FLDADVANCEPAYMENTID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelection" runat="server" OnCheckedChanged="chkSelection_OnCheckedChanged" AutoPostBack="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblVoucherNumber" runat="server" Text="Voucher Number"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVoucherNumer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblConsulate" runat="server" Text="Consulate"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblConsulateName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblBilltoCompany" runat="server" Text="Bill to Company"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBillToCompanyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRequestNumber" runat="server" Text="Request Number"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAdvanceId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDADVANCEPAYMENTID")%>'></asp:Label>
                                    <asp:Label ID="lblProcessId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDORDERID")%>'></asp:Label>
                                    <asp:Label ID="lblCOnsulate" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSUPPLIERCODE")%>'></asp:Label>
                                    <asp:Label ID="lblBillToCompany" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCOMPANYID")%>'></asp:Label>
                                    <asp:Label ID="lblBankid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDBANKID")%>'></asp:Label>
                                    <asp:Label ID="lblCurrency" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCURRENCY")%>'></asp:Label>
                                    <asp:Label ID="lblStatus" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPAYMENTSTATUS")%>'></asp:Label>
                                    <asp:Label ID="lblReceivingInvoice" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRECEIVINGINVOICE")%>'></asp:Label>
                                    <asp:LinkButton ID="lnkRequestNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEDOCUMENT") %>'
                                        CommandName="Select" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRankCrewName" runat="server" Text="Rank/Crew Name"></asp:Literal></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRankCrewName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField>
                        <ItemStyle Wrap="false" HorizontalAlign="Left" />
                        <HeaderTemplate>
                            Joined Vessel
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblJoinedVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOINEDVESSEL") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblLicenceApplied" runat="server" Text="Licence Applied"></asp:Literal></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLicenceApplied" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblBank" runat="server" Text="Bank"></asp:Literal></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKNAME") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrencyName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAmount" runat="server" Text="Amount"></asp:Literal></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblReimbursabletoCrew" runat="server" Text="Reimbursable to Crew"></asp:Literal></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReimbursable" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREIMBURSABLE") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal></HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatusName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="false" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:Label></ItemTemplate>
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
                            <td width="20px">
                                &nbsp;
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
