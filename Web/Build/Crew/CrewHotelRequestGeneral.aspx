<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHotelRequestGeneral.aspx.cs"
    Inherits="CrewHotelRequestGeneral" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="HotelRoom" Src="~/UserControls/UserControlHotelRoom.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hotel Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmHotelRequestGeneral" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:TabStrip ID="MenuHotelRequest" runat="server" OnTabStripCommand="MenuHotelRequest_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="btnApprove_Click" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuRequest" runat="server" OnTabStripCommand="MenuRequest_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRequisitionNo" runat="server" Text="Reference No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReqNo" runat="server" Width="40%" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="btnTooltipHelp" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                            Text="Exclude the '0' before the 'Area Code'. (Eg. For Chennai:44 22222222)">
                        </telerik:RadToolTip>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ucVessel" Width="40%" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            Entitytype="VSL" ActiveVessels="true" AssignedVessels="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCity" runat="server" Text="City"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MUCCity ID="ucCity" runat="server" CssClass="dropdown_mandatory" Width="40%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPurpose" runat="server" Text="Purpose"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:TravelReason ID="ucPurpose" Width="40%" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            ReasonFor="1" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblGuestFrom" runat="server" Text="Guest From"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlGuestFrom" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="40%"
                            Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="1" Text="Travel" />
                                <telerik:RadComboBoxItem Value="2" Text="Seafarers" />
                                <telerik:RadComboBoxItem Value="3" Text="Office" />
                                <telerik:RadComboBoxItem Value="4" Text="Others" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <br />

            <div id="RequestDetail" runat="server">
                <telerik:RadLabel ID="lblRequisitionGuestDetails" runat="server" Font-Bold="true" Text="Requisition Guest Details"></telerik:RadLabel>
                <eluc:TabStrip ID="MenuGuests" runat="server" OnTabStripCommand="MenuGuests_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvGuest" runat="server" EnableViewState="false"
                    AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                    OnNeedDataSource="gvGuest_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvGuest_ItemDataBound"
                    OnItemCommand="gvGuest_ItemCommand" OnDeleteCommand="gvGuest_DeleteCommand" ShowFooter="false" AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
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
                            <telerik:GridTemplateColumn HeaderText="Guest Name" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblGuestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGUESTID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Passport No." AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPassportNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" ShowSortIcon="true">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblstatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGUESTSTATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDITGUESTS" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
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
            </div>
            <br />
            <div id="PanelDetail" runat="server">
                <telerik:RadLabel ID="lblRoomDetails" runat="server" Font-Bold="true" Text="Room Details"></telerik:RadLabel>
                <table width="100%" cellpadding="1" cellspacing="2">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRoomType" runat="server" Text="Room Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:HotelRoom ID="ucHotelRoom" runat="server" CssClass="dropdown_mandatory" OnTextChangedEvent="ucHotelRoom_TextChangedEvent"
                                AutoPostBack="true"></eluc:HotelRoom>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblNumberofBeds" runat="server" Text="Number of Beds"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtNoOfBeds" runat="server" CssClass="readonlytextbox" Style="text-align: Right" ReadOnly="true" Width="68%"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblExtraBeds" runat="server" Text="Extra Beds"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtExtraBeds" runat="server" ispositive="true" Width="88%"
                                isinteger="true" MaxLength="1" defaultzero="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNumberofRooms" runat="server" Text="Number of Rooms"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtNoOfRooms" runat="server" CssClass="readonlytextbox" Style="text-align: Right" ReadOnly="true" Width="50%"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCheckinDate" runat="server" Text="Checkin Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtCheckinDate" runat="server" CssClass="input_mandatory" DatePicker="true"
                                AutoPostBack="true"></eluc:Date>
                            <telerik:RadMaskedTextBox runat="server" ID="txtTimeOfCheckIn" CssClass="input_mandatory" Width="50px" Mask="##:##"></telerik:RadMaskedTextBox>
                            <telerik:RadLabel ID="lblhrs" runat="server" Text="(hrs)"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCheckoutDate" runat="server" Text="Checkout Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtCheckoutDate" runat="server" CssClass="input_mandatory" DatePicker="true"
                                AutoPostBack="true"></eluc:Date>
                            <telerik:RadMaskedTextBox runat="server" ID="txtTimeOfCheckOut" CssClass="input_mandatory" Width="50px" Mask="##:##"></telerik:RadMaskedTextBox>
                            <telerik:RadLabel ID="lblCheckOuthrs" runat="server" Text="(hrs)"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNumberOfNights" runat="server" Text="Number Of Nights"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtNoOfNights" runat="server" CssClass="input_mandatory" Width="50%" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDayUse" runat="server" Text="Day Use"></telerik:RadLabel>
                        </td>
                        <td>
                            <asp:CheckBox runat="server" ID="chkDayUse" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPayableBy" runat="server" Text="Payable By"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucPaymentmode" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                AutoPostBack="true" HardList="<%# PhoenixRegistersHard.ListHard(1,185) %>" HardTypeCode="185" Width="88%"
                                OnTextChangedEvent="ucPaymentmode_TextChangedEvent" />
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                        </td>
                        <td runat="server" id="tblBudgetCode">
                            <span id="spnPickListMainBudget">
                                <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="60px" Enabled="False"></telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="180px" Enabled="False"></telerik:RadTextBox>
                                <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top"
                                    ID="btnShowBudget" ToolTip="Select BudgetCode"
                                    OnClientClick="return showPickList('spnPickListMainBudget', 'codehelp1', '', 'Common/CommonPickListBudget.aspx', true); ">
                                        <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                                </asp:LinkButton>
                                <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="0px"></telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="0px"></telerik:RadTextBox>
                            </span>&nbsp;
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text="Owner budget Code"></telerik:RadLabel>
                        </td>
                        <td runat="server" id="tblOwnerBudgetCode">
                            <span id="spnPickListOwnerBudget">
                                <telerik:RadTextBox ID="txtOwnerBudgetCode" runat="server" Text="" Enabled="false" MaxLength="20"
                                    Width="246">
                                </telerik:RadTextBox>
                                <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top"
                                    ID="btnShowOwnerBudget" ToolTip="Select Owner Budget Code">                                 
                                        <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                                </asp:LinkButton>
                                <telerik:RadTextBox ID="txtOwnerBudgetName" runat="server" Width="0px"
                                    Enabled="False">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtOwnerBudgetId" runat="server" Width="0px" Text=""></telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtOwnerBudgetgroupId" runat="server" Width="0px"></telerik:RadTextBox>
                            </span>
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Panel runat="server" ID="pnlComanyPayableCharges" GroupingText="Comany Payable Charges">
                                <asp:CheckBoxList ID="cblComanyPayableCharges" runat="server" DataValueField="FLDHOTELCHARGESID"
                                    DataTextField="FLDCHARGINGNAME" RepeatDirection="Horizontal" RepeatColumns="5"
                                    Enabled="false">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
