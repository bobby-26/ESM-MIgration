<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportsTravelStatus.aspx.cs"
    Inherits="CrewReportsTravelStatus" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Manager" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        
       <%-- <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvTravelStatus.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
            </script>--%>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTravelStatus" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
          
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                   
                            <eluc:TabStrip ID="MenuTravel" runat="server" OnTabStripCommand="MenuTravel_TabStripCommand"
                            TabStrip="false"></eluc:TabStrip>
                    
            <table width="100%">
                        <tr>
                            <td style="padding-left:10px;padding-right:15px">
                                <telerik:radlabel ForeColor="Black" ID="lblPool" runat="server" Text="Pool"></telerik:radlabel>
                            </td>
                            <td style="padding-right:15px">
                                <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" Width="240px" />
                            </td>
                            <td style="padding-right:15px">
                                <telerik:radlabel ForeColor="Black"  ID="lblRank" runat="server" Text="Rank"></telerik:radlabel>
                            </td>
                            <td style="padding-right:15px">
                                <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" Width="240px" />
                            </td>
                        </tr>
                        <tr>
                           <td style="padding-left:10px;padding-right:15px">
                                <telerik:radlabel  ForeColor="Black" ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:radlabel>
                            </td>

                            <td style="padding-right:15px">
                                <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" Width="240px" />
                            </td>
                            <td style="padding-right:15px">
                                <telerik:radlabel  ForeColor="Black" ID="lblVessel" runat="server" Text="Vessel"></telerik:radlabel>
                            </td>
                            <td style="padding-right:180px">
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" Width="240px" EntityType="VSL" AssignedVessels="true" ActiveVesselsOnly="true" />
                            </td>
                            <td></td>
                        </tr>
                      
                        <tr>
                            <td style="padding-left:10px;padding-right:15px">
                                <telerik:radlabel  ForeColor="Black" ID="lblPrincipal" runat="server" Text="Principal"></telerik:radlabel>
                            </td>
                            <td style="padding-right:15px">
                                <eluc:Principal ID="ucPrincipal" runat="server" AppendDataBoundItems="true" AddressType="128"
                                     Width="240px" />
                            </td>
                            <td style="padding-right:15px">
                                <telerik:radlabel  ForeColor="Black" ID="lblManager" runat="server" Text="Manager"></telerik:radlabel>
                            </td>
                           <td style="padding-right:15px">
                                <eluc:Principal ID="ucManager" runat="server" AppendDataBoundItems="true" AddressType="126"
                                     Width="240px"/>
                            </td>
                        </tr>
                      
                        <tr>
                            <td style="padding-left:10px;padding-right:15px">
                                <telerik:radlabel  ForeColor="Black" ID="lblPurpose" runat="server" Text="Purpose"></telerik:radlabel>
                            </td>
                           <td style="padding-right:15px">
                                <eluc:TravelReason ID="ucpurpose" runat="server" AppendDataBoundItems="true"  Width="240px" />
                            </td>
                           <td style="padding-right:15px">
                                <telerik:radlabel  ForeColor="Black" ID="lblDateofTravelBetween" runat="server" Text="Date of Travel Between"></telerik:radlabel>
                            </td>
                            <td style="padding-right:15px">
                                <eluc:Date ID="txtStartDate" CssClass="input_mandatory"  runat="server" />
                                &nbsp;to&nbsp;
                            <eluc:Date ID="txtEndDate" CssClass="input_mandatory" runat="server" />
                            </td>
                        </tr>
                        <tr>
                           <td style="padding-left:10px;padding-right:15px">
                                <telerik:radlabel  ForeColor="Black" ID="lblOrigin" runat="server" Text="Origin"></telerik:radlabel>
                            </td>
                            <td style="padding-right:15px">
                                <span id="spnPickListOriginCity">
                                    <telerik:radtextbox ID="txtOriginCity" runat="server" ReadOnly="false" CssClass="readonlytextbox"
                                        MaxLength="20" Width="235px"></telerik:radtextbox>
                                    <asp:LinkButton runat="server" id="ImgShowOrigin" OnClientClick="return showPickList('spnPickListOriginCity', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " >
                                        <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                                        </asp:LinkButton>
                                    <telerik:radtextbox ID="txtOriginId" runat="server" Width="0px"></telerik:radtextbox>
                                </span>
                            </td>
                            <td style="padding-right:15px">
                                <telerik:radlabel  ForeColor="Black" ID="lblOfficeCrewChange" runat="server" Text="Office/Crew Change"></telerik:radlabel>
                            </td>
                            <td style="padding-right:15px">
                                <telerik:RadComboBox ID="ddlofficetravelyn" runat="server" Width="240px"    Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select ">
                                    <Items>
                                    <telerik:RadComboBoxItem Text="Crew Change Travel" Value="1"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Office Travel" Value="2"     ></telerik:RadComboBoxItem>
                               </Items>
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                           <td style="padding-left:10px;padding-right:15px">
                                <telerik:radlabel  ForeColor="Black" ID="lblDestination" runat="server" Text="Destination"></telerik:radlabel>
                            </td>
                           <td style="padding-right:15px">
                                <span id="spnPickListDestinationCity">
                                    <telerik:radtextbox ID="txtDestinationCity" runat="server" ReadOnly="false" CssClass="readonlytextbox"
                                        MaxLength="20" Width="235px"></telerik:radtextbox>
                                  
                                      <asp:LinkButton runat="server" id="ImgShowDestination" OnClientClick="return showPickList('spnPickListDestinationCity', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " >
                                        <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                                        </asp:LinkButton>

                                      <telerik:radtextbox ID="txtDestinationId" runat="server" Width="0px"></telerik:radtextbox>
                                </span>
                            </td>
                            <td style="padding-right:15px">
                                <telerik:radlabel  ForeColor="Black" ID="lblStatus" runat="server" Text="Status"></telerik:radlabel>
                            </td>
                           <td style="padding-right:15px">
                                <telerik:RadComboBox ID="ddlTravelStatus" runat="server"  Width="240px"    Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select Status">
                                    <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value=""    ></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Ticket Issued" Value="3"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="ReIssued" Value="2"     ></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Text="Cancelled" Value="1"    ></telerik:RadComboBoxItem>
                                </Items>
                                        </telerik:RadComboBox>
                             </td>
                        </tr>
                        <tr>
                            <td style="padding-left:10px;padding-right:15px">
                                <telerik:radlabel  ForeColor="Black" ID="lblCrewChangesPort" runat="server" Text="Crew Change Port"></telerik:radlabel>
                            </td>
                            <td style="padding-right:15px">
                                <eluc:Port ID="ucport" runat="server" AppendDataBoundItems="true" Width="240px" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                    </table>
                    
                        <eluc:TabStrip ID="MenuTravelStatus" runat="server" OnTabStripCommand="MenuTravelStatus_TabStripCommand"
                            TabStrip="false"></eluc:TabStrip>
                     
                     <telerik:RadGrid ID="gvTravelStatus" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvTravelStatus_ItemCommand"
                OnItemDataBound= "gvTravelStatus_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvTravelStatus_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed"  DataKeyNames="FLDREQUESTID">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel  ForeColor="Black"  ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:gridtemplatecolumn HeaderText="Request No.">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:radlabel  ForeColor="Black" ID="lblRequestNoHeader" runat="server">Request NO</telerik:radlabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDREQUISITIONNO")%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="File No.">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:radlabel  ForeColor="Black" ID="lblFileNoHeader" runat="server">File No.</telerik:radlabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDEMPLOYEECODE")%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="Name">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:radlabel  ForeColor="Black" ID="lblNameHeader" runat="server">Name</telerik:radlabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDNAME")%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="Rank">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:radlabel  ForeColor="Black" ID="lblRankHeader" runat="server">Rank</telerik:radlabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="Batch">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:radlabel  ForeColor="Black" ID="lblBatchHeader" runat="server">Batch</telerik:radlabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDBATCH")%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="Vessel">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:radlabel  ForeColor="Black" ID="lblVesselHeader" runat="server">Vessel</telerik:radlabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:radlabel ForeColor="Black"  ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                            Visible="false"></telerik:radlabel>
                                        <telerik:radlabel  ForeColor="Black" ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:radlabel>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="Crew Change Port">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:radlabel ForeColor="Black"  ID="lblCrewChangePortHeader" runat="server">Crew Change Port</telerik:radlabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDPORTNAME")%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="Origin">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:radlabel  ForeColor="Black" ID="lblOrigHeader" runat="server">Orig</telerik:radlabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDORIGINNAME")%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="Destination">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:radlabel  ForeColor="Black" ID="lblDest" runat="server">Dest</telerik:radlabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDDESTINATIONNAME")%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="Departure Date">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:radlabel  ForeColor="Black" ID="lblDepDateHeader" runat="server">Dep Date</telerik:radlabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:radlabel  ForeColor="Black" ID="lblDepartureDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE","{0:HH}:{0:mm}").ToString() == "00:00" ? DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE", "{0:dd/MM/yyyy}") : DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE", "{0:dd/MM/yyyy HH:mm}").ToString()%>'></telerik:radlabel>
                                        <telerik:radlabel  ForeColor="Black" ID="lbldepartureampm" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTUREAMPM") %>'></telerik:radlabel>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="Arrival Date">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:radlabel  ForeColor="Black" ID="ArrDateHeader" runat="server">Arr Date</telerik:radlabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:radlabel ForeColor="Black"  ID="lblArrivalDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE","{0:HH}:{0:mm}").ToString() == "00:00" ? DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE", "{0:dd/MM/yyyy}") : DataBinder.Eval(Container, "DataItem.FLDARRIVALDATE", "{0:dd/MM/yyyy HH:mm}").ToString()%>'></telerik:radlabel>
                                        <telerik:radlabel ForeColor="Black"  ID="lblarrivalampm" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALAMPM") %>'></telerik:radlabel>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="Ticket No">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:radlabel  ForeColor="Black" ID="lblTicketHeader" runat="server">Ticket No</telerik:radlabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDTICKETNO")%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="Total Airfare">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:radlabel ForeColor="Black"  ID="lblTicketHeader" runat="server">Total Airfare</telerik:radlabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                                <telerik:gridtemplatecolumn HeaderText="Status">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <telerik:radlabel  ForeColor="Black" ID="lblStatusHeader" runat="server">Status</telerik:radlabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDSTATUS")%>
                                    </ItemTemplate>
                                </telerik:gridtemplatecolumn>
                           </Columns>

                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>