<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCostEvaluationQuoteRFQ.aspx.cs"
    Inherits="CrewCostEvaluationQuoteRFQ" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="../UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Cost RFQ</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
    <div style="margin: 0 auto; width: 1024px; text-align: left;">
        <form id="frmCrewCostQuotateRFQ" runat="server">
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnableScriptCombine="false"></telerik:RadScriptManager>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
            <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                <div style="height: 60px;" class="RadToolBar RadToolBar_Horizontal RadToolBar_Windows7 RadToolBar_Windows7_Horizontal">
                    <div style="position: absolute; top: 15px;">
                        <img id="Img1" runat="server" style="vertical-align: middle" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>"
                            alt="Phoenix" onclick="parent.hideMenu();" />
                        <span class="title" style="color: black">
                            <%=Application["softwarename"].ToString() %></span>
                        <telerik:RadLabel runat="server" ID="lblDatabase"
                            ForeColor="Red" Font-Size="Large" Visible="false" Text="Testing on ">
                        </telerik:RadLabel>
                        <br />
                    </div>
                </div>
            </telerik:RadCodeBlock>
            <eluc:TabStrip ID="MenuQuote" runat="server" OnTabStripCommand="MenuQuote_TabStripCommand"></eluc:TabStrip>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="93%">
                <asp:Button ID="btnconfirm" runat="server" Text="btnconfirm" OnClick="confirm_Click" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <telerik:RadLabel ID="lblGuidelinestoFilltheRFQ" ForeColor="Blue" Font-Bold="true" runat="server" Text="Guidelines to Fill the RFQ:"></telerik:RadLabel>
                <ul>
                    <li>
                        <telerik:RadLabel ForeColor="Blue" ID="lblToQuotePleasecilcktheEditbuttonandUpdatetheQuotationreferencenoandcurrencyforeachport"
                            runat="server" Text="To Quote, Please cilck the 'Edit' button and Update the Quotation referenceno. and currency for each port">
                        </telerik:RadLabel>
                    </li>
                    <li>
                        <telerik:RadLabel ForeColor="Blue" ID="lblToQuoteDetailsselecttheportandclickthePorttheintheQuotationDetailsSectionclicktheEditbuttonandenteramountandclickSavebutton"
                            runat="server" Text="To Quote Details,Click the Port link button, and in 'Quote Details' click the 'Edit button' and enter amount and click 'Save' button">
                        </telerik:RadLabel>
                    </li>
                    <li>
                        <telerik:RadLabel ForeColor="Blue" ID="lblTosendthequotationoncompletionClickonSendtoOfficebutton" runat="server"
                            Text="To send the quotation on completion, Click on Send to Office button.">
                        </telerik:RadLabel>
                    </li>
                    <li>
                        <telerik:RadLabel ForeColor="Blue" ID="lblNoteYoucannotmakechangestothequotationonceyouSendtoOffice" runat="server"
                            Text="Note: You cannot make changes to the quotation once you Send to Office.">
                        </telerik:RadLabel>
                    </li>
                </ul>
                <br />
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRequestNo" runat="server" Text="Request No."></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtRequestNo" runat="server" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCrewChangeVessel" runat="server" Text="Crew Change Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtVessel" runat="server" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                        </td>

                        <td>
                            <telerik:RadLabel ID="lblNumberofOnSigners" runat="server" Text="Number of On-Signers"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOnSigners" runat="server" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNumberofOffSigners" runat="server" Text="Number of Off-Signers"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOffSigners" runat="server" Width="80%"></telerik:RadTextBox>
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
                <br />
                &nbsp;<telerik:RadLabel ID="lblQuotes" Font-Bold="true" runat="server" Text="Quotes"></telerik:RadLabel>

                <telerik:RadGrid RenderMode="Lightweight" ID="gvQuote" runat="server" EnableViewState="false"
                    AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                    OnNeedDataSource="gvQuote_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvQuote_ItemDataBound"
                    OnItemCommand="gvQuote_ItemCommand" OnUpdateCommand="gvQuote_UpdateCommand" ShowFooter="false"
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
                            <telerik:GridTemplateColumn HeaderText="Port" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblQuoteId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEID") %>' Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblAgentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <asp:LinkButton ID="lblPortName" CommandName="Select" CommandArgument='<%# Container.DataSetIndex %>'
                                        runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblApprovedYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDYN") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="ETA" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblETA" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETA") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="ETD" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblETD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETD") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Quotation No." AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblQtnNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEREFNO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblQuoteIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtQtnNoEdit" runat="server" CssClass="input_mandatory" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEREFNO") %>'></telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Currency" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Currency ID="ddlCurrencyEdit" runat="server" CssClass="dropdown_mandatory" Width="100%"
                                        AppendDataBoundItems="true" CurrencyList="<%#PhoenixRegistersCurrency.ListCurrency(null)%>"
                                        SelectedCurrency='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYID") %>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Total Amount" AllowSorting="false" ShowSortIcon="true">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" ShowSortIcon="true">
                                <ItemStyle HorizontalAlign="left" Wrap="False" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Communication" ID="cmdCommunication" CommandName="Communication" ToolTip="Chat">
                                    <span class="icon"><i class="fas fa-comments"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
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

                <br />
                &nbsp;<telerik:RadLabel ID="lblQuoteDetails" runat="server" Font-Bold="true" Text="Quote Details"></telerik:RadLabel>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvLineItem" runat="server" EnableViewState="false"
                    AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                    OnNeedDataSource="gvLineItem_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvLineItem_ItemDataBound"
                    OnItemCommand="gvLineItem_ItemCommand" OnUpdateCommand="gvLineItem_UpdateCommand" ShowFooter="true"
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
                            <telerik:GridTemplateColumn HeaderText="Section Type" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLineItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTELINEITEMID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblSectionTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONTYPENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Section" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSectionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECTIONNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterStyle HorizontalAlign="Left" Wrap="False" />
                                <FooterTemplate>
                                    <telerik:RadLabel ID="lblTotalAmount" runat="server" Text="Total Amount" Font-Bold="true"></telerik:RadLabel>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Amount" UniqueName="AMOUNT" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblLineItemIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTELINEITEMID") %>'
                                        Visible="false">
                                    </telerik:RadLabel>
                                    <eluc:MaskNumber ID="txtAmountEdit" runat="server" DecimalPlace="2" CssClass="input_mandatory" Width="100%"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadLabel ID="lblTotalA" runat="server" Text="Total Amount" Font-Bold="true"></telerik:RadLabel>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remarks" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtRemarksEdit" TextMode="MultiLine" runat="server" Width="100%"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'>
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="" HeaderStyle-Width="7%">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
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
        </form>
    </div>
</body>
</html>
