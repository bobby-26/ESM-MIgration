<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelQuoteTicketList.aspx.cs"
    Inherits="CrewTravelQuoteTicketList" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Request for Ticket</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadLabel ID="lblTitle" runat="server" Visible="false" Text="Ticket"></telerik:RadLabel>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
            <eluc:TabStrip ID="MenuTicket" runat="server" TabStrip="true" OnTabStripCommand="MenuTicket_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuTitle" runat="server"></eluc:TabStrip>
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
                            <td>&nbsp; 5. To generate new requisition click on &nbsp;<i class="fas fa-plane-departure-it"></i>&nbsp; button in the
                                    Req. column
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </telerik:RadWindow>
            <table cellpadding="2" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblQuotationNumber" runat="server" Text="Quotation Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtQuotationReference" runat="server" Width="120px" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCurrency" runat="server" Width="60px" CssClass="readonlytextbox"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <telerik:RadLabel ID="lblPassengersList" runat="server" Font-Bold="true" Text="Passengers List"></telerik:RadLabel>

            <eluc:TabStrip ID="MenuQuotationList" runat="server" OnTabStripCommand="MenuQuotationList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvLineItem" runat="server" AllowCustomPaging="true" AllowSorting="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvLineItem_ItemCommand" OnNeedDataSource="gvLineItem_NeedDataSource" AllowPaging="true"
                OnItemDataBound="gvLineItem_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    DataKeyNames="FLDROUTEID">
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
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="150px">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblReqisitionNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'
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
                                <telerik:RadLabel ID="lblAgentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAgentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGENTNAME") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblattachmentyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblFinalizedYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINALIZEDYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTicketCancelled" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELTICKETYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblNewRequestYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWREQUESTYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblNewReqMoved" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISNEWREQUISIONMOVED") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblQuotationRefno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONREFNO") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDepartureDatePassedYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPATUREDATEPASSEDYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                    CommandName="SELECT"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="110px" HeaderText="Origin">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrgin" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGIN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="110px" HeaderText="Destination">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDestination" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" HeaderText="Departure">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="90px" HeaderText="Arrival">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="60px" HeaderText="Stops">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNoOfStops" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFSTOPS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" HeaderText="Duration(Hrs)">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDuration" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDURATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" HeaderText="Fare">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtamount" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTAMOUNT") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" HeaderText="Tax">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTTAX") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txttax" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTTAX") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Travel Status" HeaderStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltravelstatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDISCANCELLEDYNSTATUS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="Action">
                            <HeaderStyle Wrap="false" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Passanger Details" ToolTip="Passanger Details" Width="20PX" Height="20PX"
                                    CommandName="VENDORDETAILS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPassangerDetails">
                                <span class="icon"><i class="fas fa-user"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Ticket" ToolTip="Ticket" Width="20PX" Height="20PX"
                                    CommandArgument='<%# Container.DataSetIndex %>' ID="cmdShowReason">
                                <span class="icon"><i class="fas fa-tag"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attach Ticket" ToolTip="Attach Ticket" 
                                    Width="20PX" Height="20PX" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAttachment">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                    CommandName="Save" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRowSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel" Visible="false">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
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

            <br />
            <b>&nbsp;<telerik:RadLabel ID="lblHopList" runat="server" Text="Hop List"></telerik:RadLabel>
            </b>
            <br />
            <eluc:TabStrip ID="MenuHopList" runat="server" OnTabStripCommand="MenuHopList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCTBreakUp" runat="server" AllowCustomPaging="true" AllowSorting="true" Height="30%"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCTBreakUp_ItemCommand" OnNeedDataSource="gvCTBreakUp_NeedDataSource" AllowPaging="false"
                OnItemDataBound="gvCTBreakUp_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    DataKeyNames="FLDHOPLINEITEMID" >
                    
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
                        <telerik:GridTemplateColumn HeaderStyle-Width="35px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" runat="server" EnableViewState="true" OnCheckedChanged="SaveCheckedValues" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="40px">
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Approve" ID="ImgApproved" ToolTip="Edit Approve" Width="20px" Height="20px" Visible="false">                                    
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Ticket Cancelled" Visible="false"
                                    CommandArgument='<%# Container.DataSetIndex %>' ID="imgTicketCancelApproved"
                                    ToolTip="Ticket Cancelled" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                                <telerik:RadLabel ID="lblTicketCancelApproved" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCANCELAPPROVEDYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="45px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="right" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSnoHeader" runat="server">S.No.</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblhoplineitemid" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOPLINEITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblHPtravelstatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDISCANCELLEDYNSTATUS")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblNewRequestYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWREQUESTYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblattachmentyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblattachmentMappingyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTMAPPINGYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAttachment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENT") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAttachmentCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTCODE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <%#DataBinder.Eval(Container, "DataItem.FLDSERIALNO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Origin" HeaderStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOrigHeader" runat="server">Origin</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDORIGIN")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblOriginEdit" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDORIGIN")%>'></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblhoplineitemidedit" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOPLINEITEMID") %>'></telerik:RadLabel>
                                <span id="spnPickListOriginOldbreakup" runat="server">
                                    <telerik:RadTextBox ID="txtOriginNameOldBreakup" runat="server" Width="80%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGIN") %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowOriginoldbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                        OnClientClick="return showPickList('spnPickListOriginOldbreakup', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtOriginIdOldBreakup" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                                <br />
                                <%--    <br />--%>
                                <span id="spnPickListOriginbreakup">
                                    <telerik:RadTextBox ID="txtOriginNameBreakup" runat="server" Width="80%" Enabled="False"
                                        CssClass="input_mandatory">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowOriginbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                        OnClientClick="return showPickList('spnPickListOriginbreakup', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtOriginIdBreakup" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination" HeaderStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDestHeader" runat="server">Destination</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDDESTINATION")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDestinationEdit" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDESTINATION")%>'></telerik:RadLabel>
                                <span id="spnPickListDestinationOldbreakup">
                                    <telerik:RadTextBox ID="txtDestinationNameOldBreakup" runat="server" Width="80%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATION") %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowDestinationOldbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                        OnClientClick="return showPickList('spnPickListDestinationOldbreakup', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtDestinationIdOldBreakup" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                                <br />
                                <%--   <br />--%>
                                <span id="spnPickListDestinationbreakup">
                                    <telerik:RadTextBox ID="txtDestinationNameBreakup" runat="server" Width="80%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATION") %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowDestinationbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                        OnClientClick="return showPickList('spnPickListDestinationbreakup', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtDestinationIdBreakup" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure Date" HeaderStyle-Width="120px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDepDateHeader" runat="server">Departure</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDEPARTUREDATE", "{0:dd/MM/yyyy}"))%>'></telerik:RadLabel><br />
                                <telerik:RadLabel ID="LBLDEPARTUREAMPM" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDateEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MM/yyyy}") %>'></telerik:RadLabel>
                                <eluc:Date runat="server" ID="txtDepartureDateOld" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MM/yyyy}") %>'></eluc:Date>
                                <telerik:RadComboBox ID="ddldepartureampmold" runat="server" CssClass="dropdown_mandatory" Width="55px"
                                    EnableLoadOnDemand="True" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                                <br />
                                <eluc:Date runat="server" ID="txtDepartureDate" CssClass="input_mandatory"></eluc:Date>
                                <telerik:RadComboBox ID="ddldepartureampm" runat="server" CssClass="dropdown_mandatory" Width="55px"
                                    EnableLoadOnDemand="True" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival Date" HeaderStyle-Width="120px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblArrDateHeader" runat="server">Arrival</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDAteEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}")) %>'></telerik:RadLabel><br />
                                <telerik:RadLabel ID="lblARRIBALAMPM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDateEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}") %>'></telerik:RadLabel>
                                <eluc:Date runat="server" ID="txtArrivalDateOld" CssClass="input_mandatory"></eluc:Date>
                                <telerik:RadComboBox ID="ddlarrivalampmold" runat="server" CssClass="dropdown_mandatory" Width="55px"
                                    EnableLoadOnDemand="True" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                                <br />
                                <%--   <br />--%>
                                <eluc:Date runat="server" ID="txtArrivalDate" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}") %>'></eluc:Date>
                                <telerik:RadComboBox ID="ddlarrivalampm" runat="server" CssClass="dropdown_mandatory" Width="55px"
                                    EnableLoadOnDemand="True" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="70px" HeaderText="Ticket No.">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicketNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblTicketNoEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="txtTicketNoEditOld" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></telerik:RadLabel>
                                <br />
                                <telerik:RadLabel ID="txtTicketNoEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="PNR No.">
                            <HeaderStyle Wrap="false" HorizontalAlign="left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpnrno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPNRNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblpnrnoEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPNRNO") %>'></telerik:RadLabel>
                                <telerik:RadTextBox runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPNRNO") %>'
                                    ID="txtpnrnoOld" Width="80px">
                                </telerik:RadTextBox>
                                <br />
                                <telerik:RadTextBox runat="server" CssClass="input_mandatory" ID="txtpnrno"></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Airline Code">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAirlineCode" runat="server" Width="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRLINECODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblAirlineCodeEdit" runat="server" Visible="false" Width="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRLINECODE") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtAirlineCodeOld" CssClass="input_mandatory" Width="60px" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRLINECODE") %>'>
                                </telerik:RadTextBox>
                                <br />
                                <telerik:RadTextBox ID="txtAirlineCode" CssClass="input_mandatory" runat="server"></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Class">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblClassEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASS") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtClassOld" runat="server" Width="60px" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLASS") %>'></telerik:RadTextBox>
                                <br />
                                <telerik:RadTextBox ID="txtClass" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Amount">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblAmountEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtAmountOld" runat="server" CssClass="input_mandatory" Width="60px"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' />
                                <br />
                                <eluc:Number ID="txtAmount" runat="server" CssClass="input_mandatory" Width="60px" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="75px" HeaderText="Tax">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblTaxEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtTaxold" runat="server" CssClass="input_mandatory" Width="60px"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAX") %>' />
                                <br />
                                <eluc:Number ID="txtTax" runat="server" CssClass="input_mandatory" Width="60px" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ticket Status" HeaderStyle-Width="60px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicketStatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTICKETSTATUS")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTicketCancelledYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCANCELTICKETYN")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reason" HeaderStyle-Width="140px">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicketCancelledReason" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCANCELLEDREASONNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblTicketCancelledYNEdit" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELTICKETYN") %>'></telerik:RadLabel>
                                <eluc:Quick ID="ddlTktCancelledReasonEdit" runat="server" CssClass="input" QuickTypeCode="109" Width="100%"
                                    QuickList='<%#PhoenixRegistersQuick.ListQuick(1,110)%>' AppendDataBoundItems="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="New Req. Generated">
                            <HeaderStyle HorizontalAlign="left" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNewRequestGeneratedYN" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDNEWREQGENYNNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="170px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel Ticket" ToolTip="Cancel Ticket" Width="20PX" Height="20PX"
                                    CommandName="CANCELTICKET" CommandArgument='<%# Container.DataSetIndex %>' ID="imgCancelApproval">
                                <span class="icon"><i class="fas fa-times-circle-tc"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Details" ToolTip="Reissue Ticket" Width="20PX" Height="20PX"
                                    CommandName="REISSUE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdReissue">
                                <span class="icon"><i class="fas fa-plane-departure-de-select"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Initiate New Quote" ToolTip="Initiate New Quote" Width="20PX" Height="20PX"
                                    CommandName="INITIATENEWREQUEST" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdNewTravelRequest">
                                <span class="icon"><i class="fas fa-plane-departure-it"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Local Arrangements" ToolTip="Local Arrangements" Width="20PX" Height="20PX"
                                    CommandName="ARRANGEMENTS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdArrangements">
                                <span class="icon"><i class="fab fa-elementor"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attach Ticket" Visible="false" ToolTip="Attach Ticket" Width="20PX" Height="20PX"
                                    CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAttachment">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="View Ticket" ToolTip="View Ticket" Width="20PX" Height="20PX"
                                    CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAttachmentMapping">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                             <%--   <asp:LinkButton runat="server" CommandName="MOVETICKET"
                                    CommandArgument="<%# Container.DataSetIndex %>" ID="cmdmvTicket"
                                    ToolTip="Move Ticket">
                                <span class="icon"> <i class="fa fa-share-square-24"></i></span>
                                </asp:LinkButton>--%>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Save" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRowSave"
                                    ToolTip="Save Break Journey" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>

                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="4" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="NewTravelRequisition_Confirm"
                Visible="false" />
            <eluc:UserControlStatus ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
