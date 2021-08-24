<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportTravelCancellationandRefund.aspx.cs"
    Inherits="CrewReportTravelCancellationandRefund" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddress" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommonCheckBoxList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuickList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  
     <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
       <%-- 
        <script type="text/javascript">
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
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
          
                         <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                   
                            <eluc:TabStrip ID="MenuTravel" runat="server" OnTabStripCommand="MenuTravel_TabStripCommand"
                            TabStrip="false"></eluc:TabStrip>
                   
                    <table width="100%" cellpadding="1">
                        <tr>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:radlabel ForeColor="Black"  ID="lblVessel" runat="server" Text="Vessel"></telerik:radlabel>
                            </td>
                            <td style="padding-left:10px">
                                <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true" Width="230px" EntityType="VSL" AssignedVessels="true" />
                            </td>
                            <td style="padding-left:10px">
                                <telerik:radlabel  ForeColor="Black" ID="lblOrigin" runat="server" Text="Origin"></telerik:radlabel>
                            </td>
                            <td style="padding-left:10px">
                                <span id="spnPickListOriginCity">
                                    <telerik:radtextbox ID="txtOriginCity" runat="server" ReadOnly="false" CssClass="readonlytextbox"
                                        MaxLength="20" Width="230px"></telerik:radtextbox>
                                    <asp:LinkButton runat="server" id="ImgShowOrigin" OnClientClick="return showPickList('spnPickListOriginCity', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " >
                                        <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                                        </asp:LinkButton>
                                    <telerik:radtextbox ID="txtOriginId" runat="server" CssClass="input hidden" Width="0px"></telerik:radtextbox>
                                </span>
                            </td>
                            <td style="padding-left:10px">
                                <telerik:radlabel  ForeColor="Black" ID="lblAgent" runat="server" Text="Agent"></telerik:radlabel>
                            </td>
                            <td style="padding-left:10px">
                                <span id="spnPickListAgent">
                                    <telerik:radtextbox ID="txtAgentName" runat="server" Width="60px" ></telerik:radtextbox>
                                    <telerik:radtextbox ID="txtAgentNumber" runat="server" Width="180px" ></telerik:radtextbox>
                                    <asp:LinkButton runat="server" id="cmdShowAgent" OnClientClick="return showPickList('spnPickListAgent', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=135&CalledFrom=travelinvoiceexception', true); " >
                                        <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                                        </asp:LinkButton>
                                    
                                     <telerik:radtextbox ID="txtAgentId" runat="server" CssClass="input hidden" Width="0px"></telerik:radtextbox>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:radlabel  ForeColor="Black" ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:radlabel>
                            </td>
                            <td style="padding-left:10px">
                                <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" Width="245px" />
                            </td>
                            <td style="padding-left:10px">
                                <telerik:radlabel ForeColor="Black"  ID="lblDestination" runat="server" Text="Destination"></telerik:radlabel>
                            </td>
                            <td style="padding-left:10px">
                                <span id="spnPickListDestinationCity">
                                    <telerik:radtextbox ID="txtDestinationCity" runat="server" ReadOnly="false" CssClass="readonlytextbox"
                                        MaxLength="20" Width="230px"></telerik:radtextbox>
                                    
                                    <asp:LinkButton runat="server" id="ImgShowDestination" OnClientClick="return showPickList('spnPickListDestinationCity', 'codehelp1', '', '../Common/CommonPickListCity.aspx', true); " >
                                        <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                                        </asp:LinkButton>
                                    
                                     <telerik:radtextbox ID="txtDestinationId" runat="server" CssClass="input hidden" Width="0px"></telerik:radtextbox>
                                </span>
                            </td>
                            <td style="padding-left:10px">
                                <telerik:radlabel ForeColor="Black"  ID="lblPrinciple" runat="server" Text="Principle"></telerik:radlabel>
                            </td>
                            <td style="padding-left:10px">
                                <eluc:MultiAddress runat="server" ID="ucPrincipal" AddressType='<%# ((int)SouthNests.Phoenix.Common.PhoenixAddressType.PRINCIPAL).ToString() %>'
                                    Width="230px"  />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:radlabel ForeColor="Black"  ID="lblPool" runat="server" Text="Pool"></telerik:radlabel>
                            </td>
                           <td style="padding-left:10px">
                                <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" Width="245px" />
                            </td>
                            <td style="padding-left:10px">
                                <telerik:radlabel ForeColor="Black"  ID="lbltravelstartdate" runat="server" Text="Date of Travel"></telerik:radlabel>
                            </td>
                            <td style="padding-left:10px">
                                <eluc:Date ID="txtStartDate"  runat="server" Width="115px" />
                                <telerik:radlabel ID="lblfrom" runat="server" Text="-"></telerik:radlabel>
                                <eluc:Date ID="txtEndDate" ForeColor="Black"  runat="server"  Width="115px" />
                            </td>
                            <td style="padding-left:5px">
                                <telerik:radlabel ForeColor="Black"  ID="lblinvoicedate" runat="server" Text="Invoice Date"></telerik:radlabel>
                            </td>
                            <td style="padding-left:10px">
                                <eluc:Date ID="txtInvoicefromdate"  runat="server" Width="115px" />
                                <telerik:radlabel ForeColor="Black"  ID="lblinvoicetodate" runat="server" Text="-"></telerik:radlabel>
                                <eluc:Date ID="txtInvoicetodate"  runat="server" Width="115px"  />
                            </td>
                                                      
                        </tr>
                        <tr>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:radlabel ForeColor="Black"  ID="lblRank" runat="server" Text="Rank"></telerik:radlabel>
                            </td>
                           <td style="padding-left:10px">
                                <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td style="padding-left:10px">
                                <telerik:radlabel  ForeColor="Black" ID="lblcancellation" runat="server" Text="Cancellation Reason"></telerik:radlabel>
                            </td>
                            <td style="padding-left:10px">
                                <eluc:Quick ID="lstCancelReason" runat="server" QuickTypeCode="110"
                                    QuickList='<%#PhoenixRegistersQuick.ListQuick(1,110)%>' AppendDataBoundItems="true"  Width="230px"/>
                            </td>
                            <td style="padding-left:5px">
                                <telerik:radlabel  ForeColor="Black" ID="lblPurposeoftravel" runat="server" Text="Purpose of Travel"></telerik:radlabel>
                            </td>
                            <td style="padding-left:10px">
                                <telerik:RadListBox ID="lstPurposeofTravel" DataTextField="FLDREASON" DataValueField="FLDTRAVELREASONID"
                                    SelectionMode="Multiple" runat="server" Width="230px" Height="80px" ></telerik:RadListBox>
                            </td>
                            
                        </tr>
                        <tr>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:radlabel  ForeColor="Black" ID="lblzone" runat="server" Text="Zone"></telerik:radlabel>
                            </td>
                           <td style="padding-left:10px">
                                <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" Width="245px" />
                            </td>
                            <td style="padding-left:10px">
                                <telerik:radlabel  ForeColor="Black" ID="lblname" runat="server" Text=" Passenger Name"></telerik:radlabel>
                            </td>
                           <td style="padding-left:10px">
                                <telerik:radtextbox ID="txtpasengername" runat="server" Width="240px" ></telerik:radtextbox>
                            </td>
                            <td style="padding-left:10px">
                                <telerik:radlabel  ForeColor="Black" ID="lblinvoicenumber" runat="server" Text="Invoice Number"></telerik:radlabel>
                            </td>
                            <td style="padding-left:10px">
                                <telerik:radtextbox ID="txtinvoicenumber" runat="server" Width="230px" ></telerik:radtextbox>
                            </td>
                           
                        </tr>
                        <tr>
                            <td style="padding-left:10px;padding-right:10px">
                                <telerik:radlabel ForeColor="Black"  ID="lblRefundCancel" runat="server" Text="Refund/Cancel"></telerik:radlabel>
                            </td>
                            <td style="padding-left:10px">
                                <telerik:RadComboBox ID="ddlRefundCancel" runat="server" Width="245px" >
                                    <Items>
                                    <telerik:RadComboBoxItem Text="All" Value=""></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem  Text="Refund" Value="1" />
                                    <telerik:RadComboBoxItem  Text="Cancel" Value="0" />
                                        </Items>
                                </telerik:RadComboBox>
                            </td>
                             <td style="padding-left:10px">
                                <telerik:radlabel  ForeColor="Black" ID="lblShowArchive" runat="server" Text="Show Archive"></telerik:radlabel>
                            </td>
                            <td style="padding-left:10px">
                                <telerik:RadCheckBox ID="chkShowArchive" runat="server" />
                            </td>
                        </tr>                       
                    </table>
                    
                        <eluc:TabStrip ID="MenuTravelStatus" runat="server" TabStrip="false" OnTabStripCommand="MenuTravelStatus_TabStripCommand"></eluc:TabStrip>
                   
               <telerik:RadGrid ID="gvTravelStatus" runat="server" AutoGenerateColumns="False" Font-Size="11px" CellPadding="3" RowStyle-Wrap="false" 
                ShowHeader="true" EnableViewState="false" OnItemCommand="gvTravelStatus_ItemCommand" 
                OnItemDataBound= "gvTravelStatus_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvTravelStatus_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" 
                 AllowNaturalSort="false" AutoGenerateColumns="false" TableLayout="Fixed" >
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
                            <telerik:gridtemplatecolumn HeaderText="SI No.">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDROWNUMBER")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                              <telerik:gridtemplatecolumn HeaderText="Passenger Name">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDPASSENGERNAME")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                              <telerik:gridtemplatecolumn HeaderText="Rank">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                               <telerik:gridtemplatecolumn HeaderText="Vessel">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                              <telerik:gridtemplatecolumn HeaderText="Cancel/Refund">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCANCELREFUND")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                               <telerik:gridtemplatecolumn HeaderText="Origin">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDORGIN")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                              <telerik:gridtemplatecolumn HeaderText="Destination">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDDESTINATION")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                          <telerik:gridtemplatecolumn HeaderText="Airline">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDAIRLINECODE")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn HeaderText="PO Number">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDPONUMBER")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                           <telerik:gridtemplatecolumn HeaderText="Invoice Number">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDINVOICENUMBER")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn HeaderText="Invoice Date">
                                <ItemTemplate>
                                    <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDINVOICEDATE"))%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                          <telerik:gridtemplatecolumn HeaderText="Travel Date">
                                <ItemTemplate>
                                    <%#SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTRAVELDATE"))%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                           <telerik:gridtemplatecolumn HeaderText="Basic">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDBASIC")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                         <telerik:gridtemplatecolumn HeaderText="Tax">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDTOTALTAX")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn HeaderText="Service Tax">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDSTXCOLLECTED")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn HeaderText="Discount">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDDISCOUNT")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn HeaderText="Cancel Charges">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCANCELLATIONCHARGE")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn HeaderText="Total Refund">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDTOTAL")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                           <telerik:gridtemplatecolumn HeaderText="Ticket Number">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDTICKETNO")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn HeaderText="Cancel Reason">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDCANCELREASON")%>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                       </Columns>

                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    
                     <Scrolling AllowScroll="True" UseStaticHeaders="true" ></Scrolling>
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>