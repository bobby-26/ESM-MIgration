<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelRequestList.aspx.cs" Inherits="CrewTravelRequestList" %>


<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Travel Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
        </script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCCT.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewChangeTravel" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" >
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            

            <eluc:TabStrip ID="MenutravelList" runat="server" OnTabStripCommand="MenutravelList_TabStripCommand"></eluc:TabStrip>
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
                            <td>&nbsp; By default travel request list will show only the pending requests.
                            </td>
                        </tr>

                    </table>
                </ContentTemplate>
            </telerik:RadWindow>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCCT" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCCT_ItemCommand" OnNeedDataSource="gvCCT_NeedDataSource" Height="90%"
                OnItemDataBound="gvCCT_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnDataBound="gvCCT_DataBound">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    DataKeyNames="FLDTRAVELID">
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
                        <telerik:GridTemplateColumn HeaderStyle-Width="30px">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="imgFlag" Enabled="false" Width="15px" Height="15px" Visible="false">
                                 <span class="icon" id="imgFlagcolor" style="color:orange;" ><i class="fas fa-star"></i></span>      
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Request No." HeaderStyle-Width="150px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRequestNoHeader" runat="server">Request No.</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkReqNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'
                                    CommandName="SELECT"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblVesselHeader" runat="server">Vessel</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblCrewChangePort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTOFCREWCHANGE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDateOfCrewChange" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFCREWCHANGE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblFamilyNok" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDateHeader" runat="server">Crew Change</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFCREWCHANGE", "{0:dd/MMM/yyyy}"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Crew Change Port">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCrewChangePortHeader" runat="server">Crew Change Port</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDPORTNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblpurposeHeader" runat="server">Purpose</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurpose" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPURPOSE")%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipCourse" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblstatusHeader" runat="server">Status</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTravelPurpose" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELTYPE")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELSTATUS")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblstatus" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Office/Crew Change">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOfficeHeader" runat="server">Office/Crew Change</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblofficetravel" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDOFFICETRAVEL") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOfficeTravelYN" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDOFFICEREQUESTYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblApprovedYN" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTAPPROVE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Requested">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRequestedDateHeader" runat="server">Requested</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequestedDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDREQUESTEDDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Approved Y/N">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblAppreovedYN" runat="server">Approved Y/N</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApproved" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREQUESTAPPROVED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="Attachment" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAtt"
                                    ToolTip="Attachment" Width="20PX" Height="20PX">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Travel Email" Visible="false"
                                    CommandName="PICEMAIL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPICEmail"
                                    ToolTip="Travel Email" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-envelope"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Itinerary" ToolTip="Itinerary" Width="20PX" Height="20PX"
                                    CommandArgument='<%# Container.DataSetIndex %>' ID="cmdShowTicket">
                                <span class="icon"><i class="far fa-list-alt-Itinerary"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Approve" ID="cmdApprove" CommandName="APPROVE"
                                    ToolTip="Approve" Width="20px" Height="20px" Visible="false">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel Travel"
                                    CommandName="CANCELTRAVEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancelTravel"
                                    ToolTip="Cancel Travel" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    
    <table cellpadding="1" cellspacing="1">
        <tr>
            <td>
                <span class="icon" style="color: green;"><i class="fas fa-star-blue"></i></span>
            </td>
            <td>
                <telerik:RadLabel ID="lblCompletedRequest" runat="server" Text="* Completed Request"></telerik:RadLabel>
            </td>
            <td>
                <span class="icon" style="color: orange;"><i class="fas fa-star-yellow"></i></span>
            </td>
            <td>
                <telerik:RadLabel ID="lblPendingRequest" runat="server" Text="* Pending Request"></telerik:RadLabel>
            </td>
            <td>
                <span class="icon" style="color: red;"><i class="fas fa-star-red"></i></span>
            </td>
            <td>
                <telerik:RadLabel ID="lblCancelledRequest" runat="server" Text="* Cancelled Request"></telerik:RadLabel>
            </td>
            <td>
                <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                        RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true" CssClass="fon"
                        HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                        Text="Note: <br/> By default travel request list will show only the pending requests.">
                    </telerik:RadToolTip>
            </td>
        </tr>
    </table>
    <eluc:Status ID="ucStatus" runat="server" />
    <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnApprove_Click" OKText="Yes"
        CancelText="No" />
    </form> 
</body>
</html>
