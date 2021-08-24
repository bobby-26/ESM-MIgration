<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelRequestGeneral.aspx.cs"
    Inherits="CrewTravelRequestGeneral" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
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
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Travel Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmtravelrequest" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
            <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="Menuapprove" runat="server" OnTabStripCommand="Menuapprove_TabStripCommand"></eluc:TabStrip>       
            <table cellspacing="5" cellpadding="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text=" Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="80%">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory"
                            OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID"
                            EnableLoadOnDemand="True" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCrewChangePort" runat="server">Crew Change Port</telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ucport" runat="server" CssClass="input_mandatory"  AppendDataBoundItems="true" Width="300px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofCrewChange" runat="server">Crew Change Date</telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateOfCrewChange" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
            </table>
            <hr />
            <b>&nbsp;<telerik:RadLabel ID="lblTravelPlan" runat="server" Text="Passenger List"></telerik:RadLabel>
            </b>
            <eluc:TabStrip ID="Menutravel" runat="server" OnTabStripCommand="MenuTravel_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCCT" runat="server" AllowCustomPaging="true" AllowSorting="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCCT_ItemCommand" OnNeedDataSource="gvCCT_NeedDataSource"
                OnItemDataBound="gvCCT_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    CommandItemDisplay="Top" DataKeyNames="FLDREQUESTID">
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
                            <ItemStyle HorizontalAlign="Center" Width="32px" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSnoHeader" runat="server">S.No.</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDROWNUMBER")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No." HeaderStyle-Width="10px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="55px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblfilenoHeader" runat="server">File No.</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDFILENO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="120px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblNameHeader" runat="server">Name/NOK of Employee</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                    CommandName="SELECT"></asp:LinkButton>
                                /
                                    <br />
                                <asp:LinkButton ID="lnkFName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFNAME") %>'
                                    CommandName="SELECT"></asp:LinkButton>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmpId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelRequestId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblOfficeTravel" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEREQUESTYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblTravelReqNo" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDOB" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDATEOFBIRTH", "{0:dd/MM/yyyy}")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblppno" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPASSPORTNO")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblothervisadet" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDOTHERVISADETAILS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server">Vessel</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblVesselName" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90px"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRankHeader" runat="server">Rank</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                                <telerik:RadLabel runat="server" ID="lblRankId" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblRankIdEdit" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblRankName" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblOfficeTravelYN" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEREQUESTYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblFamilyTravelYN" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYTRAVELYN") %>'></telerik:RadLabel>
                                <eluc:Rank ID="ddlRankEdit" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="80px"
                                    RankList="<%#PhoenixRegistersRank.ListRank() %>" SelectedRank='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="On/Off-Signer" HeaderStyle-Width="120px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOnOffsignerHeader" runat="server">On/Off-Signer</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDONSIGNERYESNO") %>
                                <telerik:RadLabel runat="server" ID="lblonoffsignerid" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Mode"  HeaderStyle-Width="120px">
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPaymentModeHeader" runat="server">Payment</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaymentmode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPAYMENTMODENAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Hard ID="ucPaymentmode" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="80px"
                                    HardList="<%# PhoenixRegistersHard.ListHard(1,185) %>" HardTypeCode="185" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTMODE") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Origin"  HeaderStyle-Width="160px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOrigHeader" runat="server">Origin</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDORIGINNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListAirportEdit">
                                    <telerik:RadLabel ID="lbloriginname" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDORIGINNAME")%>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtAirportNameEdit" runat="server" Width="75%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'
                                        Enabled="False" CssClass="input_mandatory">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowAirportEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" ImageAlign="Top" Text=".." CommandName="BUDGETCODE"                                         
                                        OnClientClick="return showPickList('spnPickListAirportEdit', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtoriginIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>' 
                                        Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination"  HeaderStyle-Width="160px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDestHeader" runat="server">Destination</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDDESTINATIONNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDesname" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDESTINATIONNAME")%>'></telerik:RadLabel>
                                <span id="spnPickListAirportdestinationedit">
                                    <telerik:RadTextBox ID="txtDestinsationNameedit" runat="server" Width="75%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'
                                        Enabled="False" CssClass="input_mandatory">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowDestinationedit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                        OnClientClick="return showPickList('spnPickListAirportdestinationedit', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtDestinationIdedit" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure Date"  HeaderStyle-Width="190px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDepDateHeader" runat="server">Departure</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldepartureampm" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDepdate" runat="server" Visible="false" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldepartureampmedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREAMPM") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTravelRequestIdedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <eluc:Date runat="server" ID="txtDepartureDate" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE", "{0:dd/MM/yyyy}")%>'
                                    Width="120px"></eluc:Date>
                                <telerik:RadComboBox ID="ddlampmdeparture" runat="server" CssClass="dropdown_mandatory" Width="55px"
                                    Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True" EmptyMessage="Type to select">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival Date"  HeaderStyle-Width="190px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" ></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblArrDate" runat="server">Arrival</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>                                
                                <telerik:RadLabel ID="lblarrivalampm" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblArrdate" runat="server" Visible="false" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblarrivalampmedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALAMPM") %>'></telerik:RadLabel>
                                <eluc:Date runat="server" ID="txtArrivalDate" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE")%>'
                                    Width="120px"></eluc:Date>
                                <telerik:RadComboBox ID="ddlampmarrival" runat="server" CssClass="input"  Width="55px"
                                    EnableLoadOnDemand="True"  EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                    <Items>                                        
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Travel Status" HeaderStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblTravelStstus" runat="server">Status</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltravelstatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDISCANCELLEDYNSTATUS")%>'></telerik:RadLabel>
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
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel Travel" ToolTip="Cancel Travel" Width="20PX" Height="20PX"
                                    CommandName="CANCELTRAVEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancelTravel">
                                <span class="icon"><i class="fas fa-times-circle"></i></span>
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
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true"
                        ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />
            <b>&nbsp;<telerik:RadLabel ID="lblBreakJourneyDetails" runat="server" Text="Break Journey Details"></telerik:RadLabel>
            </b>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCTBreakUp" runat="server" AllowCustomPaging="true" AllowSorting="true" 
                CellSpacing="0" GridLines="None" OnItemCommand="gvCTBreakUp_ItemCommand" OnNeedDataSource="gvCTBreakUp_NeedDataSource" ShowFooter="true"
                OnItemDataBound="gvCTBreakUp_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="false">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left">                  
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
                        <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="45px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblSnoHeader" runat="server" Text="S.No."></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblTravelReqNo" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></telerik:RadLabel>
                                <%#DataBinder.Eval(Container, "DataItem.FLDSERIALNO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Origin" HeaderStyle-Width="125px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblOrigHeader" runat="server">Origin</telerik:RadLabel>
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
                                        OnClientClick="return showPickList('spnPickListOriginOldbreakup', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
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
                                        OnClientClick="return showPickList('spnPickListOriginbreakupAdd', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtOriginIdBreakupAdd" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINID") %>'>
                                    </telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Destination" HeaderStyle-Width="125px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDestHeader" runat="server">Destination</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDDESTINATIONNAME")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListDestinationOldbreakup">
                                    <telerik:RadTextBox ID="txtDestinationNameOldBreakup" runat="server" Width="80%" Enabled="False"
                                        CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowDestinationOldbreakup" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="Top" Text=".." CommandName="BUDGETCODE"
                                        OnClientClick="return showPickList('spnPickListDestinationOldbreakup', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
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
                                        OnClientClick="return showPickList('spnPickListDestinationbreakupAdd', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " />
                                    <telerik:RadTextBox ID="txtDestinationIdBreakupAdd" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONID") %>'>
                                    </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Departure Date" HeaderStyle-Width="220px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblDepDateHeader" runat="server">Departure</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDEPARTUREDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="LBLDEPARTUREAMPM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtDepartureDateOld" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREDATE","{0:dd/MM/yyyy}") %>'></eluc:Date>
                                <telerik:RadComboBox ID="ddldepartureampmold" runat="server" CssClass="dropdown_mandatory" Width="55px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtDepartureDateAdd" CssClass="input_mandatory"></eluc:Date>
                                <telerik:RadComboBox ID="ddldepartureampmAdd" runat="server" CssClass="dropdown_mandatory" Width="55px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Arrival Date" HeaderStyle-Width="220px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblArrDateHeader" runat="server">Arrival</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE", "{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblARRIBALAMPM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALAMPM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtArrivalDate" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}") %>'></eluc:Date>
                                <telerik:RadComboBox ID="ddlarrivalampm" runat="server" CssClass="input" Width="55px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--" Value="DUMMY" />
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtArrivalDateAdd" CssClass="input" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:dd/MM/yyyy}") %>'></eluc:Date>
                                <telerik:RadComboBox ID="ddlarrivalampmAdd" runat="server" CssClass="input" Width="55px">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--" Value="DUMMY" />
                                        <telerik:RadComboBoxItem Text="AM" Value="1" />
                                        <telerik:RadComboBoxItem Text="PM" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose" HeaderStyle-Width="150px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPurposeHeader" runat="server">Purpose</telerik:RadLabel>
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
                        <telerik:GridTemplateColumn HeaderText="Class" HeaderStyle-Width="150px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblClassHeader" runat="server">Class</telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblClass" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRAVELCLASSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblClass" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRAVELCLASS") %>'></telerik:RadLabel>
                                <eluc:Hard ID="ucClassOld" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    HardList="<%# PhoenixRegistersHard.ListHard(1,227) %>" HardTypeCode="227" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELCLASS") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Hard ID="ucClassAdd" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    HardList="<%# PhoenixRegistersHard.ListHard(1,227) %>" HardTypeCode="227" SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELCLASS") %>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save Break Journey" Width="20PX" Height="20PX">
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
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
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
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true"
                         ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnApprove_Click" OKText="Yes"
                CancelText="No" Visible="false" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
