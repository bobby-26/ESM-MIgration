<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRMidNightReportList.aspx.cs" Inherits="CrewOffshoreDMRMidNightReportList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>--%>
<%--<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>--%>
<%--<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlOffshoreVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DMR MidNight Report List</title>

 <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
           <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvReport.ClientID %>"));
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
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
          
                <eluc:TabStrip ID="MenuReportTab" TabStrip="true" runat="server"></eluc:TabStrip>
         
          
                <table runat="server" id="tblSearch">
                    <tr>
                        <td>
                           
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true" AutoPostBack="true"
                                AppendDataBoundItems="true" Width="250px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucFromDate" runat="server" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucToDate" runat="server" />
                        </td>
                    </tr>
                </table>
            <eluc:TabStrip ID="MenuReportList" runat="server" OnTabStripCommand="ReportList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvReport" RenderMode="Lightweight" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None"
                OnNeedDataSource="gvReport_NeedDataSource" OnItemCommand="gvReport_ItemCommand"
                OnItemDataBound="gvReport_ItemDataBound" EnableHeaderContextMenu="true" >
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView AllowPaging="true" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDMIDNIGHTREPORTID" TableLayout="Fixed" >
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowExportToCsvButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                    <Columns>
                       
                      
                        <telerik:GridTemplateColumn Exportable="false" HeaderStyle-Width="50px">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadImageAndTextTile ID="imgFlag" Height="20px" Width="20px" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <%--   <HeaderTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="75px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" ></ItemStyle>
                            <%-- <HeaderTemplate>
                                <telerik:RadLabel ID="lblReportDate" runat="server" Text="Report Date"></telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReportID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMIDNIGHTREPORTID") %>'></telerik:RadLabel>
                                <asp:LinkButton CommandName="EDIT" ID="lnkReportID" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREPORTDATE")) %>'
                                    runat="server">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Master" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <%--<HeaderTemplate>
                                <telerik:RadLabel ID="lblMasterHdr" runat="server" Text="Master"></telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMaster" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMASTER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CE" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <%-- <HeaderTemplate>
                                <telerik:RadLabel ID="lblChiefEEnggHdr" runat="server" Text="CE"></telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChiefEEngg" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Charter" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" ></ItemStyle>
                            <%--  <HeaderTemplate>
                                <telerik:RadLabel ID="lblVoyageIdHdr" runat="server" Text="Charter Id"></telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVoyageId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOYAGENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" ></ItemStyle>
                            <%-- <HeaderTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--  <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lbletaLocationHrd" runat="server" Text="ETA Location"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                       <asp:Label  ID="lblETALocation" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETALOCATION") %>'
                                            runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                     <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lbletdLocationHrd" runat="server" Text="ETD Location"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                       <asp:Label  ID="lblETDLocation" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETDLOCATION") %>'
                                            runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                        <telerik:GridTemplateColumn HeaderText="Port/Location" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <%--   <HeaderTemplate>
                                <telerik:RadLabel ID="lblPort" runat="server" Text="Port / Location"></telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPortName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ETA" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <%--<HeaderTemplate>
                                <telerik:RadLabel ID="lbletaDate" runat="server" Text="ETA"></telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblETA" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDETADATE")) + " " + DataBinder.Eval(Container,"DataItem.FLDETADATE", "{0:HH:mm}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ETD" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                            <%-- <HeaderTemplate>
                                <telerik:RadLabel ID="lbletdDate" runat="server" Text="ETD"></telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblETD" runat="server"  Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDETDDATE")) + " " + DataBinder.Eval(Container,"DataItem.FLDETDDATE", "{0:HH:mm}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Look Ahead (Day 1)" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <%-- <HeaderTemplate>
                                <telerik:RadLabel ID="lblPlannedActvity1" runat="server" Text="Look Ahead (Day 1)"></telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlannedActvity1Name" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNEXTPLANNEDACTIVITY1").ToString().Length > 20 ? DataBinder.Eval(Container, "DataItem.FLDNEXTPLANNEDACTIVITY1").ToString().Substring(0, 20) + "..." : DataBinder.Eval(Container, "DataItem.FLDNEXTPLANNEDACTIVITY1").ToString()%>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucPlannedActvity1Name" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTPLANNEDACTIVITY1") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Look Ahead (Day 2)" HeaderStyle-Width="150px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <%--  <HeaderTemplate>
                                <telerik:RadLabel ID="lblPlannedActvity2" runat="server" Text="Look Ahead (Day 2)"></telerik:RadLabel>
                            </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlannedActvity2Name" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNEXTPLANNEDACTIVITY2").ToString().Length > 20 ? DataBinder.Eval(Container, "DataItem.FLDNEXTPLANNEDACTIVITY2").ToString().Substring(0, 20) + "..." : DataBinder.Eval(Container, "DataItem.FLDNEXTPLANNEDACTIVITY2").ToString()%>'></telerik:RadLabel>
                                <eluc:Tooltip ID="ucPlannedActvity2Name" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTPLANNEDACTIVITY2") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblPlannedActvity3" runat="server" Text="Look - Ahead (Day 3)"></asp:Label>
                                    </HeaderTemplate>                                    
                                    <ItemTemplate>
                                        <asp:Label ID="lblPlannedActvity3Name" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNEXTPLANNEDACTIVITY3").ToString().Length > 20 ? DataBinder.Eval(Container, "DataItem.FLDNEXTPLANNEDACTIVITY3").ToString().Substring(0, 20) + "..." : DataBinder.Eval(Container, "DataItem.FLDNEXTPLANNEDACTIVITY3").ToString()%>'></asp:Label>
                                        <eluc:Tooltip ID="ucPlannedActvity3Name" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEXTPLANNEDACTIVITY3") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                        <%--<telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    <telerik:RadLabel ID="lbllAction" runat="server" Text="Action"></telerik:RadLabel>
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <%--<img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>--%>
                        <%-- %><img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <telerik:RadImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png%>"
                                    CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                </telerik:RadImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridClientDeleteColumn ConfirmDialogType="RadWindow">
                        </telerik:GridClientDeleteColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Charter per page:" CssClass="RadGrid_Default rgPagerTextBox"/>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
