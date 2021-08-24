<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelRequestforoffice.aspx.cs"
    Inherits="CrewTravelRequestforoffice" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Office Travel Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewChangeTravel" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <%--<eluc:TabStrip ID="menutab" runat="server" Title="" TabStrip="true"></eluc:TabStrip>--%>
            <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="height: 50%; width: 100%" frameborder="0"></iframe>
            <eluc:TabStrip ID="MenutravelList" runat="server" OnTabStripCommand="MenutravelList_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCCT" runat="server" AutoGenerateColumns="False" EnableViewState="false" Height="40%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="true"
                OnNeedDataSource="gvCCT_NeedDataSource" EnableHeaderContextMenu="true" OnItemCommand="gvCCT_ItemCommand"
                OnItemDataBound="gvCCT_ItemDataBound" ShowFooter="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" DataKeyNames="FLDTRAVELID" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
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
                        <telerik:GridTemplateColumn HeaderText="" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="3%">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="imgFlag" Enabled="false" Width="15px" Height="15px" Visible="false">
                                 <span class="icon" id="imgFlagcolor" style="color:orange;" ><i class="fas fa-star"></i></span>      
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Request No." AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkReqNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'
                                    CommandName="SELECT" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblPassengerFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSENGERFROM") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" ShowSortIcon="true">
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
                        <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequestedDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDDATEOFCREWCHANGE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurpose" runat="server"  ClientIDMode="AutoID" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPURPOSE")%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipCourse" runat="server" TargetControlId="lblPurpose" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSENAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose Desc" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurposeDesc" runat="server" CssClass="tooltip" ClientIDMode="AutoID" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPURPOSEREMARKSTOOLTIP")%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipPurposeDesc" runat="server" TargetControlId="lblPurposeDesc" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSEREMARKS") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passenger From" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassengers" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPASSENGERS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Approved" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="7%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApproved" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTAPPROVED")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblApprovedYN" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTAPPROVE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="9%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTravelPurpose" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELTYPE")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELSTATUS")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblstatus" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOfficeTrvApproval" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDOFFICETRAVELREQAPPROVAL") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblattachmentyn" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="" HeaderStyle-Width="9%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Approve" ID="cmdApprove" CommandName="APPROVE"
                                    ToolTip="Approve" Width="20px" Height="20px" Visible="false">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="Attachment" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAttachment"
                                    ToolTip="Attachment" Width="20PX" Height="20PX">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Itinerary" ToolTip="Itinerary" Width="20PX" Height="20PX"
                                    CommandArgument='<%# Container.DataSetIndex %>' ID="cmdShowTicket">
                                <span class="icon"><i class="far fa-list-alt-Itinerary"></i></span>
                                </asp:LinkButton>
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
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
