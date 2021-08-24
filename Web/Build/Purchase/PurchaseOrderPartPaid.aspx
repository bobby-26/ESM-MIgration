<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseOrderPartPaid.aspx.cs"
    Inherits="PurchaseOrderPartPaid" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Order Part Paid</title>
    <telerik:RadCodeBlock ID="RadCodeBlock" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOrderPartPaid" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuPurchasePartPaid">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuPurchasePartPaid" />
                        <telerik:AjaxUpdatedControl ControlID="rgvPartPaid" UpdatePanelHeight="75%" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rgvPartPaid">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvPartPaid" UpdatePanelHeight="75%" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuOrderForm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvPartPaid" UpdatePanelHeight="75%" />
                        <telerik:AjaxUpdatedControl ControlID="MenuOrderForm" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server" RenderMode="Lightweight"></telerik:RadAjaxLoadingPanel>


        <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%">

            <eluc:TabStrip ID="MenuPurchasePartPaid" runat="server" OnTabStripCommand="MenuPurchasePartPaid_TabStripCommand"
                TabStrip="true" Visible="false"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />

            <br clear="all" />
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblOrderNumber" runat="server" Text="Order Number :"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtOrderNumber" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="rgvPartPaid" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="100%"
                CellSpacing="0" GridLines="None" OnNeedDataSource="rgvPartPaid_NeedDataSource" OnItemCommand="rgvPartPaid_ItemCommand" OnItemDataBound="rgvPartPaid_ItemDataBound"
                OnUpdateCommand="rgvPartPaid_UpdateCommand" OnEditCommand="rgvPartPaid_EditCommand" OnDeleteCommand="rgvPartPaid_DeleteCommand" ShowFooter="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDORDERID,FLDDTKEY,FLDISATTACHMENT,FLDORDERPARTPAIDID,FLDSHORTNAME">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Description" UniqueName="DESCRIPTION">
                            <ItemStyle Width="300px" />
                            <HeaderStyle Width="300px" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDescription" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                    MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" MaxLength="200" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Amount" UniqueName="AMOUNT">
                            <ItemStyle Width="120px" HorizontalAlign="Right" />
                            <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--<eluc:MaskNumber ID="txtAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'
                                        MaskText="999,999,999.99" IsPositive="true" DecimalPlace="2" MaxLength="14"></eluc:MaskNumber>--%>
                                <eluc:Decimal ID="txtAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' MinValue="0" DecimalDigits="2"
                                    InterceptArrowKeys="false" InterceptMouseWheel="false" Width="100%"></eluc:Decimal>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Decimal ID="txtAmountAdd" runat="server" MinValue="0" DecimalDigits="2" InterceptArrowKeys="false" InterceptMouseWheel="false" Width="100%"></eluc:Decimal>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Voucher Number" UniqueName="VOUCHERNUMBER">
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="VOUCHERDATE" HeaderText="Voucher Date">
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Adv. Pymt. Status" UniqueName="ADVPYMTSTATUS">
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCEPAYSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="REMARKS" HeaderText="Remarks">
                            <ItemStyle Width="300px" />
                            <HeaderStyle Width="300px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREJECTREMARKS").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDREJECTREMARKS").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDREJECTREMARKS").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucRemarksTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREJECTREMARKS") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ACTION">
                            <ItemStyle Width="120px" HorizontalAlign="Center" />
                            <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Approve"
                                    CommandName="APPROVE" ID="cmdApprove" ToolTip="Approve">
                                            <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="Attachment" ID="cmdAtt" ToolTip="Attachment">
                                            <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Report" CommandName="Report"
                                    ID="cmdReport" ToolTip="Report">
                                    <span class="icon"><i class="fas fa-book"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="PAYMENTCANCEL" ID="imgCancel" ToolTip="Cancel" Visible="false">
                                            <span class="icon fa-stack"><i class="fas fa-award fa-stack-1x"></i><i class="fas fa-slash fa-stack-1x"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Approval History"
                                    CommandName="ApprovalHistory" ID="cmdApprovalHistory" ToolTip="Approval History" Visible="false">
                                            <span class="icon"><i class="fas fa-history"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" ID="cmdSave" ToolTip="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add"
                                    CommandName="ADD" ID="cmdAdd" ToolTip="Add">
                                            <span class="icon"><i class="fas fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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

                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                </ClientSettings>
            </telerik:RadGrid>
        </div>

    </form>
</body>
</html>
