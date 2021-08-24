<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRemittanceRejectionPMV.aspx.cs" Inherits="AccountsAllotmentRemittanceRejectionPMV" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.VesselAccounts" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BankAccount" Src="~/UserControls/UserControlEmployeeBankAccount.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PMV Details</title>
    <telerik:radcodeblock id="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

  </telerik:radcodeblock>
</head>
<body>
    <form id="AllotmentRejectedPMV" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlRejectedPMV">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <%--<asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />--%>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading" style="vertical-align: top">
                            <eluc:Title runat="server" ID="ucTitle" Text="Rejection Details" ShowMenu="false" />
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuRejectedPMV" runat="server" TabStrip="true"></eluc:TabStrip>
                        <eluc:Status runat="server" ID="ucStatus" />
                    </div>
                    <table cellpadding="1" cellspacing="1" style="width: 100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblRemittanceNumber" runat="server" Text="Remittance No."></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemittanceNumber" runat="server" CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblFileNo" runat="server" Text="File No."></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFileNo" runat="server" CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblEmployeeName" runat="server" Text="Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmployeeName" runat="server" CssClass="readonlytextbox"></asp:TextBox>
                                <asp:TextBox ID="txtEmployeeId" runat="server" Width="1" CssClass="input" Visible="false" Style="visibility: hidden"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRank" runat="server" CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <b>
                        <hr />
                    </b>
                    <br />
                    <b>
                        <asp:Literal ID="lblPaymentVoucherDetails" runat="server" Text="Payment Voucher Details"></asp:Literal>

                    </b>
                    <table cellpadding="0" cellspacing="1" style="width: 100%">
                        <tr>
                            <td style="vertical-align: top">
                                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                                    <asp:GridView ID="gvVoucherDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="3" OnRowCommand="gvVoucherDetails_RowCommand" OnRowDataBound="gvVoucherDetails_ItemDataBound"
                                        AllowSorting="true" EnableViewState="false" DataKeyNames="FLDPAYMENTVOUCHERREJECTIONID" OnRowCancelingEdit="gvVoucherDetails_RowCancelingEdit"
                                        OnRowEditing="gvVoucherDetails_RowEditing" OnRowCreated="gvVoucherDetails_RowCreated">
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" HorizontalAlign="Center" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                                        <RowStyle Height="10px" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="12%">
                                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblActiveYNHeader" runat="server"> Payment Voucher No.</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERNUMBER") %>'></asp:Label>
                                                    <asp:TextBox ID="txtVoucherId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERID") %>'
                                                        MaxLength="10" Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="txtRemittanceId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREMITTANCEID") %>' Visible="false"></asp:TextBox>
                                                    <asp:TextBox ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' Visible="false"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--                                            <asp:TemplateField ItemStyle-Width="12%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Literal ID="lblNewVoucherHeader" runat="server" Text="New"></asp:Literal>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNewVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWPAYMENTVOUCHERNUMBER") %>'></asp:Label>
                                                    <asp:Label ID="lblNewVoucherId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWPAYMENTVOUCHERID") %>'
                                                        MaxLength="10" Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Voucher Amount (USD)" ItemStyle-Width="8%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Literal ID="lblAmount" runat="server" Text="Amount(USD)"></asp:Literal>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Previous" ItemStyle-Width="29%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Literal ID="lblHeaderoldBankAccount" runat="server" Text="Previous"></asp:Literal>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOldBankAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOLDBANKACCOUNT") %>'></asp:Label>
                                                    <asp:Label ID="lblOldBankAccountId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOLDCREWBANKID") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="New" ItemStyle-Width="29%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Literal ID="lblHeaderNewBankAccount" runat="server" Text="New"></asp:Literal>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNewBankAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWBANKACCOUNT") %>'></asp:Label>
                                                    <asp:Label ID="lblNewBankAccountId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWCREWBANKID")%>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <eluc:BankAccount ID="ddlBankAccountEdit" runat="server" CssClass="dropdown_mandatory"
                                                        BankAccountList='<%#PhoenixVesselAccountsEmployeeBankAccount.ListEmployeeBankAccount(int.Parse(DataBinder.Eval(Container,"DataItem.FLDVESSELID").ToString()) ,General.GetNullableInteger(DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID").ToString()))%>'
                                                        AppendDataBoundItems="true" Width="90px"
                                                        EmployeeId='<%#DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                                        SelectedBankAccount='<%#DataBinder.Eval(Container,"DataItem.FLDNEWCREWBANKID")%>' />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="10%">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblActionHeader" runat="server"> Action </asp:Label>
                                                </HeaderTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                                        ToolTip="Remittance Rejection"></asp:ImageButton>
                                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                    <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                                        ID="cmdApprove" ToolTip="Approve" CommandName="APPROVE" CommandArgument="<%# Container.DataItemIndex %>"></asp:ImageButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl='<%$ PhoenixTheme:images/save.png%>'
                                                        CommandName="SAVE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                                        ToolTip="Save"></asp:ImageButton>
                                                    <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl='<%$ PhoenixTheme:images/te_del.png%>'
                                                        CommandName="CANCEL" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                                        ToolTip="Cancel"></asp:ImageButton>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <br />
                                <br />
                                <b>
                                    <asp:Literal ID="lblAdvancePaymentVoucherDetails" runat="server" Text="Allotment Request Details"></asp:Literal>
                                </b>
                                <table cellpadding="0" cellspacing="1" style="width: 100%">
                                    <tr>
                                        <td style="vertical-align: top">
                                            <div id="div2" style="position: relative; z-index: 0; width: 100%;">
                                                <asp:GridView ID="gvAdvanceVoucherDetails" runat="server" AutoGenerateColumns="False"
                                                    Font-Size="11px" Width="100%" CellPadding="3" AllowSorting="true" EnableViewState="false"
                                                    DataKeyNames="FLDPAYMENTVOUCHERID">
                                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" HorizontalAlign="Center" />
                                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                                                    <RowStyle Height="10px" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblActiveYNHeader" runat="server"> Request No.</asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAllotmentRequestNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTREQUESTNO") %>'></asp:Label>
                                                                <asp:TextBox ID="txtAllotmentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTID") %>' Visible="false"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Vessel">
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderTemplate>
                                                                <asp:Literal ID="lblVesselNameHeader" runat="server" Text="Vessel"></asp:Literal>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                                                <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Allotment Type">
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderTemplate>
                                                                <asp:Literal ID="lblAllotmentTypeHeader" runat="server" Text="AllotmentType"></asp:Literal>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAllotmentType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOTMENTTYPENAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Month">
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderTemplate>
                                                                <asp:Literal ID="lblMonthHeader" runat="server" Text="Month"></asp:Literal>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTH") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderTemplate>
                                                                <asp:Literal ID="lblCurrencyALHeader" runat="server" Text="Currency"></asp:Literal>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Literal ID="lblCurrencyAL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Voucher Amount">
                                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                            <HeaderTemplate>
                                                                <asp:Literal ID="lblAmount" runat="server" Text="Amount"></asp:Literal>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAdvanceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCEAMOUNT","{0:n2}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblActiveYNHeader" runat="server">Payment Voucher No.</asp:Label>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></asp:Label>
                                                                <asp:TextBox ID="txtVoucherAId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERID") %>' Visible="false"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
