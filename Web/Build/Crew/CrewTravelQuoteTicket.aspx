<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelQuoteTicket.aspx.cs"
    Inherits="CrewTravelQuoteTicket" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Travel Ticket</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
        </script>
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
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <asp:Button ID="btnconfirm" runat="server" Text="btnconfirm" OnClick="btnApprove_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
            <eluc:TabStrip ID="MenuTicket" runat="server" OnTabStripCommand="MenuTicket_TabStripCommand" Title="Ticket Details"></eluc:TabStrip>
            <br clear="all" />
            <telerik:RadWindow RenderMode="Lightweight" ID="modalPopup" runat="server" Width="360px" Height="365px" Modal="true" OffsetElementID="main"
                Style="z-index: 100001;">
                <ContentTemplate>
                    <table id="Table2" style="color: Blue">
                        <tr>
                            <td>
                                <b>
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="Notes :"></telerik:RadLabel>
                                </b>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp; 1. To enter ticket number click on &nbsp;<i class="fas fa-edit"></i>&nbsp; button in the action column.
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp; 2. To paste the ticket click on &nbsp;<i class="fas fa-tag"></i>&nbsp; button in the ticket column.
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp; 3. To attach the ticket click on &nbsp;<i class="fas fa-paperclip"></i>&nbsp; or &nbsp;<i style="color: gray;" class="fas fa-paperclip-na"></i>&nbsp;
                            button in the ticket column.
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; a.&nbsp;<i class="fas fa-paperclip"></i>&nbsp; - [Blue Clip] Has attachments.
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; b.&nbsp;<i style="color: gray;" class="fas fa-paperclip-na"></i>&nbsp; - [Black Clip] No
                            attachments.
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp; 4. To view the passenger details click on &nbsp;<i class="fas fa-user"></i>&nbsp; button in the action
                            column.
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp; 5. Click on 'Confirm ticket' to issue the tickets.
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </telerik:RadWindow>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Reference No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReferenceNo" runat="server" Width="90%" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblQuotationNumber" runat="server" Text="Quotation No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtQuotationReference" runat="server" Width="90%" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" Text="Currency" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCurrency" runat="server" Width="90%" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <telerik:RadLabel ID="lblPassengersList" runat="server" Font-Bold="true" Text="Passengers List"></telerik:RadLabel>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvLineItem" runat="server" Height="37%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvLineItem_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvLineItem_ItemDataBound"
                OnItemCommand="gvLineItem_ItemCommand" OnUpdateCommand="gvLineItem_UpdateCommand" ShowFooter="false" OnDeleteCommand="gvLineItem_DeleteCommand"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDROUTEID">
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
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" ShowSortIcon="true">
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
                                <telerik:RadLabel ID="lblTravelDtkey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDTKEY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblBreakupID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBREAKUPID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblQuotationRefno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONREFNO") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblattachmentyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                    CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Origin" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrgin" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGIN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDestination" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEffectiveDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Stops" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="57px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNoOfStops" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFSTOPS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Duration(hrs)" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDuration" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Class" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTravelClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELCLASSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Fare" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtamount" runat="server" CssClass="input_mandatory" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tax" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txttax" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Travel Status" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltravelstatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDISCANCELLEDYNSTATUS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ticket" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="75px">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Ticket" ToolTip="Ticket" Width="20PX" Height="20PX"
                                    CommandArgument='<%# Container.DataSetIndex %>' ID="cmdShowReason">
                                <span class="icon"><i class="fas fa-tag"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attach Ticket" ToolTip="Attach Ticket"
                                    Width="20PX" Height="20PX" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAttachment">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Passanger Details" ID="cmdPassangerDetails" CommandName="VENDORDETAILS" ToolTip="Passenger Details" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-user"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" ID="cmdSave" CommandName="Update" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdRowSave" Visible="false" CommandName="Save" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px">
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

            &nbsp;<telerik:RadLabel ID="lblHopList" runat="server" Text="Hop List" Font-Bold="true"></telerik:RadLabel>
            <br />
            <eluc:TabStrip ID="MenuHopList" runat="server" OnTabStripCommand="MenuHopList_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCTBreakUp" runat="server" Height="37%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvCTBreakUp_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvCTBreakUp_ItemDataBound" OnEditCommand="gvCTBreakUp_EditCommand"
                OnItemCommand="gvCTBreakUp_ItemCommand" OnUpdateCommand="gvCTBreakUp_UpdateCommand" ShowFooter="false" OnDeleteCommand="gvCTBreakUp_DeleteCommand"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="120px" />
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
                        <telerik:GridTemplateColumn HeaderText="Exceptions" UniqueName="EXCEPTIONS" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="74px">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="imgTicketCancelApproved" Visible="false" CommandArgument="<%# Container.DataSetIndex %>" Enabled="false" Width="15px" Height="15px">
                                 <span class="icon" id="imgFlagcolor" style="color:red;" ><i class="fas fa-star-red"  ></i></span>      
                                </asp:LinkButton>
                                <telerik:RadLabel ID="lblTicketCancelApproved" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCANCELAPPROVEDYN") %>'></telerik:RadLabel>
                                <asp:LinkButton runat="server" ID="ImgApproved" Visible="false" CommandArgument="<%# Container.DataSetIndex %>" Enabled="false"
                                    Width="15px" Height="15px">
                                 <span class="icon"><i class="fas fa-award"></i></span>     
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="48px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblattachmentyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblattachmentMappingyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTMAPPINGYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAttachmentCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTCODE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAttachment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENT") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblhoplineitemid" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOPLINEITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblsno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Origin" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrgin" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGIN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblhoplineitemidedit" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOPLINEITEMID") %>'></telerik:RadLabel>
                                <span id="spnPickListOriginOldbreakup">
                                    <telerik:RadTextBox ID="txtOriginNameOldBreakup" runat="server" Width="100%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGIN") %>'>
                                    </telerik:RadTextBox>
                                    <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top"
                                        ID="btnShowOriginoldbreakup" ToolTip="Select BudgetCode" CommandName="BUDGETCODE" CommandArgument="<%# Container.DataSetIndex %>"
                                        OnClientClick="return showPickList('spnPickListOriginOldbreakup', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); ">
                                        <span class="icon"><i class="fas fa-list-alt"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtOriginIdOldBreakup" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                                <br />
                                <br />
                                <span id="spnPickListOriginbreakup">
                                    <telerik:RadTextBox ID="txtOriginNameBreakup" runat="server" Width="100%" Enabled="False"
                                        CssClass="input_mandatory">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top"
                                        ID="btnShowOriginbreakup" ToolTip="Select Budget Code" CommandName="BUDGETCODE" CommandArgument="<%# Container.DataSetIndex %>"
                                        OnClientClick="return showPickList('spnPickListOriginbreakup', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); ">
                                        <span class="icon"><i class="fas fa-list-alt"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtOriginIdBreakup" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDestination" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListDestinationOldbreakup">
                                    <telerik:RadTextBox ID="txtDestinationNameOldBreakup" runat="server" Width="100%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATION") %>'>
                                    </telerik:RadTextBox>
                                    <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top"
                                        ID="btnShowDestinationOldbreakup" ToolTip="Select Budget Code" CommandName="BUDGETCODE" CommandArgument="<%# Container.DataSetIndex %>"
                                        OnClientClick="return showPickList('spnPickListDestinationOldbreakup', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); ">
                                        <span class="icon"><i class="fas fa-list-alt"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtDestinationIdOldBreakup" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                                <br />
                                <br />
                                <span id="spnPickListDestinationbreakup">
                                    <telerik:RadTextBox ID="txtDestinationNameBreakup" runat="server" Width="100%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATION") %>'>
                                    </telerik:RadTextBox>
                                    <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top"
                                        ID="btnShowDestinationbreakup" ToolTip="Select Budget Code" CommandName="BUDGETCODE" CommandArgument="<%# Container.DataSetIndex %>"
                                        OnClientClick="return showPickList('spnPickListDestinationbreakup', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); ">
                                        <span class="icon"><i class="fas fa-list-alt"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtDestinationIdBreakup" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="151px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEffectiveDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="LBLDEPARTUREAMPM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtDepartureDateOld" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MM/yyyy}") %>'></eluc:Date>
                                <telerik:RadComboBox ID="ddldepartureampmold" runat="server" CssClass="dropdown_mandatory"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="AM" />
                                        <telerik:RadComboBoxItem Value="2" Text="PM" />
                                    </Items>
                                </telerik:RadComboBox>
                                <br />
                                <br />
                                <eluc:Date runat="server" ID="txtDepartureDate" CssClass="input_mandatory"></eluc:Date>
                                <telerik:RadComboBox ID="ddldepartureampm" runat="server" CssClass="dropdown_mandatory"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="AM" />
                                        <telerik:RadComboBoxItem Value="2" Text="PM" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="151px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblARRIBALAMPM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtArrivalDateOld" CssClass="input_mandatory"></eluc:Date>
                                <telerik:RadComboBox ID="ddlarrivalampmold" runat="server" CssClass="dropdown_mandatory"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="AM" />
                                        <telerik:RadComboBoxItem Value="2" Text="PM" />
                                    </Items>
                                </telerik:RadComboBox>
                                <br />
                                <br />
                                <eluc:Date runat="server" ID="txtArrivalDate" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}") %>'></eluc:Date>
                                <telerik:RadComboBox ID="ddlarrivalampm" runat="server" CssClass="dropdown_mandatory"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="1" Text="AM" />
                                        <telerik:RadComboBoxItem Value="2" Text="PM" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ticket No." AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="100px">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicketNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="txtTicketNoEditOld" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></telerik:RadLabel>
                                <br />
                                <br />
                                <telerik:RadLabel ID="txtTicketNoEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PNR No." AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpnrno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPNRNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPNRNO") %>'
                                    ID="txtpnrnoOld" Width="100%">
                                </telerik:RadTextBox>
                                <br />
                                <br />
                                <telerik:RadTextBox runat="server" CssClass="input_mandatory" ID="txtpnrno" Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Airline Code" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="100px">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAirlineCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRLINECODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtAirlineCodeOld" CssClass="input_mandatory" Width="100%" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRLINECODE") %>'>
                                </telerik:RadTextBox>
                                <br />
                                <br />
                                <telerik:RadTextBox ID="txtAirlineCode" CssClass="input_mandatory" Width="100%" runat="server"></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Class" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtClassOld" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASS") %>'></telerik:RadTextBox>
                                <br />
                                <br />
                                <telerik:RadTextBox ID="txtClass" runat="server" Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtAmountOld" runat="server" CssClass="input_mandatory" Width="100%"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' />
                                <br />
                                <br />
                                <eluc:Number ID="txtAmount" runat="server" CssClass="input_mandatory" Width="100%" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tax" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="100px">
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtTaxold" runat="server" CssClass="input_mandatory" Width="100%"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>' />
                                <br />
                                <br />
                                <eluc:Number ID="txtTax" runat="server" CssClass="input_mandatory" Width="100%" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ticket Status" UniqueName="TICKETSTATUS" AllowSorting="false" HeaderStyle-Width="63px" ShowSortIcon="true">
                            <ItemStyle HorizontalAlign="Right" Wrap="true" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicketStatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTICKETSTATUS")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTicketCancelledYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCANCELTICKETYN")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDITROW" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Add Hop" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdTravelBreakUp">
                                <span class="icon"><i class="fas fa-plane"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" ID="cmdAttachment" Visible="false" ToolTip="Attach Ticket" CommandName="Attachment" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" ID="cmdAttachmentMapping" ToolTip="View Ticket" CommandName="AttachmentMapping" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save Break Journey" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Save" Visible="false" ID="cmdRowSave" CommandName="Save" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px">
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
            <table cellpadding="1" cellspacing="2">
                <tr>
                    <td>
                        <span class="icon" style="color: red;"><i class="fas fa-star-red"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompletedRequest" runat="server" Text="* Ticket Cancelled"></telerik:RadLabel>
                    </td>
                    <td>
                        <span class="icon"><i class="fas fa-award"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="* Approved"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
