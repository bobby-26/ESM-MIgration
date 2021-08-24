<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelAgentInvoiceUpload.aspx.cs"
    Inherits="CrewTravelAgentInvoiceUpload" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Travel Request Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
        </script>
        <script type="text/javascript">
            function fileUploaded(sender, args) {
                document.getElementById("<%= btnattachment.ClientID %>").click();
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form2" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager ID="RadWindowManager" runat="server"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%" EnableAJAX="false">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenutravelInvoice" runat="server" OnTabStripCommand="MenutravelInvoice_OnTabStripCommand"></eluc:TabStrip>
            <telerik:RadWindow RenderMode="Lightweight" ID="modalPopup" runat="server" Width="360px" Height="365px" Modal="true" OffsetElementID="main"
                Style="z-index: 100001;">
                <ContentTemplate>
                    <table id="tblnotes" runat="server" style="color: Blue">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel7" runat="server" Font-Bold="true" Text="Notes:"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="1. Click the link and download the Statement excel file."></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr></tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel2" runat="server" Text="2. In that file please fill the invoice details then save and upload the file."></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr></tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel3" runat="server" Text="3. The number of columns in the statement excel sheet should be 21."></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr></tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel4" runat="server" Text="4. All the column cells in statement excel sheet should be in 'text' format."></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr></tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel5" runat="server" Text="5. If any exception occurs kindly click the 'Refresh' button, correct the statement excel sheet data and upload again."></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr></tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel6" runat="server" Text="6. Finally click 'Confirm' to confirm the Statement upload."></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </telerik:RadWindow>
            <br />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblChooseaFile" runat="server" Text="Choose a file"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:LinkButton ID="btnattachment" runat="server"></asp:LinkButton>
                        <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" ID="FileUploadInvoice" OnClientFileUploaded="fileUploaded"
                            OnFileUploaded="FileUploadInvoice_FileUploaded" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkDownloadExcel" runat="server" Text="Click here to download format for statement" 
                            OnClick="lnkDownloadExcel_Click"></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <hr />
            <br />

            <telerik:RadGrid RenderMode="Lightweight" ID="gvInvoiceUploadBulk" runat="server" AutoGenerateColumns="False" Height="75%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="true"
                OnNeedDataSource="gvInvoiceUploadBulk_NeedDataSource" EnableHeaderContextMenu="true" OnItemCommand="gvInvoiceUploadBulk_ItemCommand"
                OnItemDataBound="gvInvoiceUploadBulk_ItemDataBound">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Date (DD/MM/YYYY)" Name="Date" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Exceptions" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="imgIvalidTkt" Visible="false" ToolTip="Invalid Ticket No."
                                    CommandName="INVALIDTICKETNO" CommandArgument="<%# Container.DataSetIndex %>" Enabled="false" Width="15px" Height="15px">
                                 <span class="icon" id="imginvalid" style="color:red;" ><i class="fas fa-ban"></i></span>      
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="imgDuplicateTicket" Visible="false" ToolTip="Duplicate Ticket No. Uploaded More Than Once"
                                    CommandName="DUPLICATETICKETNO" CommandArgument="<%# Container.DataSetIndex %>" Enabled="false" Width="15px" Height="15px">
                                 <span class="icon" id="imgDuplicate" style="color:orange;" ><i class="far fa-copy"  ></i></span>      
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="imgInvDate3daysBefore" Visible="false" ToolTip="Invoice Date is not 3 days later From Depature Date"
                                    CommandName="THREEDAYSFROMDEPDATE" CommandArgument="<%# Container.DataSetIndex %>" Enabled="false" Width="15px" Height="15px">
                                 <span class="icon" id="imgInvDate" style="color:red;" ><i class="fas fa-file-alt"  ></i></span>      
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="ImgAmountExeeded" Visible="false" ToolTip="Amount Exceeded"
                                    CommandName="AMOUNTEXCEEDED" CommandArgument="<%# Container.DataSetIndex %>" Enabled="false" Width="15px" Height="15px">
                                 <span class="icon" id="imgAmount" style="color:goldenrod;" ><i class="fas fa-coins"></i></span>    
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="ImgInvoiceAlreadyPosted" Visible="false" ToolTip="Invoice Already Uploaded"
                                    CommandName="INVOICEPOSTED" CommandArgument="<%# Container.DataSetIndex %>" Enabled="false" Width="15px" Height="15px">
                                 <span class="icon" id="imgInvoice"><i class="fas fa-file-contract"  ></i></span>      
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="imgInvalidPONumber" Visible="false" ToolTip="Invalid PO Number"
                                    CommandName="INVALIDPO" CommandArgument="<%# Container.DataSetIndex %>" Enabled="false" Width="15px" Height="15px">
                                 <span class="icon" id="imgInvalidPO" style="color:red;" ><i class="fas fa-star"></i></span>   
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="ImgCommited" Visible="false" ToolTip="Ticket Not Confirmed"
                                    CommandName="COMMITED" CommandArgument="<%# Container.DataSetIndex %>" Enabled="false" Width="15px" Height="15px">
                                 <span class="icon" id="imgNotConfirmed" style="color:orange;" ><i class="fas fa-star"  ></i></span>      
                                </asp:LinkButton>
                                <telerik:RadLabel ID="lblInvalidTktNoYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDINAVLIDTICKETYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInvalidPOYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDINVALIDPONUMBERYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDuplicateTicketNoYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDUPLICATEINVOICEUPLOADYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInvoicePosted" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDINVOICEPOSTEDYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDepDateMismatch" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDEPDATEMISMATCHEDYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAmountExceeded" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAMTEXCEEDEDYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbl3DaysFromDepDate" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLD3DAYSFROMDEPDATE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCommitedYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMMITEDYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice No." AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAgentInvoiceBulkID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAGENTINVOICEBULKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice" ColumnGroupName="Date" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="7%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDINVOICEDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure"  ColumnGroupName="Date" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="7%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Request No." AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="9%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequisitionNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PNR No." AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPNR" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPNRNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passenger" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPASSENGERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselname" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Airline Code" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAirlineNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAIRLINENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ticket" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTicket" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTICKETNO") %>'
                                    CommandArgument="<%# Container.DataSetIndex %>" CommandName="SELECT"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total" AllowSorting="false" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChargers" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Imported On" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblimporteddate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
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
            <table cellpadding="1" cellspacing="2">
                <tr>
                    <td>
                        <span class="icon" style="color: red;"><i class="fas fa-ban"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblinvalidticketno" runat="server" Text="* Invalid Ticket No"></telerik:RadLabel>
                    </td>
                    <td>
                        <span class="icon" style="color: orange;"><i class="far fa-copy"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel8" runat="server" Text="* Duplicate Ticket No."></telerik:RadLabel>
                    </td>
                    <td>
                        <span class="icon" style="color: red;"><i class="fas fa-file-alt"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel9" runat="server" Text="* Invoice Date is not 3 days later From Depature Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <span class="icon" style="color: goldenrod;"><i class="fas fa-coins"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel10" runat="server" Text="* Amount Exceeded"></telerik:RadLabel>
                    </td>
                    <td>
                        <span class="icon"><i class="fas fa-file-contract"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel11" runat="server" Text="* Invoice Already Uploaded"></telerik:RadLabel>
                    </td>
                    <td>
                        <span class="icon" style="color: red;"><i class="fas fa-star"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel12" runat="server" Text="* Invalid PO Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <span class="icon" style="color: orange;"><i class="fas fa-star"></i></span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="RadLabel13" runat="server" Text="* Ticket Not Confirmed"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>


    </form>
</body>
</html>
