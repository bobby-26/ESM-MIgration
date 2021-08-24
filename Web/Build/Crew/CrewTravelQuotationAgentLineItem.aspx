<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelQuotationAgentLineItem.aspx.cs"
    Inherits="CrewTravelQuotationAgentLineItem" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tax" Src="~/UserControls/UserControlTaxMaster.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AgentItems</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript">
            function btnconfirm(args) {
                if (args) {
                    __doPostBack("<%=btnconfirm.UniqueID %>", "");
                }
            }
        </script>
        <style>
            .tooltipHeight {
                height: 130px;
                padding: 0;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewTravelQuotationAGentLineItems" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="88%">
            <asp:Button ID="btnconfirm" runat="server" Text="btnconfirm" OnClick="btn_approve" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <telerik:RadLabel ID="lblTitle" runat="server" Visible="false"></telerik:RadLabel>
            <eluc:TabStrip ID="MenuQuotationLineItem" runat="server" TabStrip="true" OnTabStripCommand="MenuQuotationLineItem_TabStripCommand"></eluc:TabStrip>

            <table style="color: Blue">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblToviewtheGuidelinesplacethemouseonthe" runat="server" Text="To view the Guidelines, place the mouse on the"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadToolTip RenderMode="Lightweight" ID="btnTooltipHelp" runat="server" CssClass="tooltipHeight" TargetControlID="btnHelp" HideDelay="10000">
                        </telerik:RadToolTip>
                        <asp:LinkButton runat="server" AlternateText="Guide" ID="btnHelp" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-info-circle"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
            <b>&nbsp;<telerik:RadLabel ID="lblTravelQuotes" runat="server" Text="Travel Quotes"></telerik:RadLabel>
            </b>
            <eluc:TabStrip ID="MenuQuotationList" runat="server" OnTabStripCommand="MenuQuotationList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvQuotation" runat="server" AllowCustomPaging="true" AllowSorting="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvQuotation_ItemCommand" OnNeedDataSource="gvQuotation_NeedDataSource" AllowPaging="true"
                OnItemDataBound="gvQuotation_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="45px">
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemTemplate>
                                <img id="imgflag" runat="server" src="<%$ PhoenixTheme:images/14.png %>" visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" HeaderText="Quotation No.">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuotationID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAgentID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsApproved" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.APPOVEDSTATUS") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsFinalized" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINALIZEDYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lbltravelfinalizeyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELFINALIZEYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lblQuotationNo" CommandName="Select" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONREFNO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="102px" HeaderText="Quotation Date">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuotationDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRECEIVEDDATE", "{0:dd/MMM/yyyy}")) +" "+ DataBinder.Eval(Container, "DataItem.FLDRECEIVEDDATE", "{0:t}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="60px" HeaderText="Currency">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="62px" HeaderText="Fare">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="62px" HeaderText="Tax" AllowSorting="true" SortExpression="FLDTAX" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTTAX") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="85px" HeaderText="Total Amount">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTTOTALAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="130px" HeaderText=" Total Amount(USD)(Incl. Fare + Tax)">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalAmountusd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" HeaderText="Quoted Status">
                            <ItemStyle HorizontalAlign="Left" Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText=" Marine/Non Marine Fare">
                            <ItemStyle HorizontalAlign="Left" Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmarinefare" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISMARINEFAREYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="155px" HeaderText="Approved Amount(USD)">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApprovedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDTOTALUSDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="85px" HeaderText="Approved By">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApproveBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.APPROVERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="Approved Date">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApproveDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTRAVELAPPROVEDDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="PO Date">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPODate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDPOSENTDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Approve" ID="cmdApprove" CommandName="APPROVE"
                                    ToolTip="Approve" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Finalize" ID="cmdFinalize" CommandName="FINALIZE"
                                    ToolTip="Finalize" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-check-circle-fq"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Email" ID="cmdSemdMail" CommandName="SENDMAIL"
                                    CommandArgument='<%# Container.DataSetIndex %>' ToolTip="Send PO" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-envelope"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true"
                        ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table>
                <tr>
                    <td>
                        <img id="imgflag" runat="server" src="<%$ PhoenixTheme:images/14.png %>" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFInalizedQuote" runat="server" Text="- Finalized Quote"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <br />
            <b>&nbsp;Passengers List</b>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvQuotePassengers" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvQuotePassengers_ItemCommand" OnNeedDataSource="gvQuotePassengers_NeedDataSource"
                OnItemDataBound="gvQuotePassengers_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblrequestid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblisapproved" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISAPPROVED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRankHeader" runat="Server">Rank</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="On/Off-Signer">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOnOffSignerHeader" runat="server">On/Off-Signer</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYESNO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passport No">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassport" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Mode">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" Width="15px"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPaymentModeHeader" runat="server">Payment</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaymentmode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPAYMENTMODENAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Origin">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblorg" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldest" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Fare">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tax">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTTAX") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Amount">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTTOTALAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Amount(USD)(Incl. Fare + Tax)">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalAmountusd" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Travel Status">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblTravelstatusHeader" runat="server">Travel Status</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltravelstatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDISCANCELLEDYNSTATUS")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblcancellyn" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDISCANCELLEDYN")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Quotation Number">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblQuotationNumberHeader" runat="server">Quotation No</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuotationNumber" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUOTATIONREFNO") +" "+ "-" + " "+DataBinder.Eval(Container, "DataItem.FLDAGENTCODE")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblQuoteId" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSELECTEDQUOTEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Approve" ID="cmdApprove" CommandName="APPROVE"
                                    ToolTip="Approve" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Revoke approval" ID="cmdDeApprove" CommandName="DEAPPROVE"
                                    ToolTip="Revoke approval" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-redo"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Send Mail" ID="cmdSemdMail" CommandName="SENDMAIL"
                                    ToolTip="Send Mail" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-envelope"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true"
                        ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <telerik:RadLabel ID="lblpsgdetail" runat="server" Text="Passengers Travel Details" Font-Bold="true"></telerik:RadLabel>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvLineItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvLineItem_ItemCommand" OnNeedDataSource="gvLineItem_NeedDataSource"
                OnItemDataBound="gvLineItem_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRouteID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROUTEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblQuotationID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblBreakupID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBREAKUPID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblattachmentyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>' CommandName="SELECT">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Origin">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrgin" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGIN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDestination" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDEPARTUREDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREAMPM") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container,"DataItem.FLDARRIVALAMPM") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Stops">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnknostop" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFSTOPS") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Duration">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDuration" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Airline Code">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle HorizontalAlign="Center" />                      
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblairlinecode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRLINECODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Fare">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>                      
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tax">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>                      
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <ItemStyle Wrap="false" HorizontalAlign="Center" />                        
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Attach Ticket" ToolTip="Attach Ticket" Width="20PX" Height="20PX"
                                    CommandName="Attachment" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAttachment">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Itinerary" ToolTip="Itinerary" Width="20PX" Height="20PX"
                                    CommandArgument="<%# Container.DataSetIndex %>" ID="cmdShowReason">
                               <span class="icon"><i class="fas fa-list-alt-Itinerary"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Confirm ID="ucConfirmMessage" runat="server" Text="" Visible="false" OnConfirmMesage="btn_approve" />
        </telerik:RadAjaxPanel>        
    </form>
</body>
</html>
