<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationVendorDetail.aspx.cs"
    Inherits="PurchaseQuotationVendorDetail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quotation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function PaneResized(sender, args) {
                var splitter = $find('RadSplitter1');
                var browserHeight = $telerik.$(window).height();
                splitter.set_height(browserHeight - 40);
                splitter.set_width("100%");
                var grid = $find("rgvQuotation");
                var contentPane = splitter.getPaneById("listPane");
                grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 110) + "px";
            }
            function pageLoad() {
                PaneResized();
            }
            function order(args) {
                if (args) {
                    __doPostBack("<%=hideamount.UniqueID %>", "");
                } else {
                    __doPostBack("<%=showamount.UniqueID %>", "");
                }
            }
            function revokeconfirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmRevoke.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript">
            function ConfirmRFQXML(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmRFQXML.UniqueID %>", "");
                }
            }
            function ConfirmPOXML(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmPOXML.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>


</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmPurchaseQuotationVendorDetail" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" Position="BottomCenter"
            Animation="Fade" AutoTooltipify="false" Width="300px" Font-Size="Large" RenderInPageRoot="true" AutoCloseDelay="80000">
            <TargetControls>
            </TargetControls>
        </telerik:RadToolTipManager>
        <%--<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuVendor">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuVendor" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="lnkVendorName">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                        <telerik:AjaxUpdatedControl ControlID="rgvQuotation" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rgvQuotation">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvQuotation" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucConfirm" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                        <telerik:AjaxUpdatedControl ControlID="MenuVendor" />
                        <telerik:AjaxUpdatedControl ControlID="ucConfirmRevoke" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuVendorList">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuVendorList" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucConfirm" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                        <telerik:AjaxUpdatedControl ControlID="rgvQuotation" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ucConfirmRevoke">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucConfirmRevoke" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                        <telerik:AjaxUpdatedControl ControlID="rgvQuotation" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>--%>
                <telerik:RadAjaxPanel runat="server" ID="radajaxpanelqtn" >
       
        <telerik:RadNotification ID="radnotificationstatus" RenderMode="Lightweight" runat="server" AutoCloseDelay="3500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"  
            EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false" ></telerik:RadNotification>
        
        <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false" />
        <eluc:TabStrip ID="MenuVendor" runat="server" OnTabStripCommand="MenuVendor_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <asp:Button ID="showamount" runat="server" Text="show" OnClick="showamount_Click" CssClass="hidden" />
        <asp:Button ID="hideamount" runat="server" Text="hide" OnClick="hideamount_Click" CssClass="hidden" />
        <asp:Button ID="ucConfirmRFQXML" runat="server" OnClick="ConfirmRFQXML_Click" CssClass="hidden" />
        <asp:Button ID="ucConfirmPOXML" runat="server" OnClick="ucConfirmPOXML_Click" CssClass="hidden" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
        <telerik:RadButton ID="ucConfirmRevoke" runat="server" Visible="false" OnClick="ucConfirmRevoke_Click"></telerik:RadButton>
  
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
            <telerik:RadPane ID="editPane" runat="server" Scrolling="None">

                <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 700px; width: 100%" frameborder="0"></iframe>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both"></telerik:RadSplitBar>
            <telerik:RadPane ID="listPane" runat="server" Scrolling="None" OnClientResized="PaneResized" Height="280">
                <eluc:TabStrip ID="MenuVendorList" runat="server" OnTabStripCommand="MenuVendorList_TabStripCommand"></eluc:TabStrip>

                <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="rgvQuotation" DecoratedControls="All" EnableRoundedCorners="true" />
              
                 <telerik:RadGrid RenderMode="Lightweight" ID="rgvQuotation" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnDeleteCommand="rgvQuotation_DeleteCommand" OnSortCommand="rgvQuotation_SortCommand"
                    OnNeedDataSource="rgvQuotation_NeedDataSource" OnItemDataBound="rgvQuotation_ItemDataBound" OnItemCommand="rgvQuotation_ItemCommand" OnPreRender="rgvQuotation_PreRender">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDORDERID,FLDQUOTATIONID,FLDDTKEY">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="" UniqueName="CHECKBOX">
                                <ItemStyle Width="25px" />
                                <HeaderStyle Width="25px" />
                                <ItemTemplate>
                                    <telerik:RadCheckBox ID="chkSelect" Checked="false" runat="server"></telerik:RadCheckBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="" UniqueName="FLAG">
                                <ItemStyle Width="50px" />
                                <HeaderStyle Width="50px" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgFlag" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" />
                                    <asp:ImageButton RenderMode="Lightweight" ID="imgdetailsFlag" runat="server" Enabled="false" ImageUrl="<% $PhoenixTheme:images/detail-flag.png%>"  ToolTip="Remarks exist"/>
                                    <telerik:RadLabel ID="lblIsSelected" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSELECTEDFORORDER") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblIsApproved" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.APPOVEDSTATUS") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblQuotationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblApprovalExists" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALEXISTS") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblApprovalType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONAPPROVAL") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblTechdirector" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTECHDIRECTOR") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblFleetManager" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLEETMANAGER") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSupdt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPT") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVendorId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblPortId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORT") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblFormType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWebSession" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWEBSESSIONSTATUS") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblActiveCur" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblremarksflag" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLGREMARKS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME">
                                <ItemStyle Width="250px" />
                                <HeaderStyle Width="250px" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkVendorName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' CommandName="SELECT"></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="sent Date" Visible="false" UniqueName="SENDDATE">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblSendDateCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSENTDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Received" UniqueName="RECEIVED">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblRecivedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Rejected Date" Visible="false">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblRejectedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREJECTEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Amount" UniqueName="AMOUNT">
                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALPRICE","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Discount" UniqueName="DISCOUNT">
                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALDISCOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Total" UniqueName="TOTAL">
                                <ItemStyle HorizontalAlign="Right" Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TOTALAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Del. Time (Days)" UniqueName="DELAYTIME">
                                <ItemStyle HorizontalAlign="Right" Width="60px" />
                                <HeaderStyle Width="60px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYTIME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Quoted" UniqueName="QUOTEDSTATUS">
                                <ItemStyle HorizontalAlign="Left" Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblSTATUS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.STATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Approval Status" UniqueName="APPROVALSTATUS">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblAppStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.APPOVEDSTATUS ").ToString() == "1" ? "Approved" : "Partially Approved" %>'></telerik:RadLabel>

                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Port" UniqueName="PORT">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ACTION">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="SELECTVENDOR" ID="cmdSelect" ToolTip="Select">
                                            <span class="icon"><i class="fas fa-check"></i></span>
                                    </asp:LinkButton>
                                    <asp:ImageButton runat="server" AlternateText="De-Select" ImageUrl="<%$ PhoenixTheme:images/de-select.png %>"
                                        CommandName="DESELECT" ID="cmdDeSelect" ToolTip="De-Select"></asp:ImageButton>

                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Vendor Details"
                                        CommandName="VENDORDETAILS" ID="cmdVendor" ToolTip="Vendor Details">
                                            <span class="icon"><i class="fas fa-address-book"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Export to Excel RFQ"
                                        CommandName="RFQEXCEL" ID="cmdExcelRFQ" ToolTip="Export to Excel RFQ">
                                            <span class="icon"><i class="fas fa-file-excel"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="View Queries Sent"
                                        CommandName="VIEWQUERIESSENT" ID="cmdViewQuery" ToolTip="View Queries Sent">
                                            <span class="icon"><i class="fas fas-envelop-open"></i></span>
                                    </asp:LinkButton>

                                    <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                        CommandName="APPROVE" ID="cmdApprove" ToolTip="Approve"></asp:ImageButton>

                                    <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                        CommandName="FALAPPROVE" ID="btnfalapprove" ToolTip="Approve" Visible="false"></asp:ImageButton>

                                    <asp:ImageButton runat="server" AlternateText="Revoke approval" ImageUrl="<%$ PhoenixTheme:images/cancel.png %>"
                                        CommandName="DEAPPROVE" ID="cmdDeApprove" ToolTip="Revoke approval"></asp:ImageButton>

                                    <asp:ImageButton runat="server" AlternateText="WhatIfQty" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                        CommandName="WHATIFQTY" ID="cmdWhatIfQty" ToolTip="What If Qty"></asp:ImageButton>

                                    <asp:ImageButton runat="server" AlternateText="Re-Quote" ImageUrl="<%$ PhoenixTheme:images/quotation-requote.png %>"
                                        CommandName="REQUOTE" ID="imgRequote" ToolTip="Allow to Re-quote"></asp:ImageButton>

                                    <asp:ImageButton runat="server" AlternateText="Audit Trail" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png %>"
                                        CommandName="AUDITTRAIL" ID="cmdAudit" ToolTip="Audit Trail"></asp:ImageButton>

                                    <asp:LinkButton runat="server" AlternateText="Send Acceptance Mail"
                                        CommandName="SENDACCEPTANCE" ID="cmdAcceptancemail" ToolTip="Send Acceptance Mail">
                                            <span class="icon"><i class="fas fa-envelope"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="PDF" CommandName="PDF" ID="cmdPDF" ToolTip="Export PDF">
                                            <span class="icon"><i class="fas fa-file-pdf"></i></span>
                                        
                                    </asp:LinkButton>
                                    <asp:ImageButton runat="server" AlternateText="RFQ xml" CommandName="RFQXML" ID="cmdRFQXML" Visible="false"
                                        ToolTip="Export RFQ to SeaProc" ImageUrl="<%$ PhoenixTheme:images/issue-licence.png %>">
                                    </asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="PO xml" CommandName="POXML" ID="cmdPOXML" Visible="false"
                                        ToolTip="Export PO to SeaProc" ImageUrl="<%$ PhoenixTheme:images/issue-licence.png %>">
                                    </asp:ImageButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Quotations matching your search criteria"
                            PageSizeLabelText="Quotations per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                    </ClientSettings>
                </telerik:RadGrid>
                    
                <div id="div2" style="position: relative;">
                    <table width="100%" border="0" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red">  All amounts are in USD. </asp:Label>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </div>
                <%--<eluc:Confirm ID="ucConfirmMessage" runat="server" OnConfirmMesage="CopyForm_Click" OKText="Hide PO Amount"
                        CancelText="Show PO Amount" Visible="false" />--%>
            </telerik:RadPane>
        </telerik:RadSplitter>
             </telerik:RadAjaxPanel>
    </form>
</body>
</html>
