<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelAgentInvoiceGeneralInOffice.aspx.cs"
    Inherits="CrewTravelAgentInvoiceGeneralInOffice" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlCommonVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiOwnerBudgetCode" Src="~/UserControls/UserControlMultiColumnOwnerBudgetCodeT.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Travel Invoice</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewChangeTravel" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="InvoiceHistory" runat="server" OnTabStripCommand="InvoiceHistory_TablStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand"></eluc:TabStrip>
            <table id="tbl1" cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                       
                        <asp:Panel ID="pnlinvoice" runat="server" GroupingText="From Invoice">
                            <table id="Table1" cellpadding="1" cellspacing="1" width="100%">
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblInvoiceNo" runat="server" Text="Invoice No"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtInvoiceNo" runat="server" CssClass="readonlytextbox" Width="80%"
                                            ReadOnly="true">
                                        </telerik:RadTextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblRequestNo" runat="server" Text="Request No"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinvRequestno" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                            Width="80%">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPassengerName" runat="server" Text=" Passenger Name"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinvpassname" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                            Width="80%">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblTicketNo" runat="server" Text="Ticket No"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinvticketno" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                            Width="80%">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPNRNo" runat="server" Text="PNR No"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinvPNR" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                            Width="80%">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblAirlinecode" runat="server" Text="Airline Code"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinvairlinecode" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                            Width="80%">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblDepDate" runat="server" Text="Dep Date"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtinvdepdate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinvvessel" runat="server" CssClass="readonlytextbox" Width="80%"
                                            ReadOnly="true">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblBasic" runat="server" Text="Basic"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtinvbasic" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblTotalTax1" runat="server" Text="Total Tax(Tax + STX Collected)"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtinvtax" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblDiscount" runat="server" Text="Discount"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtinvdiscount" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblCancellation" runat="server" Text="Cancellation"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtinvcancell" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblTotal" runat="server" Text="Total"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtinvtotal" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblCreditNote" runat="server" Text="Credit Note"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtCreditNote" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblAgentRemarks" runat="server" Text="Agent Remarks"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtAgentRemarks" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                            Width="80%" Style="vertical-align: top; cursor: pointer">
                                        </telerik:RadTextBox>
                                        <eluc:ToolTip ID="ucToolTip" runat="server" Width="500" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblDateFrom" runat="server" Text="Dep Date Take From"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblDepDate" runat="server" RepeatDirection="Vertical" Width="80%">
                                            <asp:ListItem Text="Invoice" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Ticket Issued" Value="2" Selected="True"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="From Ticket Issued">
                            <table id="Table2" cellpadding="1" cellspacing="1" width="100%">
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lbl1" runat="server" Text="1"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lbl2" runat="server" Text="1"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblRequestNumber" runat="server" Text="Request No"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtRequestNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                            Width="60%">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPasengerName" runat="server" Text="Passenger Name"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtpassengername" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                            Width="60%">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblTicketNumber" runat="server" Text="Ticket No"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtticket" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                            Width="60%">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPNRNumber" runat="server" Text="PNR No"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtpnr" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                            Width="60%">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblAirlineCode1" runat="server" Text="Airline Code"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtairlinecode" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                            Width="60%">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblDepDate1" runat="server" Text="Dep Date"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtdepdate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="60%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblVessel1" runat="server" Text="Vessel"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtvessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                            Width="60%">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblBasic1" runat="server" Text="Basic"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtbasic" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="60%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblTotalTax" runat="server" Text="Total Tax(Tax + STX Collected)"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txttax" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="60%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblDiscount1" runat="server" Text="Discount"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtdiscount" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="60%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblCancelledReissued" runat="server" Text="Cancelled/Reissued"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtTktStatus" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="25%"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtCancelReason" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="34%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblTotal1" runat="server" Text="Total"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txttotal" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="60%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblNonVesselYN" runat="server" Text="Non-Vessel"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadCheckBox ID="chkNonVessel" runat="server" OnCheckedChanged="chkNonVessel_CheckedChanged" AutoPostBack="true" />
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblVesselChargeable" runat="server" Text="Vessel Chargeable"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Vessel ID="ddlvessel" runat="server" VesselsOnly="true" EntityType="VSL" ActiveVessels="true"
                                            AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ddlVessel_Changed" Width="60%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account "></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlAccountDetails" runat="server" Width="60%"
                                            OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID"
                                            Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                                            <Items>
                                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                            </Items>
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblBillToCompany" runat="server" Text="Bill to Company"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Company ID="ucBillToCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>' AutoPostBack="true" Width="60%"
                                            runat="server" AppendDataBoundItems="true" OnTextChangedEvent="ucBillToCompany_TextChangedEvent" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="Literal1" runat="server" Text="Paying Company"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Company ID="ucPayingCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>' AutoPostBack="true"
                                            runat="server" Width="60%" AppendDataBoundItems="true" Readonly="true" Enabled="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account Code"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlAccountCode" runat="server" Width="60%" AutoPostBack="true" AppendDataBoundItems="true"
                                            OnTextChanged="ddlAccountCode_TextChanged" DataTextField="FLDDESCRIPTION" DataValueField="FLDACCOUNTID"
                                            Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblSubAccount" runat="server" Text="Sub Account"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlSubAccount" runat="server" Width="60%"
                                            DataTextField="FLDDESCRIPTION" DataValueField="FLDSUBACCOUNTMAPID"
                                            Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                                        </telerik:RadComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:BudgetCode ID="ucBudgetCode" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                            OnTextChangedEvent="ucBudgetCode_Changed" AutoPostBack="true" Width="60%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblOwnerBudget" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:MultiOwnerBudgetCode ID="ucOwnerBudgetCode" runat="server" Enabled="true" Width="60%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lbl5" runat="server" Text="1"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lbl6" runat="server" Text="1"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>&nbsp
                                    <telerik:RadLabel ID="lblSector1" runat="server" Text="Sector1"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtSector1" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp
                                    <telerik:RadLabel ID="lblSector2" runat="server" Text="Sector2"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtSector2" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp<telerik:RadLabel ID="lblSector3" runat="server" Text="Sector3"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtSector3" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp
                                    <telerik:RadLabel ID="lblSector4" runat="server" Text="Sector4"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtSector4" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table id="tbladjustment" cellpadding="1" cellspacing="1" width="100%">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblAdjustmentAmount" runat="server" Text="Adjustment Amount"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtadjustamount" runat="server" Width="80%" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblPayableAmount" runat="server" Text="Payable Amount"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtpayableamount" runat="server" Width="40%" />
                                    <eluc:Number ID="txtDifferencePercentage" runat="server" CssClass="readonlytextbox"
                                        Width="40%" />
                                    %
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtremarks" runat="server" CssClass="input_mandatory" Width="80%"
                                        Height="50px" TextMode="MultiLine">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblstatus" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                        OnSelectedIndexChanged="rblstatus_OnSelectedIndexChanged">
                                        <asp:ListItem Text="Can be Paid" Value="1168"></asp:ListItem>
                                        <asp:ListItem Text="On Hold" Value="1169"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblStaffName" runat="server" Text="Staff Name"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtUser" CssClass="readonlytextbox" ReadOnly="true" runat="server"
                                        Width="80%">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvList" runat="server" Height="37%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvList_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvList_ItemDataBound1"
                OnItemCommand="gvList_ItemCommand" ShowFooter="false" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Reference No.">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReferenceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Origin">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrigin" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGIN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDestination" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ticket No">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicket" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ticket  Cancelled">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicketCancelled" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELTICKETYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Status">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
        <eluc:Confirm ID="ucConfirmDuplicateSector" runat="server" OnConfirmMesage="btnConfirmDuplicateSector_Click"
            OKText="Yes" CancelText="No" Visible="false" />
    </form>
</body>
</html>
