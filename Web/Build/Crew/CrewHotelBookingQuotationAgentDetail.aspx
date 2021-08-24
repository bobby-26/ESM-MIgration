<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHotelBookingQuotationAgentDetail.aspx.cs"
    Inherits="CrewHotelBookingQuotationAgentDetail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function resize() {
            var obj = document.getElementById("ifMoreInfo");
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 150 + "px";
        }
    </script>
</head>
<body onload="resize()" onresize="resize()">
    <form id="frmHBQuotationAgentDetail" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuHotelBooking" runat="server" OnTabStripCommand="MenuHotelBooking_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
            <br />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReferenceNo" runat="server" ReadOnly="true" Width="81%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCheckinDate" runat="server" Text="Checkin Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtCheckinDate" runat="server" Enabled="false"></eluc:Date>
                        <telerik:RadTextBox ID="txtTimeOfCheckIn" runat="server" CssClass="readonlytextbox" Width="50px" />
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCheckoutDate" runat="server" Text="Checkout Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtCheckoutDate" runat="server" Enabled="false"></eluc:Date>
                        <telerik:RadTextBox ID="txtTimeOfCheckOut" runat="server" CssClass="readonlytextbox" Width="50px" />
                        <telerik:RadLabel ID="lblhrs" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNumberOfNights" runat="server" Text="Number Of Nights"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNoOfNights" runat="server" Style="text-align: Right" CssClass="readonlytextbox" Width="100px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRoomType" runat="server" Text="Room Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRoomType" runat="server" CssClass="readonlytextbox" Width="80%" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNumberofBeds" runat="server" Text="Number of Beds"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNoOfBeds" runat="server" CssClass="readonlytextbox" Style="text-align: Right" ReadOnly="true" Width="50%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblExtraBeds" runat="server" Text="Extra Beds"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MaskNumber ID="txtExtraBeds" runat="server" CssClass="readonlytextbox" IsPositive="true"
                            ReadOnly="true" IsInteger="true" MaxLength="1" DefaultZero="true" Width="50%" />
                    </td>
                      <td>
                        <telerik:RadLabel ID="lblRoomsRequest" runat="server" Text="Rooms Request"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNoOfRooms" runat="server" CssClass="readonlytextbox" Width="60%" Style="text-align: Right" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>                  
                    <td>
                        <telerik:RadLabel ID="lblDayUse" runat="server" Text="Day Use"></telerik:RadLabel>
                    </td>
                    <td colspan="7">
                        <telerik:RadTextBox ID="txtDayUseYN" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="11%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuAgentList" runat="server" OnTabStripCommand="MenuAgentList_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvHBAgent" runat="server" AllowCustomPaging="true" OnItemDataBound="gvHBAgent_ItemDataBound" Height="30%"
                AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" EnableViewState="false" Font-Size="11px" EnableHeaderContextMenu="true"
                OnItemCommand="gvHBAgent_ItemCommand" ShowHeader="true" OnNeedDataSource="gvHBAgent_NeedDataSource" OnSortCommand="gvHBAgent_SortCommand" OnDeleteCommand="gvHBAgent_DeleteCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" DataKeyNames="FLDQUOTEID" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
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
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Tick" AllowSorting="false" HeaderStyle-Width="3%">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkSelect" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDSENTDATE").ToString() != ""  ? true : false%>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="" AllowSorting="false" HeaderStyle-Width="3%">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>                                
                                <asp:LinkButton runat="server" ID="imgFlag" ToolTip="Confirmed" Enabled="false" Width="15px" Height="15px" Visible="false">
                                 <span class="icon" style="color:green;" ><i class="fas fa-star-blue"></i></span>      
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuoteId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblHotelId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOTELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblHotelCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOTELCODE") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkHotelName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOTELNAME") %>' CommandName="SELECT"
                                    CommandArgument='<%# Container.DataSetIndex %>' OnCommand="onHotelBookingQuotation"></asp:LinkButton>
                                <telerik:RadLabel ID="lblDtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Charges" AllowSorting="false">
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Discount" AllowSorting="false">
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDiscountAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNTAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Tax" AllowSorting="false">
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalTax" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALTAXAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Amount" AllowSorting="false">
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFinalTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINALTOTALAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Quoted" AllowSorting="false">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuoteStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTESTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApprovedYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALSTATUSYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblApprovalStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALSTATUS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblConfirmedYN" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOTELCONFIRMEDYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblConfirmedStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIRMEDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="cmdApprove" ToolTip="Approve" CommandName="APPROVE" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdDeApprove" CommandName="DEAPPROVE" ToolTip="Revoke approval" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-undo-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdConfirm" ToolTip="Confirm" CommandName="CONFIRM" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-file-invoice-dollar"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="height: 50%; width: 100%" frameborder="0"></iframe>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
