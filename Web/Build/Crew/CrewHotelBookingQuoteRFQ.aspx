<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHotelBookingQuoteRFQ.aspx.cs"
    Inherits="CrewHotelBookingQuoteRFQ" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="../UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tax" Src="~/UserControls/UserControlTaxMaster.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Hotel RFQ</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function btnconfirm(args) {
                if (args) {
                    __doPostBack("<%=btnconfirm.UniqueID %>", "");
                }
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmHotelBookingQuotateRFQ" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <eluc:TabStrip ID="MenuQuote" runat="server" OnTabStripCommand="MenuQuote_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <asp:Button ID="btnconfirm" runat="server" Text="btnconfirm" OnClick="Confirm_Click" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCheckinDate" runat="server" Text="Checkin Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtCheckinDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" DatePicker="false"></eluc:Date>
                        <telerik:RadTextBox ID="txtTimeOfCheckIn" runat="server" CssClass="readonlytextbox" Width="50px" ReadOnly="true" />
                        <telerik:RadLabel ID="lblCheckInhrs" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCheckoutDate" runat="server" Text="Checkout Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtCheckoutDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" DatePicker="false"></eluc:Date>
                        <telerik:RadTextBox ID="txtTimeOfCheckOut" runat="server" CssClass="readonlytextbox" Width="50px" ReadOnly="true" />
                        <telerik:RadLabel ID="lblCheckOuthrs" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                            Text="1.To Quote, provide the number of rooms, extra beds,charges ,discount,tax. Click on Save button to save the information.</br>2.To send the quotation on completion, Click on Send to Office button. </br> Note:You cannot make changes to the quotation once you Send to Office.If you want to change after click Send To Office button ">
                        </telerik:RadToolTip>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNumberOfNights" runat="server" Text="Number Of Nights"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNoOfNights" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRoomType" runat="server" Text="Room Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRoomType" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNumberofBeds" runat="server" Text="Number of Beds"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNoOfBeds" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblExtraBeds" runat="server" Text="Extra Beds"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtExtraBeds" runat="server" CssClass="readonlytextbox" ispositive="true"
                            ReadOnly="true" isinteger="true" MaxLength="1" defaultzero="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRoomsRquest" runat="server" Text="Rooms Rquest"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNoOfRooms" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDayUse" runat="server" Text="Day Use"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDayUseYN" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="20">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSentby" runat="server" Text="Sent by"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSenderName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="200px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSendersEMailId" runat="server" Text="Sender's E-Mail Id"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSenderMail" ReadOnly="true" CssClass="readonlytextbox"
                            Width="200px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlQuote" runat="server" GroupingText="Quote">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblQuotedRooms" runat="server" Text="Quoted Rooms"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtNoOfRoomsQuote" CssClass="input_mandatory" runat="server"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDNOOFROOMS") %>' IsInteger="true"
                                IsPositive="true" MaxLength="2" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblQuotedExtraBeds" runat="server" Text="Quoted Extra Beds"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtExtraBedsQuote" runat="server" IsInteger="true" IsPositive="true"
                                MaxLength="2" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Currency ID="ucCurrency" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAmount" runat="server" Text="Charges(Per Room)"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT")%>'
                                DecimalPlace="2" IsPositive="true" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblExtraBedCharges" runat="server" Text="Extra Bed Charges"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtExtraBedAmount" runat="server" IsPositive="true" DecimalPlace="2"
                                CssClass="input" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDisc" runat="server" Text="Discount (%)"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:MaskNumber ID="txtDiscount" runat="server" IsPositive="true" DecimalPlace="2"
                                CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTotalAmount" runat="server" Text="Total Amount"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTotalAmount" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <b>
                <telerik:RadLabel ID="lblTax" runat="server" Text="Tax"></telerik:RadLabel>
            </b>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvTax" runat="server" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvTax_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvTax_ItemDataBound1"
                OnItemCommand="gvTax_ItemCommand" OnUpdateCommand="gvTax_UpdateCommand" ShowFooter="true" OnDeleteCommand="gvTax_DeleteCommand"
                AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="Tax/Others Charge Description" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBookingId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBOOKINGID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                    CssClass="gridinput_mandatory" MaxLength="50">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="lblBookingIdEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBOOKINGID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDescriptionAdd" Text='' runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="50">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:TaxType ID="ucTaxTypeEdit" runat="server" TaxType='<%# DataBinder.Eval(Container,"DataItem.FLDTAXTYPE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:TaxType ID="ucTaxTypeAdd" runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Value" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE" ,"{0:f2}") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtTaxMapId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTETAXMAPID") %>'></telerik:RadTextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtTaxMapIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTETAXMAPID") %>'></telerik:RadTextBox>
                                <eluc:Decimal ID="txtValueEdit" runat="server" CssClass="gridinput_mandatory" Mask="999,999.99" MaxLength="200" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE","{0:f2}") %>'
                                    Width="90px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Decimal ID="txtValueAdd" runat="server" CssClass="gridinput_mandatory" Mask="999,999.99"
                                    Width="90px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTaxAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXAMOUNT","{0:f2}") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdAdd" CommandName="Add" ToolTip="Add New" Width="20px" Height="20px">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
