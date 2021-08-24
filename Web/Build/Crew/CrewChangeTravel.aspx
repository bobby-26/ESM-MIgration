<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewChangeTravel.aspx.cs"
    Inherits="CrewChangeTravel" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Airport" Src="~/UserControls/UserControlAirport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Change Travel</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewChangeTravel" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
            <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand"
                TabStrip="true" Title="Travel Plan"></eluc:TabStrip>
            <table cellpadding="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="300px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory"
                            Width="300px" OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID"
                            EnableLoadOnDemand="True" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCrewChangePort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ucport" runat="server" CssClass="input_mandatory" Width="300px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCrewChangeReason" runat="server" Text="Crew Change Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:TravelReason ID="ucCrewChangeReason" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" ReasonFor="1" Width="300px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofCrewChange" runat="server" Text="Crew Change"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateOfCrewChange" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
            </table>
            <hr />
            <b>
                <telerik:RadLabel ID="ltRecentTravel" runat="server" Text="Recent Travel Request"></telerik:RadLabel>
            </b>
            <div>
                <table cellpadding="1">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="ltExisting" runat="server" Text="Existing Travel"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlTravelRequest" runat="server" CssClass="dropdown_mandatory"
                                Width="240px" OnDataBound="ddlTravelRequest_DataBound" DataTextField="FLDREQUISITIONNO" DataValueField="FLDTRAVELID"
                                AutoPostBack="false" EnableLoadOnDemand="True" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                </table>
            </div>
            <eluc:TabStrip ID="MenuRecentTravel" runat="server" OnTabStripCommand="TravelRecent_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRecentTravel" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvRecentTravel_ItemCommand" OnNeedDataSource="gvRecentTravel_NeedDataSource" 
                OnItemDataBound="gvRecentTravel_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
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
                        <telerik:GridTemplateColumn HeaderStyle-Width="50px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSNo" runat="server" Text="S.No."></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDROWNUMBER")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="150px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblReqNo" runat="server" Text="Request No."></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%-- <asp:LinkButton ID="lnkReqNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></asp:LinkButton>--%>
                                <telerik:RadLabel ID="lnkReqNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No."></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkFileNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblFamilyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblCrewPlanId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblOnSignerYN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusID" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTRAVELSTATUS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="130px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text="Passenger"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Family Member" HeaderStyle-Width="130px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFName" runat="server" Text="NOK of Employee"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkFName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="50px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="On/Off-Signer" HeaderStyle-Width="130px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOnOffSigner" runat="server" Text="On/Off-Signer"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYESNO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Origin">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOrig" runat="server" Text="Origin"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="ltOriginName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDORIGIN")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDest" runat="server" Text="Destination"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="ltDestinationName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDESTINATION")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure Date" HeaderStyle-Width="150px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDepDate" runat="server" Text="Departure"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE", "{0:dd/MM/yyyy}")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldepartureampm" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival Date" HeaderStyle-Width="150px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblArrDate" runat="server" Text="Arrival"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE", "{0:dd/MM/yyyy}")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblarrivalampm" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Mode">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPaymentMode" runat="server" Text="Payment"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaymentmode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPAYMENTMODENAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" Screen='<%# "Crew/CrewTravelMoreInfoList.aspx?empId=" + DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID").ToString() +"&familyId=" + DataBinder.Eval(Container,"DataItem.FLDFAMILYID").ToString()%>' />
                                <asp:LinkButton runat="server" AlternateText="Cancel Batch"
                                    CommandName="CANCELTRAVEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancelTravel"
                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" 
                        ScrollHeight=""/>
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />
            <b>
                <telerik:RadLabel ID="lblTravelPlan" runat="server" Text="Travel Plan"></telerik:RadLabel>
            </b>

            <eluc:TabStrip ID="MenuBreakUpAssign" runat="server" OnTabStripCommand="BreakUpAssign_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCCT" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCCT_ItemCommand" OnNeedDataSource="gvCCT_NeedDataSource"
                OnItemDataBound="gvCCT_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true"
                    ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" AutoGenerateColumns="false" TableLayout="Fixed"
                    ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
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
                        <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="50px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDROWNUMBER")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No."></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkFileNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblFamilyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblCrewPlanId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblOnSignerYN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblCrewChangePort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWCHANGEPORT") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDateOfCrewChange" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFCREWCHANGE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblTravelReqNo" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text="Passenger"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                    CommandName="SELECT"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Employee">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblFName" runat="server" Text="NOK of Employee"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkFName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFNAME") %>'
                                    CommandName="SELECT"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="On/Off-Signer">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOnOffSigner" runat="server" Text="On/Off-Signer"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYESNO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Origin">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOrig" runat="server" Text="Origin"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="ltOriginName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDORIGINNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListAirportEdit">
                                    <telerik:RadTextBox ID="txtAirportNameEdit" runat="server" Width="80%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'
                                        Enabled="False" CssClass="input_mandatory">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowAirportEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                        OnClientClick="return showPickList('spnPickListAirportEdit', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtoriginIdEdit" runat="server" Width="0px" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>'></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDest" runat="server" Text="Destination"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="ltDestinationName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDESTINATIONNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListAirportdestinationedit">
                                    <telerik:RadTextBox ID="txtDestinsationNameedit" runat="server" Width="80%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'
                                        Enabled="False" CssClass="input_mandatory">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowDestinationedit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                        OnClientClick="return showPickList('spnPickListAirportdestinationedit', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtDestinationIdedit" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure Date" HeaderStyle-Width="200px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDepDate" runat="server" Text="Departure"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE", "{0:dd/MM/yyyy}")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldepartureampm" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtDepartureDate" Width="120px" CssClass="input_mandatory"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE","{0:dd/MM/yyyy}")%>'></eluc:Date>
                                <telerik:RadComboBox ID="ddlampmdeparture" runat="server" CssClass="dropdown_mandatory" Height="100%" Width="50px"
                                    EnableLoadOnDemand="True" Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival Date" HeaderStyle-Width="200px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblArrDate" runat="server" Text="Arrival"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE", "{0:dd/MM/yyyy}")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblarrivalampm" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALAMPM") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISEDIT")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtArrivalDate" Width="120px" CssClass="input_mandatory"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}")%>'></eluc:Date>
                                <telerik:RadComboBox ID="ddlampmarrival" runat="server" CssClass="dropdown_mandatory" Height="100%" Width="50px"
                                    EnableLoadOnDemand="True"  Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Mode" HeaderStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPaymentMode" runat="server" Text="Payment"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaymentmode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPAYMENTMODENAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ucPaymentmode" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="80px"
                                    HardList="<%# PhoenixRegistersHard.ListHard(1,185) %>" HardTypeCode="185" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTMODE") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="150px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="cmdEdit" runat="server" AlternateText="Edit" CommandName="EDIT"
                                    CommandArgument='<%# Container.DataSetIndex %>' ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="cmdDelete" runat="server" AlternateText="Delete" CommandName="DELETE"
                                    CommandArgument="<%# Container.DataSetIndex %>" ToolTip="Delete" Width="20PX" Height="20PX">                                    
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="cmdRequest" runat="server" AlternateText="Delete" CommandName="TRAVELREQUEST"
                                    CommandArgument="<%# Container.DataSetIndex %>" ToolTip="Existing Travel Request" Width="20PX" Height="20PX">                                    
                                <span class="icon"><i class="fas fa-plane"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="cmdNewRequest" runat="server" AlternateText="Delete" CommandName="NEWTRAVELREQUEST"
                                    CommandArgument="<%# Container.DataSetIndex %>" ToolTip="New Travel Request" Width="20PX" Height="20PX">                                    
                                <span class="icon"><i class="fas fa-plane-departure"></i></span>
                                </asp:LinkButton>
                                <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" Screen='<%# "Crew/CrewTravelMoreInfoList.aspx?empId=" + DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID").ToString() +"&familyId=" + DataBinder.Eval(Container,"DataItem.FLDFAMILYID").ToString()%>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" 
                        ScrollHeight=""/>
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />
            <br />
            <b>
                <telerik:RadLabel ID="lblBreakJourneyDetails" runat="server" Text="Break Journey Details"></telerik:RadLabel>
            </b>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCTBreakUp" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCTBreakUp_ItemCommand" OnNeedDataSource="gvCTBreakUp_NeedDataSource" ShowFooter="true"
                OnItemDataBound="gvCTBreakUp_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
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
                        <telerik:GridTemplateColumn HeaderText="S.No.">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSNo" runat="server" Text="S.No"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblTravelReqNo" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></telerik:RadLabel>
                                <%#DataBinder.Eval(Container, "DataItem.FLDSERIALNO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Origin">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOrig" runat="server" Text="Origin"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblOnSignerYN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblBreakUpId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBREAKUPID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISEDIT")%>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <%#DataBinder.Eval(Container, "DataItem.FLDORIGINNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelRequestIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblOnSignerYNEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblBreakUpIdEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBREAKUPID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <span id="spnPickListOriginOldbreakup">
                                    <telerik:RadTextBox ID="txtOriginNameOldBreakup" runat="server" Width="80%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowOriginoldbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                        OnClientClick="return showPickList('spnPickListOriginOldbreakup', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtOriginIdOldBreakup" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListOriginbreakupAdd">
                                    <telerik:RadTextBox ID="txtOriginNameBreakupAdd" runat="server" Width="80%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowOriginbreakupAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                        OnClientClick="return showPickList('spnPickListOriginbreakupAdd', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtOriginIdBreakupAdd" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDest" runat="server" Text="Destination"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDDESTINATIONNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListDestinationOldbreakup">
                                    <telerik:RadTextBox ID="txtDestinationNameOldBreakup" runat="server" Width="80%" Enabled="False"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>' CssClass="input_mandatory">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowDestinationOldbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                        OnClientClick="return showPickList('spnPickListDestinationOldbreakup', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtDestinationIdOldBreakup" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListDestinationbreakupAdd">
                                    <telerik:RadTextBox ID="txtDestinationNameBreakupAdd" runat="server" Width="80%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowDestinationbreakupAdd" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                        OnClientClick="return showPickList('spnPickListDestinationbreakupAdd', 'codehelp1', '', 'Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtDestinationIdBreakupAdd" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'>
                                    </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure Date" HeaderStyle-Width="180px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDepDate" runat="server" Text="Departure"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDEPARTUREDATE", "{0:dd/MM/yyyy}")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="LBLDEPARTUREAMPM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtDepartureDateOld" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE") %>'></eluc:Date>
                                <telerik:RadComboBox ID="ddldepartureampmold" runat="server" CssClass="dropdown_mandatory" Height="100%" Width="50px" 
                                    EnableLoadOnDemand="True"  Filter="Contains" MarkFirstMatch="true" >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtDepartureDateAdd" CssClass="input_mandatory"></eluc:Date>
                                <telerik:RadComboBox ID="ddldepartureampmAdd" runat="server" CssClass="dropdown_mandatory" Height="100%" Width="50px"
                                    EnableLoadOnDemand="True"  EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival Date" HeaderStyle-Width="180px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblArrDate" runat="server" Text="Arrival"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblARRIBALAMPM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtArrivalDate" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}") %>'></eluc:Date>
                                <telerik:RadComboBox ID="ddlarrivalampm" runat="server" CssClass="dropdown_mandatory" Height="100%" Width="50px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtArrivalDateAdd" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}") %>'></eluc:Date>
                                <telerik:RadComboBox ID="ddlarrivalampmAdd" runat="server" CssClass="dropdown_mandatory" Height="100%" Width="50px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose" HeaderStyle-Width="150px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPurpose" runat="server" Text="Purpose"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblPurpose" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPURPOSENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:TravelReason ID="ucPurposeOld" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    ReasonList="<%# PhoenixCrewTravelRequest.ListTravelReason(null) %>" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:TravelReason ID="ucPurposeAdd" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    ReasonList="<%# PhoenixCrewTravelRequest.ListTravelReason(null) %>" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="cmdEdit" AlternateText="Edit" CommandName="EDIT"
                                    CommandArgument='<%# Container.DataSetIndex %>' ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdDelete" AlternateText="Delete" CommandName="DELETE"
                                    CommandArgument="<%# Container.DataSetIndex %>" ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add"
                                    CommandName="BREAKUPADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                    ToolTip="Add New" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status ID="ucStatus" runat="server" />
            <%--  <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnApprove_Click" OKText="Yes"
                        CancelText="No" />--%>
            <br />
            <b>
                <telerik:RadLabel ID="ltCopyBreakup" runat="server" Text="Copy Break Journey"></telerik:RadLabel>
            </b>

            <eluc:TabStrip ID="MenuBreakJourney" runat="server" OnTabStripCommand="BreakUpCopy_TabStripCommand" Visible="false"></eluc:TabStrip>

            <table cellpadding="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="ltSeafarer" runat="server" Text="Copy To"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSeafarerBreakup" runat="server" CssClass="dropdown_mandatory"
                            Width="240px" OnDataBound="ddlSeafarerBreakup_DataBound" DataTextField="FLDNAME"
                            AutoPostBack="false" OnTextChanged="ddlseafarerBreakup" DataValueField="FLDREQUESTID"
                            EnableLoadOnDemand="True"  Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:LinkButton runat="server" AlternateText="Copy Breakup" Width="20PX" Height="20PX" 
                            CommandName="copy" ID="lnkTravelBreakUpCopy" ToolTip="Copy Breakup" OnClick="lnkTravelBreakUpCopy_Click">
                                <span class="icon"><i class="far fa-copy"></i></span>
                        </asp:LinkButton>
                        <%--<div class="navSelect" style="position: relative; width: 15px">--%>
                        <%-- <eluc:TabStrip ID="MenuTravelBreakUpCopy" runat="server" OnTabStripCommand="TravelBreakUpCopy_TabStripCommand"></eluc:TabStrip>--%>
                        <%--</div>--%>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
